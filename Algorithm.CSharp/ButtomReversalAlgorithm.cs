using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{

    public class ButtomReversalAlgorithm : QCAlgorithm
    {
        private Symbol _spy = QuantConnect.Symbol.Create("SPY", SecurityType.Equity, Market.USA);

        public override void Initialize()
        {
            SetStartDate(2013, 10, 07);  //Set Start Date
            SetEndDate(2013, 10, 11);    //Set End Date
            SetCash(100000);             //Set Strategy Cash

            AddEquity("SPY", Resolution.Minute);
        }

        public override void OnData(Slice data)
        {
            if (!Portfolio.Invested)
            {
                SetHoldings(_spy, 1);
                Debug("Purchased Stock");
                
            }
        }
    }
}