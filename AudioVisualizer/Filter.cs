using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    //Helper class
    //Creation of filters
    public abstract class Filter
    {
        public abstract A CreateFilter(long frequencyBin1, long frequencyBin2, long N);

        //statics
        static public Filter FILTER = new LowPassFilter();
        static public void ChangeFilter(int newFilter)
        {
            switch (newFilter)
            {
                case 2:
                    FILTER = new BandPassFilter();
                    break;
                case 1:
                    FILTER = new HighPassFilter();
                    break;
                case 0:
                default:
                    FILTER = new LowPassFilter();
                    break;
            }
        }
    }
    public class LowPassFilter : Filter
    {
        /*
            create a low pass filter based on user selection
            user selects a frequency cutoff
            all frequencies after freq cutoff are removed

            freq = (f(frequency bins) * SamplingRate) / NumSamples

                                                      Nyquist
            [1, 1, 1, 1, 1, 1,            0, 0, 0, 0,    0,    0, 0, 0, 0, 1, 1, 1, 1, 1]
                            freq(cutoff) ---------------> <---------------
        */
        public override A CreateFilter(long frequencyBin1, long frequencyBin2, long N)
        {
            long nyquistLimit = N / 2;
            //error checking
            if (frequencyBin1 > nyquistLimit)
            {
                frequencyBin1 = nyquistLimit - (frequencyBin1 - nyquistLimit);
            }

            long difference = ((nyquistLimit - frequencyBin1) * 2) + 1;
            A filter = new A();
            
            long i;

            //beginning of filter
            for (i = 0; i <= frequencyBin1; i++)
            {
                filter.Add(1, 1);
            }

            //middle of filter
            for (; i <= frequencyBin1 + difference; i++)
            {
                filter.Add(0, 0);
            }

            //end of filter
            for (; i < N; i++)
            {
                filter.Add(1, 1);
            }

            //return filter;
            return filter;
        }
    }
    public class HighPassFilter : Filter
    {
        /*
            create a high pass filter based on user selection
            user selects a frequency cutoff
            all frequencies after freq cutoff are removed

            freq = (f(frequency bins) * SamplingRate) / NumSamples

                                                      Nyquist
            [0, 0, 0, 0, 0, 0,            1, 1, 1, 1,    1,    1, 1, 1, 1, 0, 0, 0, 0, 0]
                            freq(cutoff) ---------------> <---------------
        */
        public override A CreateFilter(long frequencyBin1, long frequencyBin2, long N)
        {
            long nyquistLimit = N / 2;
            //error checking
            if (frequencyBin1 > nyquistLimit)
            {
                frequencyBin1 = nyquistLimit - (frequencyBin1 - nyquistLimit);
            }

            long difference = ((nyquistLimit - frequencyBin1) * 2) + 1;
            A filter = new A();

            long i;

            //beginning of filter
            for (i = 0; i <= frequencyBin1; i++)
            {
                filter.Add(0, 0);
            }

            //middle of filter
            for (; i <= frequencyBin1 + difference; i++)
            {
                filter.Add(1, 1);
            }

            //end of filter
            for (; i < N; i++)
            {
                filter.Add(0, 0);
            }

            //return filter;
            return filter;
        }
    }
    public class BandPassFilter : Filter
    {
        public override A CreateFilter(long frequencyBin1, long frequencyBin2, long N)
        {
            long nyquistLimit = N / 2;
            //error checking
            if (frequencyBin1 > nyquistLimit && frequencyBin2 > nyquistLimit && frequencyBin1 > frequencyBin2)
            {
                frequencyBin1 = nyquistLimit - (frequencyBin1 - nyquistLimit);
            }
            if (frequencyBin2 > nyquistLimit)
            {
                frequencyBin2 = nyquistLimit - (frequencyBin2 - nyquistLimit);
            }
            if (frequencyBin1 > frequencyBin2)
            {
                long temp = frequencyBin1;
                frequencyBin1 = frequencyBin2;
                frequencyBin2 = temp;
            }

            long difference1 = ((nyquistLimit - frequencyBin2) * 2) + 1, difference2 = ((nyquistLimit - frequencyBin1) * 2) + 1;
            A filter = new A();

            long i;

            //beginning of filter
            for (i = 0; i <= frequencyBin1; i++)
            {
                filter.Add(0, 0);
            }

            //1st band
            for (i = 0; i <= frequencyBin2; i++)
            {
                filter.Add(1, 1);
            }

            //middle of filter
            for (; i <= frequencyBin2 + difference1; i++)
            {
                filter.Add(0, 0);
            }

            //2nd band
            for (; i <= frequencyBin1 + difference2; i++)
            {
                filter.Add(1, 1);
            }

            //end of filter
            for (; i < N; i++)
            {
                filter.Add(0, 0);
            }

            //return filter;
            return filter;
        }
    }
}
