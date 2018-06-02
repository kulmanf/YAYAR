using System.Collections.Generic;

namespace QuantConnect.Algorithm.CSharp
{
    internal static class FkuConfiguration
    {
        public const FkuMode Environment = FkuMode.Regression;

        public const decimal WinPercent = 1; // 1%
        public const decimal LosePercent = -0.5m;
        
        public const int startYear = 2018;
        public const int startMonth = 5;
        public const int startDay = 1;

        public const int endYear = 2018;
        public const int endMonth = 5;
        public const int endDay = 31;
    }

    internal class FkuTickers
    {
        public readonly List<string> Tickers = new List<string> {"FB", "MU", "MTCH", "TSLA"};
        
    }
}