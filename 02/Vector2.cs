namespace ZPG
{
    /// <summary>
    /// Dvourozměrný vektor
    /// </summary>
    public class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2(double x, double y) {  X = x; Y = y; }

        public override string ToString()
        {
            return $"[{X:F2}; {Y:F2}]";
        }
    }
}
