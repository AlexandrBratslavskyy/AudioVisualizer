using System;

namespace AudioVisualizer
{
    public abstract class Windowing
    {
        abstract protected double Equation(ref long n, ref long N);
        public S CreateWindow(ref S OGs, ref long N)
        {
            S NEWs = new S();
            double[] W = new double[N];
            for (long n = 0; n < N; n++)
            {
                W[n] = Equation(ref n, ref N);
            }

            for (long t = 0, n = 0; t < OGs.Size(); t++)
            {
                NEWs.Add((int)(OGs.Get(t) * W[(int)n]));
                n++;
                if (n == N)
                    n = 0;
            }
            return NEWs;
        }
    }
    public class RectangleWindow : Windowing
    {
        //apply triangle windowing on samples
        protected override double Equation(ref long n, ref long N)
        {
            return 1;
        }
    }
    public class TriangleWindow : Windowing
    {
        //apply triangle windowing on samples
        protected override double Equation(ref long n, ref long N)
        {
            return 1 - Math.Abs((n - (N - 1) / 2) / (N / 2));
        }
    }
    public class WelchWindow : Windowing
    {
        //apply Welch windowing on samples
        protected override double Equation(ref long n, ref long N)
        {
            return 1 - (Math.Pow(((n - ((N - 1) / 2)) / ((N - 1) / 2)), 2));
        }
    }
    public class HanningWindow : Windowing
    {
        //apply hanning windowing on samples
        protected override double Equation(ref long n, ref long N)
        {
            return 0.5 * (1 - Math.Cos((2 * Math.PI * n) / (N - 1)));
        }
    }
}
