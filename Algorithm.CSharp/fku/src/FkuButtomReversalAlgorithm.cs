using System.Collections.Generic;
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
        private FkuRiskManager _risk = new FkuRiskManager();

        public override void Initialize()
        {
            SetCash(10000);
            SetBrokerageModel(BrokerageName.InteractiveBrokersBrokerage, AccountType.Cash);
            _universe.Initialize(this, Environment);
            _alpha.Initialize(this, _universe.Symbol);
            _portfolio.Initialize(this, _universe.Symbol);
            _seller.Initialize(this);
            _executor.Initialize(this, _universe.Symbol);
            _risk.Initialize(this);

            Schedule.On(DateRules.EveryDay(), TimeRules.At(8,0), () =>
            {
                _universe.LogDaily();
                _alpha.LogDaily();
                _portfolio.LogDaily();
                _seller.LogDaily();
                _executor.LogDaily();
                _risk.LogDaily();
            });
        }

        public override void OnData(Slice data)
        {
            var signals = _alpha.OnData(data);
            var positionSize = _portfolio.OnData(data);
            var sellSignals = _seller.OnData(data);
            _executor.OnBuy(data, signals, positionSize);
            _executor.OnSell(sellSignals);
            _risk.OnData(data);

//            if (!Portfolio.Invested && Environment.IsEqual(FkuMode.Regression))
//            {
//                SetHoldings(_universe.Symbol, 1);
//                Debug("Purchased Stock - mighty mac monkey face");
//            }
        }
    }
}