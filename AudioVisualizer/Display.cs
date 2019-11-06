﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    public class Display
    {

        // Statics

        static private long ZOOM = 10;
        static public void ZoomIn()
        {
            if (ZOOM != 1)
                ZOOM--;
        }
        static public void ZoomOut()
        {
            ZOOM++;
        }
        static public long DrawTimeDomain(Canvas canvas)
        {
            //clear previous
            canvas.Children.Clear();

            //line showing signal zero
            double Y = canvas.ActualHeight / 2;
            Line zeroline = new Line();

            zeroline.X1 = 0;
            zeroline.Y1 = Y;
            zeroline.X2 = S.COMPOSITE.Size() - ZOOM;
            zeroline.Y2 = Y;

            zeroline.Stroke = new SolidColorBrush(Colors.Black);
            zeroline.StrokeThickness = 1.0;

            canvas.Children.Add(zeroline);

            //compensate for limited space
            double c = 1.0;
            if (Y < S.COMPOSITE.GetMax())
                c = S.COMPOSITE.GetMax() / Y;

            //display on canvas
            long i = 0;
            for (long next = 0; next < S.COMPOSITE.Size() - ZOOM; i++)
            {
                Line line = new Line();
                int c1 = S.COMPOSITE.Get(next), c2 = S.COMPOSITE.Get(next += ZOOM);

                line.X1 = i;
                line.X2 = i;
                line.Y1 = c * (Y - c1);
                line.Y2 = c * (Y - c2);

                line.Stroke = new SolidColorBrush(Colors.Blue);
                line.StrokeThickness = 1.0;

                canvas.Children.Add(line);
            }
            return i;
        }
        static public void DrawFrequencyDomain(Canvas canvas)
        {
            //clear previous
            canvas.Children.Clear();

            //line showing signal zero
            double Y = canvas.ActualHeight, X = canvas.ActualWidth / Filter.getSize();

            //display filters
            long i = 0, j = 1;
            for (double l = 0; l < canvas.ActualWidth; l+=X)
            {
                //display lines on canvas
                Line line = new Line(), value = new Line();

                line.X1 = l;
                line.X2 = l;
                line.Y1 = 0;
                line.Y2 = Y;

                line.Stroke = new SolidColorBrush(Colors.Black);
                line.StrokeThickness = 1.0;

                //display values
                double v = 0;
                for (; i < j * A.COMPLEX.Size() / Filter.getSize() && i < A.COMPLEX.Size(); i++)
                {
                    v += Math.Abs(A.COMPLEX.Get(i).getReal());
                }
                j++;

                value.X1 = l + X / 2;
                value.X2 = l + X / 2;
                value.Y1 = Y - v;
                value.Y2 = Y;

                value.Stroke = new SolidColorBrush(Colors.Red);
                value.StrokeThickness = X;

                // add both - order matters I think
                canvas.Children.Add(value);
                canvas.Children.Add(line);
            }
        }
    }
}
