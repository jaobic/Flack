using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SampleTagger.AudioAnalysis;
using NAudio;
using NAudio.Wave;
using System.Windows.Forms.DataVisualization.Charting;

namespace Flack
{
    public partial class formMain : Form
    {
        public int FFTSize = 512;

        public formMain()
        {
            InitializeComponent();
        }

        public void ReadFlac(string filename)
        {
            AudioAnalyser analys = new AudioAnalyser(FFTSize);

            //Initialize the colours
            analys.SpectrumColors = null;

            NAudio.Flac.FlacReader reader = new NAudio.Flac.FlacReader(filename);

            //Set all the format related label text
            var format = reader.WaveFormat;

            lblSampleRate.Text = "Sample Rate: " + (format.SampleRate / 1000).ToString() + " KHz";
            lblChannels.Text = "Channels: " + format.Channels.ToString();
            lblBitDepth.Text = "Bit Depth: " + format.BitsPerSample.ToString();

            double originalSize = (double)reader.Length / (1024 * 1024);
            double compressedSize = (double)new System.IO.FileInfo(filename).Length / (1024 * 1024);

            lblSize.Text = "File Size: "
                + Math.Round(compressedSize, 2).ToString()
                + " MB ("
                + Math.Round(100 * compressedSize / originalSize, 1).ToString()
                + "% of original size, "
                + Math.Round(originalSize, 2).ToString()
                + " MB)";

            lblBitrate.Text = "Average Bitrate: " + Math.Round(8192 * compressedSize / reader.TotalTime.TotalSeconds, 0).ToString() + " kbps";

            int sampleRate = format.SampleRate / 2;
            
            //Start analyzing the audio data
            analys.OnNewFile((int)reader.Length / format.BlockAlign, format.SampleRate, format.BitsPerSample);

            byte[] buffer = new byte[format.BlockAlign];
            
            //Read all audio samples
            while (reader.Position < reader.Length)
            {
                reader.Read(buffer, 0, buffer.Length);

                double sample = 0;

                for (int i = buffer.Length - 1; i >= 0; i -= 2)
                {
                    //Convert to a double
                    int v = ((0xFFFF * (buffer[1] >> 7)) << 16) | (buffer[1] << 8) | buffer[0];

                    sample = (v / (double)0x8000);
                }

                analys.OnNextSample(sample, 0);
            }

            //Calculate frequency averages
            double ratio = sampleRate / (FFTSize / 2);
            List<double> averages = new List<double>();

            for (int i = 0; i < FFTSize / 2; i++)
            {
                double average = 0;

                for (int x = 0; x < analys.fftState.nbFFT; x++)
                    average += analys.fftState.magnitudes[x, i];

                average /= analys.fftState.nbFFT;

                //Add it to the graph
                chtFrequencies.Series[0].Points.Add(new DataPoint(ratio * i, average));

                averages.Add(average);
            }

            //Adjust the graph to fit all data
            chtFrequencies.ChartAreas[0].AxisX.CustomLabels[1].FromPosition = sampleRate - 200;
            chtFrequencies.ChartAreas[0].AxisX.CustomLabels[1].ToPosition = sampleRate;

            int lowest = (int)chtFrequencies.Series[0].Points.Min(x => x.YValues[0]);

            //Calculate the shelf frequency
            double shelfdB = -100;

            if (lowest < -140)
            {
                shelfdB = lowest + 1;
                chtFrequencies.ChartAreas[0].AxisY.Minimum = lowest;
            }

            chtFrequencies.ChartAreas[0].AxisX.Maximum = sampleRate;

            double shelf = averages.Reverse<double>().DefaultIfEmpty(averages[(FFTSize / 2) - 1]).FirstOrDefault(x => x > shelfdB);

            double shelfFreq = Math.Min((averages.IndexOf(shelf) + 2) * ratio, sampleRate);

            string shelfString = Math.Round(shelfFreq / 1000, 1).ToString() + " KHz";


            //Draw the spectrograph
            Bitmap image = analys.GenerateSpectrum(imgSpectrograph.Width, imgSpectrograph.Height);

            DrawImage(image, shelfFreq / sampleRate, shelfString);
        }

        public void DrawImage(Bitmap image, double position, string text)
        {
            using (Graphics gr = Graphics.FromImage(image))
            {
                //Draw the line
                Brush shelfBrush = Brushes.Red;

                Pen shelf = new Pen(shelfBrush, 2);

                shelf.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                float ypos = (float)(image.Height - (position * image.Height));

                gr.DrawLine(shelf, 0, ypos, image.Width, ypos);
                
                //Draw the text
                Font drawFont = new Font("Consolas", 8);
                SizeF stringSize = gr.MeasureString(text, drawFont);
                PointF stringPoint = new PointF(0, ypos + 2);

                //Draw the black background first
                gr.FillRectangle(Brushes.Black, new RectangleF(stringPoint, stringSize));

                gr.DrawString(text, drawFont, Brushes.Red, stringPoint);
            }

            imgSpectrograph.Image = image;
        }

        private void formMain_Shown(object sender, EventArgs e)
        {
            //Open a file dialog
            OpenFileDialog file = new OpenFileDialog()
            {
                Filter = "FLAC audio files (*.flac)|*.flac"
            };

            //Do nothing if not OK
            if (file.ShowDialog() != DialogResult.OK)
                return;
            
            //Process the audio
            ReadFlac(file.FileName);
        }
    }
}
