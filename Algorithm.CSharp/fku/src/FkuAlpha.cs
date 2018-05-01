using System.Collections.Generic;
using Accord.Diagnostics;
using QuantConnect.Algorithm.Framework.Alphas;
using QuantConnect.Data;
using QuantConnect.Indicators;

namespace QuantConnect.Algorithm.CSharp
{
    internal class FkuAlpha
    {
        private Dictionary<Symbol, SymbolData> _symbolDataBySymbol = new Dictionary<Symbol, SymbolData>();
        private int _period;
        private Resolution _resolution;
        private QCAlgorithm _algorithm;

        internal void Initialize(QCAlgorithm algorithm, Symbol symbol)
        {
            _period = 14;
            _resolution = Resolution.Minute;
            _algorithm = algorithm;

            // initialize data for added securities
            var addedSymbols = new List<Symbol>();
            {
                if (!_symbolDataBySymbol.ContainsKey(symbol))
                {
                    var rsi = algorithm.RSI(symbol, _period, MovingAverageType.Wilders, _resolution);
                    var symbolData = new SymbolData(symbol, rsi);
                    _symbolDataBySymbol[symbol] = symbolData;
                    addedSymbols.Add(symbolData.Symbol);
                }
            }

            if (addedSymbols.Count > 0)
            {
                // warmup our indicators by pushing history through the consolidators
                algorithm.History(addedSymbols, _period, _resolution)
                    .PushThrough(data =>
                    {
                        SymbolData symbolData;
                        if (_symbolDataBySymbol.TryGetValue(data.Symbol, out symbolData))
                        {
                            symbolData.RSI.Update(data.EndTime, data.Value);
                        }
                    });
            }
        }

        internal IEnumerable<Insight> OnData(Slice data)
        {
            var insights = new List<Insight>();
            foreach (var kvp in _symbolDataBySymbol)
            {
                var symbol = kvp.Key;
                var rsi = kvp.Value.RSI;
                var previousState = kvp.Value.State;
                var state = GetState(rsi, previousState);

                if (state != previousState && rsi.IsReady)
                {
                    var insightPeriod = _resolution.ToTimeSpan().Multiply(_period);

                    switch (state)
                    {
                        case State.TrippedLow:
                            insights.Add(new Insight(symbol, InsightType.Price, InsightDirection.Up, insightPeriod));
                            break;

                        case State.TrippedHigh:
                            insights.Add(new Insight(symbol, InsightType.Price, InsightDirection.Down, insightPeriod));
                            break;
                    }
                }

                kvp.Value.State = state;
            }

            insights.ForEach(insight => _algorithm.Debug(insight.ToString()));
            return insights;
        }


        private State GetState(RelativeStrengthIndex rsi, State previous)
        {
            if (rsi > 70m)
            {
                return State.TrippedHigh;
            }

            if (rsi < 30m)
            {
                return State.TrippedLow;
            }

            if (previous == State.TrippedLow)
            {
                if (rsi > 35m)
                {
                    return State.Middle;
                }
            }

            if (previous == State.TrippedHigh)
            {
                if (rsi < 65m)
                {
                    return State.Middle;
                }
            }

            return previous;
        }

        private class SymbolData
        {
            public Symbol Symbol { get; }
            public State State { get; set; }
            public RelativeStrengthIndex RSI { get; }

            public SymbolData(Symbol symbol, RelativeStrengthIndex rsi)
            {
                Symbol = symbol;
                RSI = rsi;
                State = State.Middle;
            }
        }

        private enum State
        {
            TrippedLow,
            Middle,
            TrippedHigh
        }
    }
}