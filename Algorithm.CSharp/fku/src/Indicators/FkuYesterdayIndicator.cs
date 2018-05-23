using System.Linq;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuYesterdayIndicator
    {
        private readonly Symbol _symbol;
        private readonly QCAlgorithm _algorithm;

        public FkuYesterdayIndicator(QCAlgorithm algorithm, Symbol symbol)
        {
            _symbol = symbol;
            _algorithm = algorithm;
        }

        public bool IsYesterdayGreen()
        {
            var bars = _algorithm.History(_symbol, 1, Resolution.Daily);
            var yesterday = bars.Last();
            return yesterday.Close > yesterday.Open;
        }
    }
}