using System;
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

        internal void OnBuy(Slice data, List<FkuInsightSignal> signals, int positionSize)
        {
            var signal = signals.Last();
            
            if(signal.Advice == Advice.None) return;
            
            if (positionSize == 0) return;
            
            if (!_algorithm.IsMarketOpen(_symbol)) return;

            if(_algorithm.Portfolio.Invested) return;

            if (_algorithm.Transactions.GetOpenOrders().Count > 0) return;

            var orderTicket = _algorithm.MarketOrder(_symbol, positionSize);
            Log("Ordered: " + orderTicket.Symbol + " " + orderTicket.Quantity);
        }

        internal void OnSell(List<bool> signals)
        {
            if (!signals.Contains(true)) return;
            
            if (!_algorithm.IsMarketOpen(_symbol)) return;

            if(!_algorithm.Portfolio.Invested) return;

            if (_algorithm.Transactions.GetOpenOrders().Count > 0) return;

            _algorithm.SetHoldings(_symbol, 0);
            Log("Set holdings to zero: " + _symbol);
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