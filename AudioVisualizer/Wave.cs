using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AudioVisualizer
{
    //Class that stores the wave file information
    public class Wave
    {
        //Default constructor
        public Wave() { }
        //Play constroctor
        public Wave(Wave w, S s)
        {
            byte[] signalData = s.GetByte();

            chunkID = w.chunkID;
            fileSize = 36 + (uint)signalData.Length;
            riffType = w.riffType;
            fmtID = w.fmtID;
            fmtSize = w.fmtSize;
            fmtCode = w.fmtCode;
            channels = w.channels;
            sampleRate = w.sampleRate;
            fmtAvgBPS = w.fmtAvgBPS;
            fmtBlockAlign = w.fmtBlockAlign;
            bitsPerSample = w.bitsPerSample;
            dataID = w.dataID;
            dataSize = (uint)signalData.Length;

            data = signalData;
        }
        //Constructor when recording
        public Wave(byte[] signalData, ushort bitsPerSample, ushort sampleRate)
        {
            chunkID = System.Text.Encoding.ASCII.GetBytes("RIFF");
            fileSize = 36 + (uint)signalData.Length;
            riffType = System.Text.Encoding.ASCII.GetBytes("WAVE");
            fmtID = System.Text.Encoding.ASCII.GetBytes("fmt ");
            fmtSize = 16;
            fmtCode = 1;
            channels = 1;
            this.sampleRate = sampleRate;
            fmtAvgBPS = 22050;
            fmtBlockAlign = 2;
            this.bitsPerSample = bitsPerSample;
            dataID = System.Text.Encoding.ASCII.GetBytes("data");
            dataSize = (uint)signalData.Length;

            data = signalData;
        }

        public byte[] chunkID;
        public uint fileSize;
        public byte[] riffType;
        public byte[] fmtID;
        public uint fmtSize;
        public ushort fmtCode;
        public ushort channels;
        public uint sampleRate;
        public uint fmtAvgBPS;
        public ushort fmtBlockAlign;
        public ushort bitsPerSample;
        public byte[] dataID;
        public uint dataSize;

        public byte[] data;

        // statics

        //Read in a wave file from a stream and store the header information
        //into a Wave object and the data in a byte array
        public Byte[] Read(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            chunkID = br.ReadBytes(4);
            fileSize = br.ReadUInt32();
            riffType = br.ReadBytes(4);
            fmtID = br.ReadBytes(4);
            fmtSize = br.ReadUInt32();
            fmtCode = br.ReadUInt16();
            channels = br.ReadUInt16();
            sampleRate = br.ReadUInt32();
            fmtAvgBPS = br.ReadUInt32();
            fmtBlockAlign = br.ReadUInt16();
            bitsPerSample = br.ReadUInt16();
            dataID = br.ReadBytes(4);
            dataSize = br.ReadUInt32();

            if (fmtSize != 16)
            {
                //this = null;
                return null;
            }

            data = br.ReadBytes((int)dataSize);

            stream.Close();
            br.Close();
            return data;
        }

        //Write a WAVE file to the specified stream using
        //the information provided by the Wave object
        public void Write(Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(chunkID);
            bw.Write(fileSize);
            bw.Write(riffType);
            bw.Write(fmtID);
            bw.Write(fmtSize);
            bw.Write(fmtCode);
            bw.Write(channels);
            bw.Write(sampleRate);
            bw.Write(fmtAvgBPS);
            bw.Write(fmtBlockAlign);
            bw.Write(bitsPerSample);
            bw.Write(dataID);
            bw.Write(dataSize);
            bw.Write(data);

            bw.Close();
            stream.Close();
        }
    }
}
