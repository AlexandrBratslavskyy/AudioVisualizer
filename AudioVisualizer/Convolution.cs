using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    class Convolve
    {
        //Filtering using convolution to the time domain
        //Convolution algorithm to create new samples
        static public S Convolution(A filter, S OGs)
        {
            S weights = S.ReverseDFT(filter), NEWs = new S(OGs);
            NEWs.Convolution(weights.Size()-1);
            
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
