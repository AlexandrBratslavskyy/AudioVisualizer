using System;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    class Algorithms
    {
        //statics
        static public A DFT(S s, long N)
        {
            Task[] Tasks = new Task[N];

            A a = new A(N);
            for (long f = 0; f < N; ++f)
            {
                long i = f;
                Tasks[f] = Task.Run(() =>
                {
                    double real = 0, imm = 0;
                    for (long t = 0; t < N; ++t)
                    {
                        real += s.Get(t+1000) * Math.Cos(2 * Math.PI * t * i / N);
                        imm -= s.Get(t + 1000) * Math.Sin(2 * Math.PI * t * i / N);
                    }
                    a.Set(i, real / N, imm / N);
                });
            }
            Task.WaitAll(Tasks);
            return a;
        }

        static public S ReverseDFT(A a)
        {
            long N = a.Size();

            Task[] Tasks = new Task[N];

            S s = new S(N);
            for (long t = 0; t < N; t++)
            {
                long i = t;
                Tasks[t] = Task.Run(() =>
                {
                    double samples = 0;
                    for (long f = 0; f < N; f++)
                    {
                        samples += a.GetReal(f) * Math.Cos(2 * Math.PI * i * f / N) - a.GetImm(f) * Math.Sin(2 * Math.PI * i * f / N);
                    }
                    s.Set(i, samples / N);
                });
            }
            Task.WaitAll(Tasks);
            return s;
        }
    }
}
