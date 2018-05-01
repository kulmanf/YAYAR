using QuantConnect.Brokerages;
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{

    public class FkuButtomReversalAlgorithm : QCAlgorithm
    {
        private const FkuMode Environment = FkuMode.Regression;
        
        private FkuUniverseSelection _universe = new FkuUniverseSelection();

        public override void Initialize()
        {
            SetCash(10000); 
            SetBrokerageModel(BrokerageName.InteractiveBrokersBrokerage, AccountType.Cash);   
            _universe.Initialize(this, Environment);
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