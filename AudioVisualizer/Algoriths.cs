using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    class Algoriths
    {
        //statics
        static public A DFT(S s)
        {
            A a = new A();
            long N = s.Size();
            for (long f = 0; f < N; f++)
            {
                double real = 0, imm = 0;
                for (long t = 0; t < N; t++)
                {
                    real += s.Get(t) / N * Math.Cos(2 * Math.PI * t * f / N);
                    imm -= s.Get(t) / N * Math.Sin(2 * Math.PI * t * f / N);
                }
                a.Add(real, imm);
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
                    samples += a.Get(f).getReal() * Math.Cos(2 * Math.PI * t * f / N) - a.Get(f).getImm() * Math.Cos(2 * Math.PI * t * f / N);
                }
                s.Add((long)samples);
            }
            return s;
        }



        //Filtering using convolution to the time domain
        //Convolution algorithm to create new samples
        static public S Convolution(A filter, S OGs)
        {
            S weights = ReverseDFT(filter), NEWs = new S(OGs);
            NEWs.Convolution(weights.Size());
            
            for (int i = 0; i < OGs.Size(); i++)
            {
                double sum = 0;
                for (int j = 0; j < weights.Size(); j++)
                {
                    sum += weights.Get(j) * OGs.Get(i + j);
                }
                NEWs.Set(i, (int)sum);
            }

            return NEWs;
        }
    }
}
