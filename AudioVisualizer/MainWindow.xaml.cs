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

        S s;
        void DisplayTimeDomain()
        {
            TimeDomain.Width = Display.DrawTimeDomain(TimeDomain, s);
        }
        //display wave
        void record(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can't connect to dll", "TODO");
        }
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
                    s = Tests.SimpleCosineWave();
                    DisplayTimeDomain();
                    break;
                case MessageBoxResult.No:
                    s = Tests.ComplexCosineWave();
                    DisplayTimeDomain();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
        void save(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Can't save wav files", "TODO");
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
            if(s == null)
            { 
                MessageBox.Show("Select Waveform First", "Zoom Out");
                return;
            }
            Display.ZoomOut();
            DisplayTimeDomain();
        }
        void zoomin(object sender, RoutedEventArgs e)
        {
            if (s == null)
            {
                MessageBox.Show("Select Waveform First", "Zoom In");
                return;
            }
            Display.ZoomIn();
            DisplayTimeDomain();
        }
    }
}
