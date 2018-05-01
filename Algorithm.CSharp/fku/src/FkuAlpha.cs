using QuantConnect.Data;

namespace QuantConnect.Algorithm.CSharp
{
    internal enum FkuInsight
    {
        Buy, 
        Hold,
        Sell
    }
    
    internal class FkuAlpha
    {
        internal FkuInsight Insight { get; private set; }
        
        internal void Initialize()
        {
            Insight = FkuInsight.Hold;

            // Warm up algo
        }

        internal void OnData(Slice data)
        {
            // Update Algo
            
        }
    }
}