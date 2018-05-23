using System.Linq;
using QuantConnect.Brokerages;
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuButtomReversalAlgorithm : QCAlgorithm
    {
        private FkuUniverse _universe = new FkuUniverse();
        private FkuPortfolio _portfolio = new FkuPortfolio();
        private FkuSeller _seller = new FkuSeller();
        private FkuBuyer _buyer = new FkuBuyer();
        private FkuExecutor _executor = new FkuExecutor();
        private FkuRiskManager _risk = new FkuRiskManager();

        public override void Initialize()
        {
            SetCash(10000);
            SetBrokerageModel(BrokerageName.InteractiveBrokersBrokerage, AccountType.Cash);
            _universe.Initialize(this, FkuConfiguration.Environment);
            _portfolio.Initialize(this, _universe.Symbol);
            _buyer.Initialize(this, _universe.GetSymbols());
            _seller.Initialize(this);
            _executor.Initialize(this, _universe.Symbol);
            _risk.Initialize(this);

            Schedule.On(DateRules.EveryDay(), TimeRules.At(8,0), () =>
            {
                _universe.LogDaily();
                _portfolio.LogDaily();
                _buyer.LogDaily();
                _seller.LogDaily();
                _executor.LogDaily();
                _risk.LogDaily();
            });
        }

        public override void OnData(Slice data)
        {
            var positionSize = _portfolio.OnData(data);
            var buySignals = _buyer.OnData(data);
            var sellSignals = _seller.OnData(data);
            _executor.OnBuy(data, buySignals, positionSize);
            _executor.OnSell(sellSignals);
            _risk.OnData(data);
        }
    }
}