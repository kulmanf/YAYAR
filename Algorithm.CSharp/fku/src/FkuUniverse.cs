﻿using System;

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
        internal void Initialize(QCAlgorithm algorithm, FkuMode fkuMode)
        {
            switch (fkuMode)
            {
                case FkuMode.InteractiveBrokers:
                    Symbol = Symbol.Create("FB", SecurityType.Equity, Market.USA);
                    algorithm.AddEquity(Symbol, Resolution.Minute);
                    break;
                case FkuMode.Backtest:
                    Symbol = Symbol.Create("FB", SecurityType.Equity, Market.USA);
                    algorithm.SetStartDate(2018, 04, 25); //Set Start Date
                    algorithm.SetEndDate(2018, 04, 28); //Set End Date
                    algorithm.AddEquity(Symbol, Resolution.Minute);
                    break;
                case FkuMode.Regression:
                    Symbol = Symbol.Create("SPY", SecurityType.Equity, Market.USA);
                    algorithm.SetStartDate(2013, 10, 07); //Set Start Date
                    algorithm.SetEndDate(2013, 10, 11); //Set End Date
                    algorithm.AddEquity("SPY", Resolution.Minute);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fkuMode), fkuMode, null);
            }
        }

        internal Symbol Symbol { get; private set; }
    }
}