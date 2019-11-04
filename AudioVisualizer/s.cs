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
        public void Add(long val)
        {
            data.Add((int)val);
            if (val > max || -1 * val > max)
                max = val;
        }
        public void Convolution(int c)
        {
            for (int i = 0; i < c-1; i++)
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
        public long GetMax()
        {
            return max;
        }
        public int Size()
        {
            return data.Count;
        }
        public void Cut(int start, int end)
        {
            data.RemoveRange(start, end - start);
        }
        public void Copy(int start, int end)
        {
            data.GetRange(start, end - start);
        }
        public void Paste(int paste, S s)
        {
            for(int i = 0; i < s.Size(); i++)
                data.Insert(paste + i, s.Get(i));
        }
        private List<int> data = new List<int>();
        private long max = 0;
    }
}
