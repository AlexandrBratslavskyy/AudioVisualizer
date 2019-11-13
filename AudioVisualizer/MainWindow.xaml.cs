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
        static long N = 12;
        A COMPLEX;

        S ORIGINAL;
        S WINDOWED;
        S FILTERED;

        Wave WAVE;

        Windowing WINDOW = new RectangleWindow();
        Filter FILTER = new FilterLowPass(N);
        //Start
        public MainWindow()
        {
            InitializeComponent();
        }
        //New signal added
        void CreateSignal()
        {
            //save original
            ORIGINAL = new S(WAVE);
            //windowing
            CreateWindow();
        }
        void CreateSignal(S s)
        {
            //save original
            ORIGINAL = s;
            //windowing
            CreateWindow();
        }
        void CreateWindow()
        {
            //windowing
            WINDOWED = WINDOW.CreateWindow(ORIGINAL, N);
            //freq domain
            CreateComplex();
        }
        void CreateComplex()
        {
            //dft
            COMPLEX = Algorithms.DFT(WINDOWED, N);
            //display freq domain
            DisplayFrequencyDomain();
            //draw filter
            CreateFilter();
        }
        //
        void CreateFilter()
        {
            //draw filter
            FILTER.DrawFilter(left1, left2, right1, right2, rect1, rect2, FilterCanvas);
            //convolution
            CreateFilterRange();
        }
        void CreateFilterRange()
        {
            //convolution
            FILTERED = WINDOWED; //FILTER.Convolution(WINDOWED)   //showing wrong output
            //display time domain
            DisplayTimeDomain();
        }
        void DisplayTimeDomain()
        {
            TimeDomain.Width = Display.DrawTimeDomain(TimeDomain, WINDOWED);
        }
        void DisplayFrequencyDomain()
        {
            Display.DrawFrequencyDomain(FrequencyDomain, COMPLEX, N);
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
            {
                WAVE = new Wave();
                WAVE.Read(File.Open(openFileDialog.FileName, FileMode.Open));
                CreateSignal();
            }
            //MessageBox.Show("Can't open wav files", "TODO");
        }
        void test(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Simple Cosine wave: Yes\nComplex Cosine wave: No", "Test", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    CreateSignal(Tests.SimpleCosineWave());
                    break;
                case MessageBoxResult.No:
                    CreateSignal(Tests.ComplexCosineWave());
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
        void save(object sender, RoutedEventArgs e)
        {
            if (WAVE == null)
                return;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "(*.wav)|*.wav";
            if (saveFileDialog.ShowDialog() == true)
                WAVE.Write(File.Create(saveFileDialog.FileName));
            //MessageBox.Show("Can't save wav files", "TODO");
        }

        //windowing
        void rectangle(object sender, RoutedEventArgs e)
        {
            WINDOW = Windowing.ChangeFilter(0);
            if (ORIGINAL == null)
                return;
            CreateWindow();
        }
        void triangle(object sender, RoutedEventArgs e)
        {
            WINDOW = Windowing.ChangeFilter(1);
            if (ORIGINAL == null)
                return;
            CreateWindow();
        }
        void welch(object sender, RoutedEventArgs e)
        {
            WINDOW = Windowing.ChangeFilter(2);
            if (ORIGINAL == null)
                return;
            CreateWindow();
        }
        void hanning(object sender, RoutedEventArgs e)
        {
            WINDOW = Windowing.ChangeFilter(3);
            if (ORIGINAL == null)
                return;
            CreateWindow();
        }

        //filtering
        void low(object sender, RoutedEventArgs e)
        {
            FILTER = Filter.ChangeFilter(0, N);
            if (ORIGINAL == null)
                return;
            CreateFilter();
        }
        void high(object sender, RoutedEventArgs e)
        {
            FILTER = Filter.ChangeFilter(1, N);
            if (ORIGINAL == null)
                return;
            CreateFilter();
        }
        void band(object sender, RoutedEventArgs e)
        {
            FILTER = Filter.ChangeFilter(2, N);
            if (ORIGINAL == null)
                return;
            CreateFilter();
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
            if(ORIGINAL == null)
                return;
            Display.ZoomOut();
            DisplayTimeDomain();
        }
        void zoomin(object sender, RoutedEventArgs e)
        {
            if (ORIGINAL == null)
                return;
            Display.ZoomIn();
            DisplayTimeDomain();
        }
        
        //draging and droping
        //drag
        public void dragfilterleft1(object sender, DragDeltaEventArgs e)
        {
            FILTER.DragFilterLeft1(e);
        }
        public void dragfilterleft2(object sender, DragDeltaEventArgs e)
        {
            FILTER.DragFilterLeft2(e);
        }
        public void dragfilterright1(object sender, DragDeltaEventArgs e)
        {
            FILTER.DragFilterRight1(e);
        }
        public void dragfilterright2(object sender, DragDeltaEventArgs e)
        {
            FILTER.DragFilterRight2(e);
        }
        //drop
        public void dropfilterleft1(object sender, DragCompletedEventArgs e)
        {
            FILTER.DropFilterLeft1();
            CreateFilterRange();
        }
        public void dropfilterleft2(object sender, DragCompletedEventArgs e)
        {
            FILTER.DropFilterLeft2();
            CreateFilterRange();
        }
        public void dropfilterright1(object sender, DragCompletedEventArgs e)
        {
            FILTER.DropFilterRight1();
            CreateFilterRange();
        }
        public void dropfilterright2(object sender, DragCompletedEventArgs e)
        {
            FILTER.DropFilterRight2();
            CreateFilterRange();
        }
    }
}
