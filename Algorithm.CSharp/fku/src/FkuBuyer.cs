using System.Collections.Generic;
using QuantConnect.Data;
using QuantConnect.Data.Market;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuBuyer
    {
        private QCAlgorithm _algorithm;
        private List<Symbol> _symbols;
        private FkuDayTimeIndicator _dayTimeIndicator;
        private Dictionary<Symbol, FkuYesterdayIndicator> _yesterdayIndicators;
        private Dictionary<Symbol, FkuRsiIndicator> _rsiIndicators;

        internal void Initialize(QCAlgorithm algorithm, List<Symbol> symbols)
        {
            _algorithm = algorithm;
            _symbols = symbols;
            _dayTimeIndicator = new FkuDayTimeIndicator(algorithm);
            _yesterdayIndicators = new Dictionary<Symbol, FkuYesterdayIndicator>();
            _rsiIndicators = new Dictionary<Symbol, FkuRsiIndicator>();
            foreach (var symbol in _symbols)
            {
                _yesterdayIndicators.Add(symbol, new FkuYesterdayIndicator(algorithm, symbol));
                _rsiIndicators.Add(symbol, new FkuRsiIndicator(algorithm, symbol));
            }
        }

        internal List<FkuInsightSignal> OnData(Slice data)
        {
            var signals = new List<FkuInsightSignal>();
            foreach (var symbol in _symbols)
            {
                if(!data.Bars.ContainsKey(symbol)) continue;
                
                var signal = InsightForSymbol(symbol, data.Bars[symbol]);
                signals.Add(signal);
            }
            return signals;
        }

        internal void LogDaily()
        {
        }

        private FkuInsightSignal InsightForSymbol(Symbol symbol, TradeBar bar)
        {
            var isInTime = _dayTimeIndicator.IsBetween0930And1000();
            var isBelow30 = _rsiIndicators[symbol].IsBelow30();
            var isYesterdayGreen = _yesterdayIndicators[symbol].IsYesterdayGreen();

            Log(symbol + " isInTime: " + isInTime + " isBelow30: " + isBelow30 + " isYesterdayGreen: " + isYesterdayGreen);
            
            var advice = isInTime && isBelow30 && isYesterdayGreen ? Advice.Buy : Advice.None;
            
            return new FkuInsightSignal(symbol, advice, 1);
        }

        private void Log(string message)
        {
            var stamp = _algorithm.Time + " [FkuBuyer] ";
            _algorithm.Log(stamp + message);
        }
    }
}