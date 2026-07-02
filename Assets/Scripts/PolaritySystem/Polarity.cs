namespace PolarityBreach.PolaritySystem
{
    public enum Polarity
    {
        Black,
        White
    }

    public static class PolarityExtensions
    {
        public static Polarity Opposite(this Polarity polarity)
        {
            return polarity == Polarity.Black ? Polarity.White : Polarity.Black;
        }
    }
}