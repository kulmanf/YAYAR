using System;
using System.Collections.Generic;
using System.Linq;
using Accord;
using Accord.Math;
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuExecutor
    {
        private QCAlgorithm _algorithm;
        private List<Symbol> _symbols;

        internal void Initialize(QCAlgorithm algorithm, List<Symbol> symbols)
        {
            _algorithm = algorithm;
            _symbols = symbols;
        }

        internal void OnBuy(Slice data, List<FkuInsightSignal> signals, Dictionary<Symbol, int> positionSizes)
        {
            signals.ForEach(signal =>
            {
                if (signal.Advice == Advice.Buy)
                {
                    Log(signal.Symbol + " " + signal.Advice + " " + signal.Strength);
                    
                    foreach (var positionSizesKey in positionSizes.Keys)
                    {
                        Log("position size: " + positionSizesKey + " " + positionSizes[positionSizesKey]);
                    }
                }
            });


            foreach (var signal in signals)
            {
                var symbol = signal.Symbol;
                var size = positionSizes[signal.Symbol];

                if (_algorithm.Transactions.GetOpenOrders().Count > 0) return;

                if (signal.Advice == Advice.None) continue;

                if (positionSizes[symbol] == 0) continue;

                if (!_algorithm.IsMarketOpen(symbol)) continue;

                var orderTicket = _algorithm.MarketOrder(symbol, size);
                Log("Ordered: " + orderTicket.Symbol + " " + orderTicket.Quantity);
            }
        }

        internal void OnSell(List<FkuInsightSignal> signals)
        {
            if (_algorithm.Transactions.GetOpenOrders().Count > 0) return;

            foreach (var signal in signals)
            {
                var symbol = signal.Symbol;

                if (signal.Advice != Advice.Sell) continue;

                if (!_algorithm.IsMarketOpen(symbol)) continue;

                _algorithm.SetHoldings(symbol, 0);
                Log("Set holdings to zero: " + symbol);
            }
        }

        internal void LogDaily()
        {
            Log("Invested: " + _algorithm.Portfolio.Invested);
            Log("Open Orders: " + _algorithm.Transactions.GetOpenOrders().Count);
        }

        private void Log(string message)
        {
            var stamp = _algorithm.Time + " [FkuExecutor] ";
            _algorithm.Log(stamp + message);
        }
    }
}