namespace QuantConnect.Algorithm.CSharp
{
    internal static class FkuConfiguration
    {
        public const FkuMode Environment = FkuMode.Regression;
        
        public const decimal WinPercent = 1; // 1%
        public const decimal LosePercent = -0.5m;
    }
}