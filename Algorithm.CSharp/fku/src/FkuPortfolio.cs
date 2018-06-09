using System;
using System.Collections.Generic;
using System.Linq;
using QuantConnect.Data;
using QuantConnect.Orders;
using QuantConnect.Securities;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuPortfolio
    {
        private const decimal MAX_POSITION_VALUE = 2500;
        
        private SecurityPortfolioManager _portfolioManager;
        private SecurityTransactionManager _transactionManager;
        private QCAlgorithm _algorithm;
        private List<Symbol> _symbols;

        internal void Initialize(QCAlgorithm algorithm, List<Symbol> symbols)
        {
            _algorithm = algorithm;
            _portfolioManager = algorithm.Portfolio;
            _transactionManager = algorithm.Transactions;
            _symbols = symbols;
        }

        internal Dictionary<Symbol, int> OnData(Slice data)
        {
            var positionSizes = new Dictionary<Symbol, int>();
            _symbols.ForEach(symbol => positionSizes.Add(symbol, 0));
            
            foreach (var symbol in _symbols)
            {
                if (availableCash() > MAX_POSITION_VALUE && data.Bars.ContainsKey(symbol))
                {
                    var price = data.Bars[symbol].Price;
                    positionSizes[symbol] = postionSize(price);
                }
            }    
            return positionSizes;
        }
            
        internal void LogDaily()
        {
            Log("Cash: " + _portfolioManager.Cash);
            Log("Number of securities: " + _portfolioManager.Count);
            Log("TotalPortfolioValue: " + _portfolioManager.TotalPortfolioValue);
            _portfolioManager.Values.ToList().ForEach(securityHolding =>
            {
                Log("SecurityHolding: "+ securityHolding.Symbol 
                               + " AveragePrice " + Math.Round(securityHolding.AveragePrice, 3)
                               + " Price " + Math.Round(securityHolding.Price,3)
                               + " PercentDiff " + Math.Round(FkuSeller.PercentDiff(securityHolding.AveragePrice, securityHolding.Price), 3)
                               + " Quantity " + securityHolding.Quantity);
            });    
        }
        
        private void Log(string message)
        {
            var stamp = _algorithm.Time + " [FkuPortfolio] "; 
            _algorithm.Log(stamp + message);
        }

        private int postionSize(decimal price)
        {
            return Convert.ToInt32(Math.Floor(MAX_POSITION_VALUE / price));
        }

        private decimal availableCash()
        {
            return _portfolioManager.Cash - sumOfOpenOrders();
        }
        
        private decimal sumOfOpenOrders()
        {
            if (_transactionManager.OrdersCount == 0) return 0;

            return _transactionManager.GetOpenOrders()
                .Where(order => order.Direction == OrderDirection.Buy)
                .Where(order => order.Status != OrderStatus.Filled)
                .Sum(order => order.Price);
        }
    }
}