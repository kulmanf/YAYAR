using QuantConnect.Brokerages;
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{

    public class FkuButtomReversalAlgorithm : QCAlgorithm
    {
        private const FkuMode Environment = FkuMode.Regression;
        
        private FkuUniverseSelection _universe = new FkuUniverseSelection();
        private FkuAlphaCreation _alpha = new FkuAlphaCreation();

        public override void Initialize()
        {
            SetCash(10000); 
            SetBrokerageModel(BrokerageName.InteractiveBrokersBrokerage, AccountType.Cash);   
            _universe.Initialize(this, Environment);
            _alpha.Initialize();
        }

        public override void OnData(Slice data)
        {
            _alpha.OnData(data);
            
            if (!Portfolio.Invested)
            {
                SetHoldings(_universe.Symbol, 1);
                Debug("Purchased Stock - mighty mac monkey face");
            }
        }
    }
}