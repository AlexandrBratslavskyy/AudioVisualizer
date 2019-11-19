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
using System.ComponentModel;

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
        Filter FILTER = new FilterLowPass();
        Editor EDITOR;
        //Start
        public MainWindow()
        {
            InitializeComponent();
            Win32.OpenDialog();
        }
        public void MainWindowClose(object sender, CancelEventArgs e)
        {
            //Win32.CloseDialog();
        }

        //New signal added
        void CreateSignal()
        {
            //save original
            ORIGINAL = new S(WAVE);
            //enable all
            btnsave.IsEnabled = true;
            btnplay.IsEnabled = true;
            btncut.IsEnabled = true;
            btncopy.IsEnabled = true;
            //freq domain
            CreateComplex();
        }
        void CreateSignal(S s)
        {
            //save original
            ORIGINAL = s;
            //enable all
            btnsave.IsEnabled = true;
            btnplay.IsEnabled = true;
            btncut.IsEnabled = true;
            btncopy.IsEnabled = true;
            //freq domain
            CreateComplex();
        }
        void CreateComplex()
        {
            //dft
            COMPLEX = Algorithms.DFT(ORIGINAL, N);
            //display freq domain
            DisplayFrequencyDomain();
            //draw filter
            CreateFilter();
        }
        //
        void CreateFilter()
        {
            //draw filter
            FILTER.DrawFilter(left1, left2, right1, right2, rect1, rect2, FilterCanvas, N);
            //convolution
            CreateFilterRange();
        }
        void CreateFilterRange()
        {
            //convolution
            FILTERED = FILTER.Convolution(ORIGINAL);   //showing wrong output
            //windowing
            CreateWindow();
        }
        void CreateWindow()
        {
            //windowing
            WINDOWED = WINDOW.CreateWindow(ORIGINAL, N); //WINDOW.CreateWindow(FILTERED, N);
            //display time domain
            DisplayTimeDomain();
        }
        void DisplayTimeDomain()
        {
            TimeDomain.Width = Display.DrawTimeDomain(TimeDomain, ORIGINAL);
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
            WINDOW = new RectangleWindow();
            if (ORIGINAL == null)
                return;
            CreateWindow();
        }
        void triangle(object sender, RoutedEventArgs e)
        {
            WINDOW = new TriangleWindow();
            if (ORIGINAL == null)
                return;
            CreateWindow();
        }
        void welch(object sender, RoutedEventArgs e)
        {
            WINDOW = new WelchWindow();
            if (ORIGINAL == null)
                return;
            CreateWindow();
        }
        void hanning(object sender, RoutedEventArgs e)
        {
            WINDOW = new HanningWindow();
            if (ORIGINAL == null)
                return;
            CreateWindow();
        }

        //filtering
        void low(object sender, RoutedEventArgs e)
        {
            FILTER = new FilterLowPass();
            if (ORIGINAL == null)
                return;
            CreateFilter();
        }
        void high(object sender, RoutedEventArgs e)
        {
            FILTER = new FilterHighPass();
            if (ORIGINAL == null)
                return;
            CreateFilter();
        }
        void band(object sender, RoutedEventArgs e)
        {
            FILTER = new FilterBandPass();
            if (ORIGINAL == null)
                return;
            CreateFilter();
        }

        //record
        void startrecord(object sender, RoutedEventArgs e)
        {
            Win32.StartRecord();
            btnstartrecord.IsEnabled = false;
            btnstoprecord.IsEnabled = true;
        }
        void stoprecord(object sender, RoutedEventArgs e)
        {
            btnstartrecord.IsEnabled = true;
            btnstoprecord.IsEnabled = false;
            WAVE = Win32.StopRecord();
            CreateSignal();
        }
        void play(object sender, RoutedEventArgs e)
        {
            Win32.StartPlay(WAVE);
            btnplay.IsEnabled = false;
            btnstopplay.IsEnabled = true;
        }
        void stopplay(object sender, RoutedEventArgs e)
        {
            Win32.StopPlay();
            btnplay.IsEnabled = true;
            btnstopplay.IsEnabled = false;
        }

        //editing
        bool ect = false;
        bool ecp = false;
        bool ept = false;
        void cut(object sender, RoutedEventArgs e)
        {
            if (!ect)
            {
                EDITOR = new EditorCut();
                EDITOR.DrawEditor(left, right, rect, EditCanvas, Edit);

                ect = true;
                ecp = false;
                ept = false;
            }
            else
            {
                EDITOR.EraseEditor();
                ect = false;
                ecp = false;
                ept = false;
            }
            
        }
        void copy(object sender, RoutedEventArgs e)
        {
            if (!ecp)
            {
                EDITOR = new EditorCopy();
                EDITOR.DrawEditor(left, right, rect, EditCanvas, Edit);

                ect = false;
                ecp = true;
                ept = false;
            }
            else
            {
                EDITOR.EraseEditor();
                ect = false;
                ecp = false;
                ept = false;
            }
        }
        void paste(object sender, RoutedEventArgs e)
        {
            if (!ept)
            {
                EDITOR = new EditorPaste();
                EDITOR.DrawEditor(left, right, rect, EditCanvas, Edit);

                ect = false;
                ecp = false;
                ept = true;
            }
            else
            {
                EDITOR.EraseEditor();
                ect = false;
                ecp = false;
                ept = false;
            }
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

        public void drageditorleft(object sender, DragDeltaEventArgs e)
        {
            EDITOR.DragEditorLeft(e);
        }
        public void drageditorright(object sender, DragDeltaEventArgs e)
        {
            EDITOR.DragEditorRight(e);
        }
        void edit(object sender, RoutedEventArgs e)
        {
            EDITOR.Edit(ORIGINAL, scroll);
            btnpaste.IsEnabled = Editor.isCP();
            CreateSignal(ORIGINAL);
        }
    }
}
