using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Controls;

namespace AudioVisualizer
{
    class Win32
    {
        static private int bitsPerSample = 16;
        static private int sampleRate = 11025;

        static public void StartRecord()
        {
            StartRec(bitsPerSample, sampleRate);
        }
        static public Wave StopRecord()
        {
            RecordData rd = StopRec();
            byte[] data = new byte[rd.len];
            Marshal.Copy(rd.ip, data, 0, (int)rd.len);

            return new Wave(data);
        }

        static public void StartPlay(Wave wave, S s)
        {
            Wave w = new Wave(wave, s);

            IntPtr intptr = Marshal.AllocHGlobal(w.data.Length);
            Marshal.Copy(w.data, 0, intptr, w.data.Length);

            PlayStart(intptr, (int)w.data.Length, (int)w.bitsPerSample, (int)w.sampleRate);
        }

        static public void StopPlay()
        {
            PlayStop();
        }

        [DllImport("Record1.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OpenDialog();
        [DllImport("Record1.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CloseDialog();
        [DllImport("Record1.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern RecordData StopRec();
        [DllImport("Record1.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool StartRec(int d, int r);
        [DllImport("Record1.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetWaveform();
        [DllImport("Record1.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool PlayPause();
        [DllImport("Record1.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool PlayStart(IntPtr p, int size, int d, int r);
        [DllImport("Record1.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool PlayStop();

        // struct holding a pointer to the recorded data from the win32 DLL
        // along with its length (pointers don't come with lengths)
        public struct RecordData
        {
            public uint len;
            public IntPtr ip;
        }

        
    }
}
