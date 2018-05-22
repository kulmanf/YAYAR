using System.Collections.Generic;
using QuantConnect.Data;
using QuantConnect.Data.Market;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuBuyer
    {
        private QCAlgorithm _algorithm;
        private List<Symbol> _symbols;

        internal void Initialize(QCAlgorithm algorithm, List<Symbol> symbols)
        {
            _algorithm = algorithm;
            _symbols = symbols;
        }

        internal List<FkuInsightSignal> OnData(Slice data)
        {
            var signals = new List<FkuInsightSignal>();
            foreach (var symbol in _symbols)
            {
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
            // Yesterday green
            // RSI < 30
            // TimeRange 09:30h - 10:00h
            
            
            return new FkuInsightSignal(symbol, Advice.None, 0);
        }

        private void Log(string message)
        {
            var stamp = _algorithm.Time + " [FkuBuyer] ";
            _algorithm.Log(stamp + message);
        }
    }
}