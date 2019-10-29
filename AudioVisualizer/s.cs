using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    public class S
    {
        public S() { }
        // Copy constructor.
        public S(S s)
        {
            for (long i = 0; i < s.Size(); i++)
                data.Add(s.Get(i));
        }
        public void Add(int val)
        {
            data.Add(val);
        }
        public void Convolution(int c)
        {
            for (int i = 0; i < c; i++)
                Add(0);
        }
        public void Set(int t, int val)
        {
            data[t] = val;
        }
        public int Get(long t)
        {
            return data[(int)t];
        }
        public int Size()
        {
            return data.Count;
        }
        private List<int> data = new List<int>();

        // statics
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
                s.Add((int)samples);
            }
            return s;
        }
    }
}
