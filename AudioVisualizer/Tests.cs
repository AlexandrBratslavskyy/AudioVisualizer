using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    public class Tests
    {
        static public S SimpleCosineWave()
        {
            S s = new S();
            long N = 10000, A = 200, f = 80;
            for (long t=0; t< N; t++)
            {
                s.Add((int)(A * Math.Cos(2 * Math.PI * f * t /N)));
            }
            return s;
        }
        static public S ComplexCosineWave()
        {
            S s = new S();
            long N = 10000, A = 200, f1 = 80, f2 = 30;
            for (long t = 0; t < N; t++)
            {
                s.Add((int)(A * Math.Cos(2 * Math.PI * f1 * t / N) + A * Math.Cos(2 * Math.PI * f2 * t / N)));
            }
            return s;
        }
    }
}
