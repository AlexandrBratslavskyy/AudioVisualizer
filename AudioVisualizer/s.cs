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
            if (val > max)
                max = val;
            else if (val < min)
                min = val;
        }
        public void Convolution(long c)
        {
            for (int i = 0; i < c-1; i++)
                Add(0);
        }
        public void DeConvolution(long c)
        {
            for (int i = 0; i < c - 1; i++)
                data.RemoveAt(data.Count-1);
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
        public double GetMin()
        {
            return min;
        }
        public long Size()
        {
            return data.Count;
        }
        public byte[] GetByte()
        {
            byte[] b = new byte[data.Count];
            for (int i = 0; i < data.Count; ++i)
                b[i] = (byte)data[i];
            return b;
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
        private double min = 0;
    }
}
