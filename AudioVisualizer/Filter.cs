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
        public abstract A CreateFilter(long frequencyBin1, long frequencyBin2, long N);
        // visual filter
        public abstract void DrawFilter(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas);
        protected abstract void DrawRect(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2);
        public abstract void DragFilterLeft1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e);
        public abstract void DragFilterLeft2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e);
        public abstract void DragFilterRight1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e);
        public abstract void DragFilterRight2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas, DragDeltaEventArgs e);
        public abstract void DropFilterLeft1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas);
        public abstract void DropFilterLeft2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas);
        public abstract void DropFilterRight1(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas);
        public abstract void DropFilterRight2(Thumb left1, Thumb left2, Thumb right1, Thumb right2, Rectangle rect1, Rectangle rect2, Canvas canvas);
        //statics
        static public Filter FILTER = new FilterLowPass();
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
        static private long FILTERSIZE = 12;
        static public void SmallerSize()
        {
            if (FILTERSIZE != 2)
                FILTERSIZE -= 2;
        }
        static public void BiggerSize()
        {
            if(FILTERSIZE < A.COMPLEX.Size())
                FILTERSIZE += 2;
        }
        static public long getSize()
        {
            return FILTERSIZE;
        }
    }
}
