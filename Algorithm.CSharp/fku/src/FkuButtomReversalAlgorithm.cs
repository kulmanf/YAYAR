using QuantConnect.Brokerages;
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{

    public class FkuButtomReversalAlgorithm : QCAlgorithm
    {
        private FkuUniverseSelection _universe = new FkuUniverseSelection();

        public override void Initialize()
        {
            SetCash(10000); 
            SetBrokerageModel(BrokerageName.InteractiveBrokersBrokerage, AccountType.Cash);   
            _universe.Initialize(this, LiveMode);
       
        }

        public override void OnData(Slice data)
        {
            if (!Portfolio.Invested)
            {
                SetHoldings(_universe.Symbol, 1);
                Debug("Purchased Stock - mighty mac monkey face");
            }
        }
    }
}