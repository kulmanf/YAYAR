using System.Collections.Generic;
using System.Linq;
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuExecutor
    {
        private QCAlgorithm _algorithm;
        private Symbol _symbol;

        internal void Initialize(QCAlgorithm algorithm, Symbol symbol)
        {
            _algorithm = algorithm;
            _symbol = symbol;
        }

        internal void OnBuy(Slice data, List<FkuAlpha.Signal> signals, int positionSize)
        {
            if (signals.Count == 0) return;
            
            if (positionSize == 0) return;
            
            if (!_algorithm.IsMarketOpen(_symbol)) return;

            if(_algorithm.Portfolio.Invested) return;

            if (_algorithm.Transactions.GetOpenOrders().Count > 0) return;

            var orderTicket = _algorithm.MarketOrder(_symbol, positionSize);
            _algorithm.Debug("Ordered: " + orderTicket.Symbol + " " + orderTicket.Quantity);
        }

        internal void OnSell(List<bool> signals)
        {
            if (!signals.Contains(true)) return;
            
            if (!_algorithm.IsMarketOpen(_symbol)) return;

            if(_algorithm.Portfolio.Invested) return;

            if (_algorithm.Transactions.GetOpenOrders().Count > 0) return;

            _algorithm.SetHoldings(_symbol, 0);
            _algorithm.Debug("Set holdings to zero: " + _symbol);
        }
        
        internal void LogDaily()
        {
            var message = _algorithm.Time + " - FkuExecutor - ";
            _algorithm.Log(message);
        }
    }
}