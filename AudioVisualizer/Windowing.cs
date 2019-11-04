﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    public abstract class Windowing
    {
        abstract protected double Equation(long n, long N);
        public S CreateWindow(S OGs, long N)
        {
            S NEWs = new S();
            double[] W = new double[N];
            for (long n = 0; n < N; n++)
            {
                W[n] = Equation(n, N);
            }

            for (long t = 0, n = 0; n < OGs.Size(); t++)
            {
                NEWs.Add((int)(OGs.Get(t) * W[(int)n]));
                n++;
                if (n == N)
                    n = 0;
            }
            return NEWs;
        }

        //statics
        static public Windowing WINDOW = new RectangleWindow();
        static public void ChangeFilter(int newWindow)
        {
            switch (newWindow)
            {
                case 3:
                    WINDOW = new HanningWindow();
                    break;
                case 2:
                    WINDOW = new WelchWindow();
                    break;
                case 1:
                    WINDOW = new TriangleWindow();
                    break;
                case 0:
                default:
                    WINDOW = new RectangleWindow();
                    break;
            }
        }
    }
    public class RectangleWindow : Windowing
    {
        //apply triangle windowing on samples
        protected override double Equation(long n, long N)
        {
            return 1;
        }
    }
    public class TriangleWindow : Windowing
    {
        //apply triangle windowing on samples
        protected override double Equation(long n, long N)
        {
            return 1 - Math.Abs((n - (N - 1) / 2) / (N / 2));
        }
    }
    public class WelchWindow : Windowing
    {
        //apply Welch windowing on samples
        protected override double Equation(long n, long N)
        {
            return 1 - (Math.Pow(((n - ((N - 1) / 2)) / ((N - 1) / 2)), 2));
        }
    }
    public class HanningWindow : Windowing
    {
        //apply hanning windowing on samples
        protected override double Equation(long n, long N)
        {
            return 0.5 * (1 - Math.Cos((2 * Math.PI * n) / (N - 1)));
        }
    }
}