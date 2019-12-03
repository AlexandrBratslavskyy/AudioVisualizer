using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace AudioVisualizer
{
    public partial class MainWindow : Window
    {
        static long N = 100;
        A COMPLEX;

        S ORIGINAL, FILTERED, WINDOWED;

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
        void CreateSignal(bool b)
        {
            if(b)
                //save original
                ORIGINAL = new S(WAVE);
            //enable all
            btnsave.IsEnabled = true;
            btnplay.IsEnabled = true;
            btncut.IsEnabled = true;
            btncopy.IsEnabled = true;

            CreateDivergentPaths();
        }
        void CreateDivergentPaths()
        {
            //freq domain
            Task c = Task.Run(CreateComplex);
            //draw filter
            Task f = Task.Run(CreateFilter);
            Task.WaitAll(c, f);
        }
        void CreateComplex()
        {
            //dft
            COMPLEX = Algorithms.DFT(ORIGINAL, N);
            //display freq domain
            DisplayFrequencyDomain();
        }
        //
        void CreateFilter()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                //draw filter
                FILTER.DrawFilter(ref left1, ref left2, ref right1, ref right2, ref rect1, ref rect2, ref FilterCanvas, ref N);
                //convolution
                CreateFilterRange();
            });
        }
        void CreateFilterRange()
        {
            //convolution
            FILTERED = FILTER.Convolution(ref ORIGINAL);
            //windowing
            CreateWindow();
        }
        void CreateWindow()
        {
            //windowing
            WINDOWED = WINDOW.CreateWindow(ref FILTERED, ref N);
            //display time domain
            DisplayTimeDomain();
        }
        void DisplayTimeDomain()
        {
            Display.DrawTimeDomain(ref TimeDomain, ref WINDOWED);
        }
        void DisplayFrequencyDomain()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () {
                Display.DrawFrequencyDomain(ref FrequencyDomain, ref COMPLEX, ref N);
            });
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
                CreateSignal(true);
            }
            //MessageBox.Show("Can't open wav files", "TODO");
        }
        void save(object sender, RoutedEventArgs e)
        {
            if (WAVE == null)
                return;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "(*.wav)|*.wav";
            if (saveFileDialog.ShowDialog() == true)
            {
                Wave w = new Wave(WAVE, WINDOWED);
                w.Write(File.Create(saveFileDialog.FileName));
            }
        }

        //record
        void startrecord(object sender, RoutedEventArgs e)
        {
            Win32.StartRecord(ref quantizationLevel, ref sampleRate);
            btnstartrecord.IsEnabled = false;
            btnstoprecord.IsEnabled = true;
            btnplay.IsEnabled = false;
        }
        void stoprecord(object sender, RoutedEventArgs e)
        {
            btnstartrecord.IsEnabled = true;
            btnstoprecord.IsEnabled = false;
            WAVE = Win32.StopRecord(ref quantizationLevel, ref sampleRate);
            CreateSignal(true);
        }
        void play(object sender, RoutedEventArgs e)
        {
            Win32.StartPlay(ref WAVE, ref WINDOWED);
            btnplay.IsEnabled = false;
            btnstopplay.IsEnabled = true;
        }
        void stopplay(object sender, RoutedEventArgs e)
        {
            Win32.StopPlay();
            btnplay.IsEnabled = true;
            btnstopplay.IsEnabled = false;
        }

        //sample rate
        int sampleRate = 11025;
        void SR11025(object sender, RoutedEventArgs e)
        {
            sampleRate = 11025;
        }
        void SR22050(object sender, RoutedEventArgs e)
        {
            sampleRate = 22050;
        }
        void SR44100(object sender, RoutedEventArgs e)
        {
            sampleRate = 44100;
        }

        //quan lvl
        int quantizationLevel = 16;
        void QL16(object sender, RoutedEventArgs e)
        {
            quantizationLevel = 16;
        }
        void QL32(object sender, RoutedEventArgs e)
        {
            quantizationLevel = 32;
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
            Task.Run(CreateFilter);
        }
        void high(object sender, RoutedEventArgs e)
        {
            FILTER = new FilterHighPass();
            if (ORIGINAL == null)
                return;
            Task.Run(CreateFilter);
        }
        void band(object sender, RoutedEventArgs e)
        {
            FILTER = new FilterBandPass();
            if (ORIGINAL == null)
                return;
            Task.Run(CreateFilter);
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
                EDITOR.DrawEditor(ref left, ref right, ref rect, ref EditCanvas, ref Edit);

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
                EDITOR.DrawEditor(ref left, ref right, ref rect, ref EditCanvas, ref Edit);

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
                EDITOR.DrawEditor(ref left, ref right, ref rect, ref EditCanvas, ref Edit);

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

        //N
        void ndown(object sender, RoutedEventArgs e)
        {
            if (ORIGINAL == null)
                return;
            if (N > 2)
            {
                N -= 2;
                CreateDivergentPaths();
            }
        }
        void nup(object sender, RoutedEventArgs e)
        {
            if (ORIGINAL == null)
                return;
            if (N + 2 <= ORIGINAL.Size())
            {
                N += 2;
                CreateDivergentPaths();
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
            FILTER.DragFilterLeft1(ref e);
        }
        public void dragfilterleft2(object sender, DragDeltaEventArgs e)
        {
            FILTER.DragFilterLeft2(ref e);
        }
        public void dragfilterright1(object sender, DragDeltaEventArgs e)
        {
            FILTER.DragFilterRight1(ref e);
        }
        public void dragfilterright2(object sender, DragDeltaEventArgs e)
        {
            FILTER.DragFilterRight2(ref e);
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
        //more drag
        public void drageditorleft(object sender, DragDeltaEventArgs e)
        {
            EDITOR.DragEditorLeft(ref e);
        }
        public void drageditorright(object sender, DragDeltaEventArgs e)
        {
            EDITOR.DragEditorRight(ref e);
        }
        void edit(object sender, RoutedEventArgs e)
        {
            EDITOR.Edit(ref ORIGINAL, ref scroll);
            btnpaste.IsEnabled = Editor.isCP();
            CreateSignal(false);
        }
    }
}
