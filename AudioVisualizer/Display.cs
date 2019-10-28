using System;
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
        public void Add(int time, int signal)
        {
            data.Add(new Cartesian(time, signal));
        }
        public Cartesian Get(int i)
        {
            return data[i];
        }
        public int Size()
        {
            return data.Count;
        }
        private List<Cartesian> data = new List<Cartesian>();

        // Statics
        static public void DrawChart(Canvas canvas ,Display display)
        {
            canvas.Children.Clear();
            double Y = canvas.Height / 2;
            Line zeroline = new Line();

            zeroline.X1 = 0;
            zeroline.Y1 = Y;
            zeroline.X2 = canvas.Width;
            zeroline.Y2 = Y;

            zeroline.Stroke = new SolidColorBrush(Colors.Black);
            zeroline.StrokeThickness = 1.0;

            canvas.Children.Add(zeroline);

            for (int i = 0; i < display.Size() - 1; i++)
            {
                Line line = new Line();
                Cartesian c1 = display.Get(i), c2 = display.Get(i + 1);

                line.X1 = c1.Time;
                line.Y1 = Y - c1.Signal;
                line.X2 = c2.Time;
                line.Y2 = Y - c2.Signal;

                line.Stroke = new SolidColorBrush(Colors.Black);
                line.StrokeThickness = 1.0;

                canvas.Children.Add(line);
            }
        }
    }
    public struct Cartesian
    {
        public int Time;
        public int Signal;

        public Cartesian(int time, int signal)
        {
            Time = time;
            Signal = signal;
        }
    }
    
}
