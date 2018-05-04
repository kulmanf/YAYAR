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

        internal void OnData(Slice data, List<FkuAlpha.Signal> signals, int positionSize)
        {
            if (signals.Count == 0) return;
            
            if (positionSize == 0) return;
            
            if (!_algorithm.IsMarketOpen(_symbol)) return;

            if(_algorithm.Portfolio.Invested) return;

            if (_algorithm.Transactions.GetOpenOrders().Count > 0) return;

            var orderTicket = _algorithm.MarketOrder(_symbol, positionSize);
            _algorithm.Debug("Ordered: " + orderTicket.Symbol + " " + orderTicket.Quantity);
        }
    }
}