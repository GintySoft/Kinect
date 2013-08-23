using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GintySoft.KinectLib
{
    public class KinectAudioRecorder:KinectBase
    {
        private object filelock = new object();
        public event AudioRecordingDone NewAudioRecordingDone;
        public delegate void AudioRecordingDone(string fileName);
        private FileStream AudioFile { get; set; }
        private int RecordingTime { get; set; }
        private int RecordingLength { get; set; }
        private int TotalCount { get; set; }
        private bool IsDone = false;
        private string FileName { get; set; }
        private volatile bool Start = false;

        struct WAVEFORMATEX
        {
            public ushort wFormatTag;
            public ushort nChannels;
            public uint nSamplesPerSec;
            public uint nAvgBytesPerSec;
            public ushort nBlockAlign;
            public ushort wBitsPerSample;
            public ushort cbSize;
        }

        public KinectAudioRecorder(Kinect k, string fileName, int recordingTime):base(k)
        {
            this.FileName = fileName;
            this.AudioFile = new FileStream(this.FileName, FileMode.Create);
            this.RecordingTime = recordingTime;
            this.RecordingLength = this.RecordingTime * 2 * 16000;
            this.TotalCount = 0;
            this.writeWAVHeader();
            this.Start = true;
        }
        protected override void Kinect_NewRecordedAudio(byte[] audio)
        {
            base.Kinect_NewRecordedAudio(audio);
            if (!Start)
                return;
            if (this.TotalCount < this.RecordingLength)
            {
                this.AudioFile.Write(audio, 0, audio.Length);
                this.TotalCount += audio.Length;
            }
            else
            {
                if (!IsDone)
                {
                    this.AudioFile.Close();
                    if (this.NewAudioRecordingDone != null)
                    {
                        this.NewAudioRecordingDone(this.FileName);
                    }
                    this.IsDone = true;
                }
            }
        }
        private void WriteString(Stream stream, string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            stream.Write(bytes, 0, bytes.Length);
        }
        private void writeWAVHeader()
        {	//We need to use a memory stream because the BinaryWriter will close the underlying stream when it is closed
            using (MemoryStream memStream = new MemoryStream(64))
            {				
                int cbFormat = 18; //sizeof(WAVEFORMATEX)
			    WAVEFORMATEX format = new WAVEFORMATEX()
				{
					wFormatTag = 1,
					nChannels = 1,
					nSamplesPerSec = 16000,
					nAvgBytesPerSec = 32000,
					nBlockAlign = 2,
					wBitsPerSample = 16,
					cbSize = 0
				};
	
				using (var bw = new BinaryWriter(memStream))
				{   
					//RIFF header
					WriteString(memStream, "RIFF");
					bw.Write(this.RecordingLength + cbFormat +  4 ); //File size - 8
					WriteString(memStream, "WAVE");                
					WriteString(memStream, "fmt ");
					bw.Write(cbFormat);
	
					//WAVEFORMATEX
					bw.Write(format.wFormatTag);
					bw.Write(format.nChannels);
					bw.Write(format.nSamplesPerSec);
					bw.Write(format.nAvgBytesPerSec);
					bw.Write(format.nBlockAlign);
					bw.Write(format.wBitsPerSample);
					bw.Write(format.cbSize);
	
					//data header
					WriteString(memStream, "data");					
					bw.Write(this.RecordingLength);
				    memStream.WriteTo(this.AudioFile);
				}
            }   
        }
    }
}
