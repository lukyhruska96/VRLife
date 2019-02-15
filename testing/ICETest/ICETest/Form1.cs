using _3dSoundSynthesis;
using NAudio.Wave;
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

namespace ICETest
{
    public partial class Form1 : Form
    {
        private Point center;

        private Ear lEar;

        // private Ear rEar;

        private Source source;

        private List<IMovableObject> enumObjects = new List<IMovableObject>();

        private Timer timer = new Timer();

        private Mp3FileReader mp3Reader;

        private SourceLocation sourceLocation;

        public Form1()
        {
            InitializeComponent();
            this.center = new Point(this.panel1.Width / 2, this.panel1.Height / 2);
            this.lEar = new Ear(side_e.E_LEFT, new Point(this.center.X + 60, this.center.Y));
            //this.rEar = new Ear(side_e.E_RIGHT, new Point(this.center.X + 60, this.center.Y));
            this.source = new Source(center);
            enumObjects.Add(this.lEar);
            //enumObjects.Add(this.rEar);
            enumObjects.Add(this.source);
            timer.Enabled = true;
            timer.Interval = 20;
            timer.Tick += new EventHandler(TimerCallback);
            mp3Reader = new Mp3FileReader("test.mp3");
            WaveOut player = new WaveOut();
            sourceLocation = new SourceLocation(0, 0, 0);
            VoiceStream voiceStream = new VoiceStream(mp3Reader.ToSampleProvider(), new ILowLevelVoiceEffect[] { new BinauralSynthesis(sourceLocation) }, new IHighLevelVoiceEffect[0]);
            ISampleProvider sampleProvider = voiceStream.GetSampleProvider();
            voiceStream.Run();
            //using (WaveFileWriter writer = new WaveFileWriter("output.wav", sampleProvider.WaveFormat))
            //{
            //    float[] buff = new float[13000];
            //    for (int i = 0; i < 200; ++i)
            //    {
            //        int read = sampleProvider.Read(buff, 0, 13000);
            //        writer.WriteSamples(buff, 0, read);
            //    }
            //}

            player.PlaybackStopped += (_, __) =>
            {
                mp3Reader.Dispose();
            };
            player.Init(sampleProvider);
            player.Play();
        }

        private void TimerCallback(object sender, EventArgs e)
        {
            this.panel1.Invalidate();
            return;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            this.lEar_label.Text = $"{this.lEar.location.X};{this.lEar.location.Y}";
            //this.rEar_label.Text = $"{this.rEar.location.X};{this.rEar.location.Y}";
            e.Graphics.Clear(Color.White);
            foreach (IMovableObject obj in enumObjects)
                obj.ProccessPaint(e.Graphics);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (IMovableObject obj in enumObjects)
                obj.MouseMove(e.Location);
            sourceLocation.Azim = (180.0/Math.PI) * - Math.Atan2(lEar.location.Y - source.location.Y, lEar.location.X - source.location.X);
            this.azimLabel.Text = $"{sourceLocation.Azim}°";
            this.cursor_label.Text = $"{e.Location.X};{e.Location.Y}";
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (IMovableObject obj in enumObjects)
                obj.MouseDown(e.Location);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (IMovableObject obj in enumObjects)
                obj.MouseUp();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {

        }
    }

    public interface IMovableObject
    {
        void MouseDown(Point p);

        void MouseUp();

        void MouseMove(Point p);

        void ProccessPaint(Graphics g);
    }

    public enum side_e { E_LEFT, E_RIGHT };

    public class Ear : IMovableObject
    {
        private side_e side;

        public Point location;

        private bool holding;

        private string text;

        private static Pen pen = new Pen(Color.Black, 3);

        private static readonly int radius = 15;

        private static StringFormat sf = new StringFormat();

        static Ear()
        {
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
        }

        public Ear(side_e side, Point location)
        {
            this.side = side;
            this.location = location;
            if (side == side_e.E_LEFT) text = "LE";
            else text = "RE";
        }

        public void ProccessPaint(Graphics g) 
        {
            g.DrawEllipse(pen, location.X - radius, location.Y - radius, radius * 2, radius * 2);
            g.DrawString(text, SystemFonts.DefaultFont, Brushes.Black, new PointF(location.X, location.Y), sf);
        }

        public void MouseMove(Point location)
        {
            if(holding)
            {
                this.location = location;
            }
        }

        public void MouseDown(Point cursor)
        {
            if(Math.Sqrt(Math.Pow(cursor.X - location.X, 2) + Math.Pow(cursor.Y - location.Y, 2)) <= radius+2)
            {
                this.holding = true;
            }
        }

        public void MouseUp()
        {
            this.holding = false;
        }
    }

    public class Source : IMovableObject
    {

        public Point location;

        private bool holding;

        private string text;

        private static Pen pen = new Pen(Color.Black, 3);

        private static readonly int radius = 15;

        private static StringFormat sf = new StringFormat();

        static Source() {
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
        }

        public Source(Point location)
        {
            this.location = location;
            this.text = "S";
        }
        
        public void ProccessPaint(Graphics g)
        {
            g.DrawEllipse(pen, location.X - radius, location.Y - radius, radius * 2, radius * 2);
            g.DrawString(text, SystemFonts.DefaultFont, Brushes.Black, new PointF(location.X, location.Y), sf);
        }

        public void MouseMove(Point location)
        {
            if (holding)
            {
                this.location = location;
            }
        }

        public void MouseDown(Point cursor)
        {
            if (Math.Sqrt(Math.Pow(cursor.X - location.X, 2) + Math.Pow(cursor.Y - location.Y, 2)) <= radius + 2)
            {
                this.holding = true;
            }
        }

        public void MouseUp()
        {
            this.holding = false;
        }
    }

    public class ICE : IHighLevelVoiceEffect
    {
        private Ear lEar, rEar;

        private Source source;

        float max_dist;

        public ICE(Ear lEar, Ear rEar, Source source, float height, float width)
        {
            this.lEar = lEar;
            this.rEar = rEar;
            this.source = source;
            this.max_dist = Math.Max(height, width);
        }
        async public Task<VoiceStreamStruct> ProccessAsync(Task<VoiceStreamStruct> data)
        {
            float lDistance = (float)Math.Sqrt(Math.Pow(this.lEar.location.X - this.source.location.X, 2)+Math.Pow(this.lEar.location.Y - this.source.location.Y, 2));
            float rDistance = (float)Math.Sqrt(Math.Pow(this.rEar.location.X - this.source.location.X, 2) + Math.Pow(this.rEar.location.Y - this.source.location.Y, 2));
            VoiceStreamStruct voiceStreamStruct = await data;
            voiceStreamStruct.LeftChannelBalance = (max_dist - lDistance) / max_dist;
            voiceStreamStruct.RightChannelBalance = (max_dist - rDistance) / max_dist;
            return voiceStreamStruct;
        }
    }
}
