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
        public S(List<double> d)
        {
            for (long i = 0; i < d.Count; i++)
                data.Add(d[(int)i]);
        }
        public S(Wave wav)
        {
            for (long i = 0; i < wav.data.Length; i++)
                data.Add(wav.data[i]);
        }
        // Copy constructor.
        public S(S s)
        {
            for (long i = 0; i < s.Size(); i++)
                data.Add(s.Get(i));
        }
        public void Add(double val)
        {
            data.Add(val);
            if (val > max || -1 * val > max)
                max = val;
        }
        public void Convolution(long c)
        {
            for (int i = 0; i < c-1; i++)
                Add(0);
        }
        public void Set(long t, double val)
        {
            data[(int)t] = val;
        }
        public double Get(long t)
        {
            return data[(int)t];
        }
        public double GetMax()
        {
            return max;
        }
        public long Size()
        {
            return data.Count;
        }
        public void Cut(int start, int end)
        {
            data.RemoveRange(start, end - start);
        }
        public S Copy(int start, int end)
        {
            return new S(data.GetRange(start, end - start));
        }
        public void Paste(int paste, S s)
        {
            for(int i = 0; i < s.Size(); i++)
                data.Insert(paste + i, s.Get(i));
        }
        private List<double> data = new List<double>();
        private double max = 0;
    }
}
