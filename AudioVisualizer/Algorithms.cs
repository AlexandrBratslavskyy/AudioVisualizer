using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    class Algorithms
    {
        //statics
        static public A DFT(S s, long N)
        {
            A a = new A();
            for (long f = 0; f < N; ++f)
            {
                // slow, needs threading
                double real = 0, imm = 0;
                for (long t = 0; t < N; ++t)
                {
                    real += s.Get(t) * Math.Cos(2 * Math.PI * t * f / N);
                    imm -= s.Get(t) * Math.Sin(2 * Math.PI * t * f / N);
                }
                a.Add(real / N, imm / N);
            }
            return a;
        }
        static public S ReverseDFT(A a)
        {
            S s = new S();
            long N = a.Size();
            for (long t = 0; t < N; t++)
            {
                double samples = 0;
                for (long f = 0; f < N; f++)
                {
                    samples += a.Get(f).getReal() * Math.Cos(2 * Math.PI * t * f / N) - a.Get(f).getImm() * Math.Sin(2 * Math.PI * t * f / N);
                }
                s.Add((long)samples);
            }
            return s;
        }
    }
}
