using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    //Helper class
    //Creation of filters
    public abstract class Filter
    {
        // calculations for filter
        protected long fl1, fl2, fr1, fr2, N;
        public abstract void CreateFilter();
        //Filtering using convolution to the time domain
        //Convolution algorithm to create new samples
        public S Convolution(S OGs)
        {
            S NEWs = new S(OGs);
            NEWs.Convolution(WEIGHTS.Size());

            for (long i = 0; i < OGs.Size(); i++)
            {
                double sum = 0;
                for (long j = 0; j < WEIGHTS.Size(); j++)
                {
                    sum += WEIGHTS.Get(j) * NEWs.Get(i + j);
                }
                NEWs.Set(i, sum);
            }

            return NEWs;
        }
        // visual filter
        protected Thumb left1, left2, right1, right2;
        protected Rectangle rect1, rect2;
        protected Canvas canvas;
        public abstract void DrawFilter(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, long N);
        protected abstract void DrawRect();
        public abstract void DragFilterLeft1(DragDeltaEventArgs e);
        public abstract void DragFilterLeft2(DragDeltaEventArgs e);
        public abstract void DragFilterRight1(DragDeltaEventArgs e);
        public abstract void DragFilterRight2(DragDeltaEventArgs e);
        public abstract void DropFilterLeft1();
        public abstract void DropFilterLeft2();
        public abstract void DropFilterRight1();
        public abstract void DropFilterRight2();

        //statics
        static public S WEIGHTS;
    }
}
