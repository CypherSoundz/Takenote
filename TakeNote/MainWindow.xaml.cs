using Gma.System.MouseKeyHook;
using Google.Cloud.Speech.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TakeNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
private IKeyboardMouseEvents m_GlobalHook;
        public MainWindow()
        {
            InitializeComponent();
            hook();
          //  dontJudgeMe();
        }

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }
        string pathName;
        bool recording;
        int count;
        Recorder rec;
        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("KeyPress: \t{0}", e.KeyChar);

            if (e.KeyChar == '\u001a') //ctrl+z
            {
                 rec = new Recorder(0, @"D:\", string.Format("test{0}.wav", count++));
                if (!recording)
                {
                    recording = true;
                    // pathName = "d:\test.wav";
                    rec.StartRecording();
                }
            }
            
            if(e.KeyChar == '\u0018')
            {
                string path = rec.RecordEnd();
                recording = false;
                dontJudgeMe(path);
            }
            
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            Console.WriteLine("MouseDown: \t{0}; \t System Timestamp: \t{1}", e.Button, e.Timestamp);

            // uncommenting the following line will suppress the middle mouse button click
            // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
        }

        public void Unsubscribe()
        {
            m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }

        private void hook()
        {
            Subscribe();
        }
        private void RecordFromMic()
        {
          
        }
        private void dontJudgeMe(string filepath)
        {
            var speech = SpeechClient.Create();
            var response = speech.Recognize(new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                SampleRateHertz = 44100,
                LanguageCode = "en",
            }, RecognitionAudio.FromFile(filepath));
            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    File.AppendAllText(string.Format("{0}.txt",filepath), alternative.Transcript);
                    Console.WriteLine(alternative.Transcript);
                }
            }
        }
    }
}
