using System.Collections.Generic;
using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuBuyer
    {
        private QCAlgorithm _algorithm;
        private List<Symbol> _symbols;
        
        internal void Initialize(QCAlgorithm algorithm, List<Symbol> symbols)
        {
            _algorithm = algorithm;
            _symbols = symbols;

            
       
        }

        internal List<bool> OnData(Slice data)
        {
     
                
            return new List<bool>();
        }
        
        internal void LogDaily()
        {

        }
        
        private void Log(string message)
        {
            var stamp = _algorithm.Time + " [FkuBuyer] "; 
            _algorithm.Log(stamp + message);
        }
    }
}