using System.Collections.Generic;
using QuantConnect.Data;
using QuantConnect.Indicators;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuRsiIndicator
    {
        private readonly int _period;
        private readonly Resolution _resolution;
        private readonly QCAlgorithm _algorithm;
        private readonly Symbol _symbol;

        private RelativeStrengthIndex _rsi;

        public FkuRsiIndicator(QCAlgorithm algorithm, Symbol symbol)
        {
            _period = 14;
            _resolution = Resolution.Minute;
            _algorithm = algorithm;
            _symbol = symbol;
            _rsi = _algorithm.RSI(_symbol, _period, MovingAverageType.Wilders, _resolution);
            
            // warmup our indicator by pushing history through the consolidator
            _algorithm.History(new List<Symbol> {_symbol}, _period, _resolution)
                .PushThrough(data => _rsi.Update(data.EndTime, data.Value));
        }

        public bool IsBelow30()
        {
            var isBelow30 = false;

            if (_rsi.IsReady)
            {
                isBelow30 = _rsi < 30;
            }
            else
            {
                _algorithm.Log("RSI not ready!!! :(");
            }

            return isBelow30;
        }
    }
}