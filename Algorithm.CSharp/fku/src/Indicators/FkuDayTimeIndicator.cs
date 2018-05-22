using System;
using Accord;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuDayTimeIndicator
    {
        private QCAlgorithm _algorithm;
        
        public FkuDayTimeIndicator(QCAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public bool IsBetween0930And1000()
        {
            var isInHour = _algorithm.Time.Hour >= 9 && _algorithm.Time.Hour <= 10;
            var isInMinuts = _algorithm.Time.Minute >= 30;
            return isInMinuts && isInHour;
        }
    }
}