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
        protected long fl1, fl2, fr1, fr2;
        public abstract void CreateFilter();
        //Filtering using convolution to the time domain
        //Convolution algorithm to create new samples
        public S Convolution(S windowed)
        {
            S temp = new S(windowed);
            temp.Convolution(WEIGHTS.Size());

            for (long i = 0; i < windowed.Size(); i++)
            {
                double sum = 0;
                for (long j = 0; j < WEIGHTS.Size(); j++)
                {
                    sum += WEIGHTS.Get(j) * temp.Get(i + j);
                }
                temp.Set(i, sum);
            }

            return temp;
        }
        // visual filter
        protected Thumb left1, left2, right1, right2;
        protected Rectangle rect1, rect2;
        protected Canvas canvas;
        public abstract void DrawFilter(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas);
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
        static public Filter FILTER = new FilterLowPass();
        static public S WEIGHTS;
        static public void ChangeFilter(int newFilter)
        {
            switch (newFilter)
            {
                case 2:
                    FILTER = new FilterBandPass();
                    break;
                case 1:
                    FILTER = new FilterHighPass();
                    break;
                case 0:
                default:
                    FILTER = new FilterLowPass();
                    break;
            }
        }
    }
}
