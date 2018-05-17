using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using QuantConnect.Data;
using QuantConnect.Securities;
using SecurityManager = QuantConnect.Securities.SecurityManager;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuSeller
    {
        private QCAlgorithm _algorithm;
        private SecurityPortfolioManager _portfolioManager;
        
        internal void Initialize(QCAlgorithm algorithm)
        {
            _algorithm = algorithm;
            _portfolioManager = algorithm.Portfolio;
        }

        internal List<bool> OnData(Slice data)
        {
            if (!_portfolioManager.Invested) return new List<bool>();

            var securityHoldings = _portfolioManager.Values;
            return securityHoldings.Where(securityHolding =>
                {
                    var isWin = IsSellWin(securityHolding.AveragePrice, securityHolding.Price);
                    var isStop = IsSellStop(securityHolding.AveragePrice, securityHolding.Price);
                    return isWin || isStop;
                }).Select(securityHolding => true).ToList();
        }
        
        internal void LogDaily()
        {
        }
        
        private void Log(string message)
        {
            var stamp = _algorithm.Time + " [FkuSeller] "; 
            _algorithm.Log(stamp + message);
        }

        private bool IsSellWin(decimal buyPrice, decimal currentPrice)
        {
            var percentDiff = PercentDiff(buyPrice, currentPrice);
            return percentDiff > FkuConfiguration.WinPercent;
        }
        
        private bool IsSellStop(decimal buyPrice, decimal currentPrice)
        {
            var percentDiff = PercentDiff(buyPrice, currentPrice);
            return percentDiff < FkuConfiguration.LosePercent;
        }

        public static decimal PercentDiff(decimal oldPrice, decimal newPrice)
        {
            var diff = newPrice - oldPrice;
            return diff / Math.Max(oldPrice, 1) * 100;
        }
    }
}