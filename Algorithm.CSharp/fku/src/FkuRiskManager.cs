using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuRiskManager
    {
        private QCAlgorithm _algorithm;
        
        internal void Initialize(QCAlgorithm algorithm)
        {
            _algorithm = algorithm;

            // Get open stop orders
        }

        internal void OnData(Slice data)
        {
            // Is Invested?
            
            // Are there open stop orders?
            
            // Place market stop order (-5%)
                
        }
        
        internal void LogDaily()
        {

        }
        
        private void Log(string message)
        {
            var stamp = _algorithm.Time + " [FkuRiskManager] "; 
            _algorithm.Log(stamp + message);
        }
    }
}