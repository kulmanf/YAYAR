using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{
    public class FkuPortfolio
    {
       
            internal void Initialize()
            {
                // Warm up algo - get open orders / IB information
            }

            internal void OnData(Slice data)
            {
                // Update position size
                
                // -> 2500 / price
                // no amount if invested
            }
    }
}