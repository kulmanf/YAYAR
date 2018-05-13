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
        private const decimal WinPercent = 1; // 1%
        private const decimal LosePercent = -0.5m;
        
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
            var message = _algorithm.Time + " - FkuSeller - ";
            _algorithm.Log(message);
        }

        private bool IsSellWin(decimal buyPrice, decimal currentPrice)
        {
            var percentDiff = PercentDiff(buyPrice, currentPrice);
            return percentDiff > WinPercent;
        }
        
        private bool IsSellStop(decimal buyPrice, decimal currentPrice)
        {
            var percentDiff = PercentDiff(buyPrice, currentPrice);
            return percentDiff < LosePercent;
        }

        private static decimal PercentDiff(decimal oldPrice, decimal newPrice)
        {
            var diff = newPrice - oldPrice;
            return diff / oldPrice * 100;
        }
    }
}