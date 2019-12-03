using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioVisualizer
{
    public class S
    {
        public S() { }
        public S(long N)
        {
            for (long i = 0; i < N; ++i)
            {
                data.Add(0);
            }
        }
        public S(List<double> d)
        {
            for (long i = 0; i < d.Count; i++)
                data.Add(d[(int)i]);
        }
        public S(Wave wav)
        {
            double[] samples;
            switch (wav.bitsPerSample)
            {
                case 16:
                    short[] stemp = new short[(int)(wav.dataSize / wav.fmtBlockAlign)];

                    for (int i = 0, j = 0; i < wav.dataSize - 4; i += (int)wav.fmtBlockAlign, j++)
                        stemp[j] = BitConverter.ToInt16(wav.data, i);

                    samples = stemp.Select(x => (double)(x)).ToArray();
                    data.AddRange(samples);
                    
                    break;
                 case 32:
                    int[] itemp = new int[(int)(wav.dataSize / wav.fmtBlockAlign)];

                    for (int i = 0, j = 0; i < wav.dataSize - 4; i += (int)wav.fmtBlockAlign, j++)
                        itemp[j] = BitConverter.ToInt32(wav.data, i);

                    samples = itemp.Select(x => (double)(x)).ToArray();
                    data.AddRange(samples);
                    break;
                 }
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
    }
}
