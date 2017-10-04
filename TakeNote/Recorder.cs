using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeNote
{
    public class Recorder
    {

        WaveIn sourceStream;
        WaveFileWriter waveWriter;
        readonly String FilePath;
        readonly String FileName;
        readonly int InputDeviceIndex;

        public Recorder(int inputDeviceIndex, String filePath, String fileName)
        {
           // InitializeComponent();
            this.InputDeviceIndex = inputDeviceIndex;
            this.FileName = fileName;
            this.FilePath = filePath;
        }

        public void StartRecording()
        {
            sourceStream = new WaveIn
            {
                DeviceNumber = this.InputDeviceIndex,
                WaveFormat =
                    new WaveFormat(44100, 1)//WaveIn.GetCapabilities(this.InputDeviceIndex).Channels)
            };

            sourceStream.DataAvailable += this.SourceStreamDataAvailable;

            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            waveWriter = new WaveFileWriter(FilePath + FileName, sourceStream.WaveFormat);
            sourceStream.StartRecording();
        }

        public void SourceStreamDataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveWriter == null) return;
            waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
            waveWriter.Flush();
        }

        public string RecordEnd()
        {
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
                sourceStream.Dispose();
                sourceStream = null;
            }
            if (this.waveWriter == null)
            {
                return string.Empty;
            }
            this.waveWriter.Dispose();
            this.waveWriter = null;

            return string.Format("{0}{1}", this.FilePath, this.FileName);
         //   recordEndButton.Enabled = false;
            //Application.Exit();
          //  Environment.Exit(0);
        }
    }
}
