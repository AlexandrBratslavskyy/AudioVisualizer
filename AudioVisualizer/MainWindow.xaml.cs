using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AudioVisualizer
{
    public partial class MainWindow : System.Windows.Window
    {
        //Start
        public MainWindow()
        {
            InitializeComponent();
        }
        void NewSignal(S s)
        {
            //save original
            S.ORIGINAL = s;
            //dft
            A.COMPLEX = Algoriths.DFT(s, 12);
            //display freq domain
            DisplayFrequencyDomain();
            //windowing
            NewWindow();
        }
        void NewFilter()
        {
            Filter.FILTER.DrawFilter(left1, left2, right1, right2, rect1, rect2, FilterCanvas);
        }
        void NewWindow()
        {
            //revdft
            S rdft = Algoriths.ReverseDFT(A.COMPLEX);
            //windowing
            S.COMPOSITE = Windowing.WINDOW.CreateWindow(rdft, rdft.Size());
            //display time domain
            DisplayTimeDomain();
        }
        void DisplayTimeDomain()
        {
            TimeDomain.Width = Display.DrawTimeDomain(TimeDomain);
        }
        void DisplayFrequencyDomain()
        {
            Display.DrawFrequencyDomain(FrequencyDomain);
        }

        /*
         Clicking the options box
         */
        //display wave
        void browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.wav)|*.wav";
            if (openFileDialog.ShowDialog() == true)
                MessageBox.Show("Can't open wav files", "TODO");
        }
        void test(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Simple Cosine wave: Yes\nComplex Cosine wave: No", "Test", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    NewSignal(Tests.SimpleCosineWave());
                    break;
                case MessageBoxResult.No:
                    NewSignal(Tests.ComplexCosineWave());
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
        void save(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can't save wav files", "TODO");
        }

        //windowing
        void rectangle(object sender, RoutedEventArgs e)
        {
            Windowing.ChangeFilter(0);
            NewWindow();
        }
        void triangle(object sender, RoutedEventArgs e)
        {
            Windowing.ChangeFilter(1);
            NewWindow();
        }
        void welch(object sender, RoutedEventArgs e)
        {
            Windowing.ChangeFilter(2);
            NewWindow();
        }
        void hanning(object sender, RoutedEventArgs e)
        {
            Windowing.ChangeFilter(3);
            NewWindow();
        }

        //filtering
        void low(object sender, RoutedEventArgs e)
        {
            Filter.ChangeFilter(0);
            NewFilter();
        }
        void high(object sender, RoutedEventArgs e)
        {
            Filter.ChangeFilter(1);
            NewFilter();
        }
        void band(object sender, RoutedEventArgs e)
        {
            Filter.ChangeFilter(2);
            NewFilter();
        }

        //record
        void record(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can't connect to dll", "TODO");
        }
        void stoprecord(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can't connect to dll", "TODO");
        }
        void play(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can't connect to dll", "TODO");
        }
        void stopplay(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can't connect to dll", "TODO");
        }

        //editing
        void cut(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can't cut", "TODO");
        }
        void copy(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can't copy", "TODO");
        }
        void paste(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can't paste", "TODO");
        }

        //zooming
        void zoomout(object sender, RoutedEventArgs e)
        {
            if(S.ORIGINAL == null)
            { 
                MessageBox.Show("Select Waveform First", "Zoom Out");
                return;
            }
            Display.ZoomOut();
            DisplayTimeDomain();
        }
        void zoomin(object sender, RoutedEventArgs e)
        {
            if (S.ORIGINAL == null)
            {
                MessageBox.Show("Select Waveform First", "Zoom In");
                return;
            }
            Display.ZoomIn();
            DisplayTimeDomain();
        }

        /////////////////////////////////////////////////////////////////////////////////
        public void dragfilterlineleft(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            Point m = Mouse.GetPosition(FilterCanvas);
            if(m.X >= 0 && m.X <= FilterCanvas.ActualWidth/2)
            Canvas.SetLeft(thumb, Canvas.GetLeft(thumb) + e.HorizontalChange);
            //Canvas.SetTop(thumb, Canvas.GetTop(thumb) + e.VerticalChange);
        }

        public void dropfilterlineleft(object sender, DragCompletedEventArgs e)
        {
            var thumb = sender as Thumb;
            Canvas.SetLeft(thumb, 0);
        }
        /////////////////////////////////////////////////////////////////////////////////
        
        //draging and droping
        //drag
        public void dragfilterleft1(object sender, DragDeltaEventArgs e)
        {
            Filter.FILTER.DragFilterLeft1(left1, right1, rect1, rect2, FilterCanvas, e);
        }
        public void dragfilterleft2(object sender, DragDeltaEventArgs e)
        {
            Filter.FILTER.DragFilterLeft2(left2, right2, rect1, rect2, FilterCanvas, e);
        }
        public void dragfilterright1(object sender, DragDeltaEventArgs e)
        {
            Filter.FILTER.DragFilterRight1(left1, right1, rect1, rect2, FilterCanvas, e);
        }
        public void dragfilterright2(object sender, DragDeltaEventArgs e)
        {
            Filter.FILTER.DragFilterRight2(left2, right2, rect1, rect2, FilterCanvas, e);
        }
        //drop
        public void dropfilterleft1(object sender, DragCompletedEventArgs e)
        {
            //TODO
        }
        public void dropfilterleft2(object sender, DragCompletedEventArgs e)
        {
            //TODO
        }
        public void dropfilterright1(object sender, DragCompletedEventArgs e)
        {
            //TODO
        }
        public void dropfilterright2(object sender, DragCompletedEventArgs e)
        {
            //TODO
        }
    }
}
