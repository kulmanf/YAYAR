using System;
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
        private Symbol _symbol;

        internal void Initialize(QCAlgorithm algorithm, Symbol symbol)
        {
            _algorithm = algorithm;
            _portfolioManager = algorithm.Portfolio;
            _transactionManager = algorithm.Transactions;
            _symbol = symbol;
        }

        internal int OnData(Slice data)
        {
            if (_portfolioManager.Invested) return 0;

            if (_transactionManager.GetOpenOrders().Count > 0) return 0;
            
            if (availableCash() > MAX_POSITION_VALUE)
            {
                var price = data.Bars[_symbol].Price;
                return postionSize(price);
            }
            return 0;
        }
            
        internal void LogDaily()
        {
            var stamp = _algorithm.Time + " [FkuPortfolio]";
            _algorithm.Log(stamp + " Cash: " + _portfolioManager.Cash);
            _algorithm.Log(stamp + " Number of securities: " + _portfolioManager.Count);
            _algorithm.Log(stamp + " TotalPortfolioValue: " + _portfolioManager.TotalPortfolioValue);
            _portfolioManager.Values.ToList().ForEach(securityHolding =>
            {
                _algorithm.Log(stamp + " SecurityHolding: "+ securityHolding.Symbol 
                               + " AveragePrice " + Math.Round(securityHolding.AveragePrice, 3)
                               + " Price " + Math.Round(securityHolding.Price,3)
                               + " PercentDiff " + Math.Round(FkuSeller.PercentDiff(securityHolding.AveragePrice, securityHolding.Price), 3)
                               + " Quantity " + securityHolding.Quantity);
            });    
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