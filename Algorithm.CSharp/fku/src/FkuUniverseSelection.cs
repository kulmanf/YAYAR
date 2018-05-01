using System.Runtime.InteropServices;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuUniverseSelection
    {
       
        public void Initialize(QCAlgorithm algorithm, bool liveMode)
        {
            if (liveMode)
            {
                Symbol = QuantConnect.Symbol.Create("FB", SecurityType.Equity, Market.USA);
                algorithm.SetStartDate(2018, 04, 25); //Set Start Date
                algorithm.SetEndDate(2018, 04, 28); //Set End Date
                algorithm.AddEquity("FB", Resolution.Minute);
            }
            else
            {
                Symbol = QuantConnect.Symbol.Create("SPY", SecurityType.Equity, Market.USA);
                algorithm.SetStartDate(2013, 10, 07); //Set Start Date
                algorithm.SetEndDate(2013, 10, 11); //Set End Date
                algorithm.AddEquity("SPY", Resolution.Minute);
            }
        }
        
        public Symbol Symbol
        {
            get;
            private set;
        }
    }
}