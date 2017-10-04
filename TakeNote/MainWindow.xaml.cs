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
        public MainWindow()
        {
            InitializeComponent();
            dontJudgeMe();
        }

        private void dontJudgeMe()
        {
            var speech = SpeechClient.Create();
            var response = speech.Recognize(new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                SampleRateHertz = 44100,
                LanguageCode = "en",
            }, RecognitionAudio.FromFile(@"D:\once.wav"));
            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    File.AppendAllText(@"D:\transcript.txt", alternative.Transcript);
                    Console.WriteLine(alternative.Transcript);
                }
            }
        }
    }
}
