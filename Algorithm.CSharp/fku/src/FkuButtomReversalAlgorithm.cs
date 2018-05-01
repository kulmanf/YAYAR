using Accord.Math;
using QuantConnect.Brokerages;
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuButtomReversalAlgorithm : QCAlgorithm
    {
        private const FkuMode Environment = FkuMode.Regression;

        private FkuUniverse _universe = new FkuUniverse();
        private FkuAlpha _alpha = new FkuAlpha();
        private FkuPortfolio _portfolio = new FkuPortfolio();
        private FkuSeller _seller = new FkuSeller();
        private FkuExecutor _executor = new FkuExecutor();
        private FkuRiskManagement _risk = new FkuRiskManagement();

        public override void Initialize()
        {
            SetCash(10000);
            SetBrokerageModel(BrokerageName.InteractiveBrokersBrokerage, AccountType.Cash);
            _universe.Initialize(this, Environment);
            _alpha.Initialize(this, _universe.Symbol);
            _portfolio.Initialize();
            _seller.Initialize();
            _executor.Initialize();
            _risk.Initialize();
        }

        public override void OnData(Slice data)
        { 
            _alpha.OnData(data);
            _portfolio.OnData(data);
            _seller.OnData(data);
            _executor.OnData(data);
            _risk.OnData(data);

            if (!Portfolio.Invested && Environment.IsEqual(FkuMode.Regression))
            {
                SetHoldings(_universe.Symbol, 1);
                Debug("Purchased Stock - mighty mac monkey face");
            }
        }
    }
}