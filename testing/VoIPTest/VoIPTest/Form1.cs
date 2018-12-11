using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoIPLib;
using NAudio.Wave;
using System.IO;

namespace VoIPTest
{
    public partial class Form1 : Form
    {
        private MicrophoneStream microphoneStream = null;

        private Task proccess;
        private BufferedWaveProvider bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat());
        private WaveOut player;

        public Form1()
        {
            InitializeComponent();
            microphoneStream = new MicrophoneStream();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            proccess = Task.Run(() =>
            {
                // set up playback
                player = new WaveOut
                {
                    DesiredLatency = 200                    
                };
                IWaveProvider waveProvider = microphoneStream.GetWaveProvider();
                player.Init(waveProvider);
                
                player.Play();
            });
        }
    }
}
