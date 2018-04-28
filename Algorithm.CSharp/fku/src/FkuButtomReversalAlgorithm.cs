using QuantConnect.Brokerages;
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{

    public class FkuButtomReversalAlgorithm : QCAlgorithm
    {
        private Symbol _symbol = QuantConnect.Symbol.Create("FB", SecurityType.Equity, Market.USA);

        public override void Initialize()
        {
            SetCash(10000); 
            SetBrokerageModel(BrokerageName.InteractiveBrokersBrokerage, AccountType.Cash);
            
            // Regression
            _symbol = QuantConnect.Symbol.Create("SPY", SecurityType.Equity, Market.USA);
            SetStartDate(2013, 10, 07);  //Set Start Date
            SetEndDate(2013, 10, 11);    //Set End Date
            AddEquity("SPY", Resolution.Minute);
            
            // Backtest
//            SetStartDate(2018, 04, 25);  //Set Start Date
//            SetEndDate(2018, 04, 28);    //Set End Date
//            AddEquity("FB", Resolution.Minute);
            
            // Live
            
            
        }

        public override void OnData(Slice data)
        {
            if (!Portfolio.Invested)
            {
                SetHoldings(_symbol, 1);
                Debug("Purchased Stock - mighty mac monkey face");
            }
        }
    }
}