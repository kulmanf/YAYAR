using System;
using System.Collections.Generic;
using System.Linq;

namespace QuantConnect.Algorithm.CSharp
{
    internal enum FkuMode
    {
        InteractiveBrokers,
        Backtest,
        Regression
    }

    internal class FkuUniverse
    {
        private QCAlgorithm _algorithm;
        
        internal List<Symbol> Symbols { get; private set; }
        
        internal void Initialize(QCAlgorithm algorithm, FkuMode fkuMode)
        {
            _algorithm = algorithm;
            var tickers = new FkuTickers().Tickers;
            var symbols = tickers.Select(ticker => Symbol.Create(ticker, SecurityType.Equity, Market.USA)).ToList();
         
            switch (fkuMode)
            {
                case FkuMode.InteractiveBrokers:
                    Symbols = symbols;
                    break;
                case FkuMode.Backtest:
                    Symbols = symbols;
                    algorithm.SetStartDate(2018, 04, 01); //Set Start Date
                    algorithm.SetEndDate(2018, 04, 30); //Set End Date
                    break;
                case FkuMode.Regression:
                    var spy = Symbol.Create("SPY", SecurityType.Equity, Market.USA);
                    Symbols = new List<Symbol>{spy};
                    algorithm.SetStartDate(2013, 10, 07); //Set Start Date
                    algorithm.SetEndDate(2013, 10, 11); //Set End Date
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Symbols.ForEach(symbol => algorithm.AddEquity(symbol, extendedMarketHours: true));
        }

        internal void LogDaily()
        {
            Log("Symbols: " + Symbols);
        }
        
        private void Log(string message)
        {
            var stamp = _algorithm.Time + " [FkuUniverse] "; 
            _algorithm.Log(stamp + message);
        }

    }
}