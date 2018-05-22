namespace QuantConnect.Algorithm.CSharp
{
    public enum Advice
    {
        Buy,
        Sell,
        None
    }

    public class FkuInsightSignal
    {
        public readonly Symbol Symbol;
        public readonly Advice Advice;
        public readonly decimal Strength;
        
        public FkuInsightSignal(Symbol symbol, Advice advice, decimal strength)
        {
            Symbol = symbol;
            Advice = advice;
            Strength = strength;
        }
    }
}