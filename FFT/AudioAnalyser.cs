using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace SampleTagger.AudioAnalysis
{
    /// <summary>
    /// Provide all values of the current FFT<br/>
    ///  - imaginary part<br/>
    ///  - real part<br/>
    ///  - Hanning analysis window<br/>
    ///  - size of the window<br/>
    /// </summary>
    public class FFTState
    {
        /// <summary>
        /// [input] Hanning window of size fft_size
        /// </summary>
        public double[] window;
        /// <summary>
        /// [input] Input audio signal of size fft_size
        /// </summary>
        public double[] input;
        /// <summary>
        /// [output]  Imaginary part of the FFT of size fft_size
        /// </summary>
        public double[] im;
        /// <summary>
        /// [output]  Real part of the FFT of size fft_size
        /// </summary>
        public double[] re;
        /// <summary>
        /// [output]  The audiospectrum, magnitude for each FFT (calculated from imaginary and real parts).
        /// The size are [nbFFT,fft_size]
        /// </summary>
        public double[,] magnitudes;
        /// <summary>
        /// [input] Number of FFT required to analyze the entire input audio signal.
        /// we overlap windows with fft_size/2
        /// </summary>
        public int nbFFT;
        /// <summary>
        /// [input] Size of a single FFT
        /// </summary>
        public int fft_size;

        /// <summary>
        /// Prepare values for the FFT
        /// </summary>
        /// <param name="nbSamples">number of samples to analyze</param>
        /// <param name="fft_size">size of the FFT</param>
        public FFTState(int nbSamples, int fft_size)
        {
            this.fft_size = fft_size;
            window = new double[fft_size];
            im = new double[fft_size];
            re = new double[fft_size];
            input = new double[fft_size];
            nbFFT = (nbSamples / (fft_size / 2)) - 1; // overlaping window with fft_size/2
            magnitudes = new double[nbFFT, fft_size/2];

            // prepare Hanning analysis window
            for (int i = 0; i < fft_size; i++)
                window[i] = (4.0 / fft_size) * 0.5 * (1 - Math.Cos(2 * Math.PI * i / fft_size)); // Hanning Vector [1] = 1 / 4595889085.750801

        }
        /// <summary>
        /// Compute the audio spectrum in dB
        /// </summary>
        /// <param name="x">nth FFT from 0 to nbFFT-1</param>
        /// <param name="y">nth magnitude from 0 to fft_size-1</param>
        public void ComputeMagnitude(int x,int y)
        {
            // compute magnitude with the good old Pythagorean theorem
            double product = re[y] * re[y] + im[y] * im[y];

            magnitudes[x, y] = Math.Sqrt(product);

            // convert to dB
            magnitudes[x, y] = ((double)20) * Math.Log10(magnitudes[x, y] + Double.Epsilon);

        }
    }

    /// <summary>
    /// Associate a color to a dB level
    /// </summary>
    public class SpectrumColor
    {
        public Color color;
        public double value_dB;
        public double gradient_pos;
        public String value;

        public SpectrumColor(String c, double value_dB)
        {
            this.color = ColorTranslator.FromHtml(c);
            this.value = c;
            this.value_dB = value_dB;
        }
    }

    /// <summary>
    /// Gradient used to compute the audio spectrum.<br/>
    /// We use System.Drawing.Drawing2D.LinearGradientBrush to generate the
    /// Gradient
    /// </summary>
    public class SpectrumGradient
    {
        public double min_dB;
        public double max_dB;
        public double amplitude_dB;
        public SpectrumColor[] colors;
        private Color[] gradient;

        /// <summary>
        /// Prepare a gradient
        /// </summary>
        /// <param name="min_dB">should be the lowest dB found in the parameter colors</param>
        /// <param name="max_dB">should be 0 dB</param>
        /// <param name="colors">colors/dB values provided by the .INI</param>
        public SpectrumGradient(double min_dB, double max_dB, SpectrumColor[] colors)
        {
            this.min_dB = min_dB;
            this.max_dB = max_dB;
            this.colors = colors;
            this.amplitude_dB = max_dB - min_dB;
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i].gradient_pos = dBtoPercent(colors[i].value_dB);
            }
            gradient = CreateGradient(100);
        }        
        /// <summary>
        /// Convert a dB value in range [0,1] regarding min and max 
        /// </summary>
        /// <param name="value_dB"></param>
        /// <returns></returns>
        public double dBtoPercent(double value_dB)
        {
            double result = value_dB;
            result = Math.Max(result, min_dB);
            result = Math.Min(result, max_dB);
            result = (result - min_dB) / amplitude_dB;
            return result;
        }
        /// <summary>
        /// Return the right color for a specific dB value
        /// </summary>
        /// <param name="value_dB"></param>
        /// <returns></returns>
        public Color pickColor(double value_dB)
        {
            int idx = (int)(dBtoPercent(value_dB) * (gradient.Length-1));
            return gradient[idx];
        }
        /// <summary>
        /// Compute the gradient with a given width
        /// </summary>
        /// <param name="width">should be 100 as we work with percentages</param>
        /// <returns></returns>
        private Color[] CreateGradient(int width)
        {
            Color[] result = new Color[width];
            int gradient_height = 1;
            using (Bitmap bitmap = new Bitmap(width, gradient_height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    ColorBlend cb = new ColorBlend();
                    int nbColord = colors.Length + 1;
                    cb.Colors = new Color[nbColord];
                    cb.Positions = new float[nbColord];
                    for (int c = 0; c < colors.Length; c++)
                    {
                        cb.Colors[c] = colors[c].color;
                        cb.Positions[c] = (float)colors[c].gradient_pos;
                    }
                    cb.Colors[nbColord - 1] = cb.Colors[nbColord - 2];
                    cb.Positions[nbColord - 1] = 1.0f;

                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        new Rectangle(0, 0, width, gradient_height),
                        Color.Black,
                        Color.Black,
                        LinearGradientMode.Horizontal))
                    {
                        brush.InterpolationColors = cb;

                        graphics.FillRectangle(brush, new Rectangle(0, 0, width, gradient_height));
                        for (int x = 0; x < width; x++)
                        {
                            result[x] = bitmap.GetPixel(x, 0);
                            
                        }

                        //bitmap.Save("gradient.png");
                        return result;
                    }
                }
            }
        }
    }
    /// <summary>
    /// This class generate an audio spectrum regarding a color table
    /// provided in .INI<br/>
    /// A list of audio features are also extracted<br/>
    /// <br/>
    /// The caller should use this class in this way:<br/>
    /// <code>
    /// OnNewFile()
    /// for each samples read by the caller
    ///     OnNextSample()
    /// OnEndOfFile()
    /// GenerateSpectrum()
    /// </code>
    /// </summary>
    public class AudioAnalyser
    {
        private FFT2 fft = new FFT2();
        private int fft_size;
        private int fft_size_logN;
        public FFTState fftState = null;
        private int current_sample_count;
        private int current_magnitudes_count;
        private SpectrumColor[] spectrumColors;

        /// <summary>
        /// Colors/dB values provided in INI file
        /// </summary>
        public SpectrumColor[] SpectrumColors
        {
            get { return spectrumColors; }
            set { spectrumColors = value; prepareSpectrumGradient(); }
        }

        /// <summary>
        /// Gradient used to generate the audio spectrum
        /// </summary>
        private SpectrumGradient sg = null;

        /// <summary>
        /// Initialize the gradient regarding the color/dB provided by the caller
        /// or use default color/dB 
        /// </summary>
        private void prepareSpectrumGradient()
        {
            if (spectrumColors == null || spectrumColors.Length<2)
            {
                sg = new SpectrumGradient(
                -108, 0,
                new SpectrumColor[] 
                {
                    new SpectrumColor("#000000",-108),
                    new SpectrumColor("#6D005F",-72),
                    new SpectrumColor("#FF0000",-50),
                    new SpectrumColor("#FFE900",-30)
                });
            }
            else
            {
                double minDb = Double.MaxValue;
                foreach (SpectrumColor sc in spectrumColors)
                {
                    minDb = Math.Min(minDb, sc.value_dB);
                }
                // create a gradient matching minDb to 0dB
                sg = new SpectrumGradient(
                    minDb, 0,
                    spectrumColors
                    );
            }
        }
        /// <summary>
        /// Create an AudioAnayze given a specific fft size
        /// </summary>
        /// <param name="fft_size">WARNING: fft_size must be a power of 2</param>
        public AudioAnalyser(int fft_size)
        {
            this.fft_size = fft_size;
            fft_size_logN = (int)Math.Log(fft_size, 2);
        }
        /// <summary>
        /// Tell if the FFT has worked.
        /// </summary>
        /// <returns></returns>
        public bool HasResult()
        {
            return fftState != null;
        }
        /// <summary>
        /// Prepare the FFT computation regarding the format of the input signal
        /// </summary>
        /// <param name="nbSamples"></param>
        /// <param name="sampleFrequency"></param>
        /// <param name="bitdepth"></param>
        public void OnNewFile(int nbSamples,int sampleFrequency,int bitdepth)
        {
            fftState = new FFTState(nbSamples, fft_size);
            current_sample_count = 0;
            current_magnitudes_count = 0;
            fft.init((uint)fft_size_logN);
        }
        /// <summary>
        /// Update the internal FFT computation with a new sample
        /// </summary>
        /// <param name="sample">sample normalized in [0,1]</param>
        /// <param name="channel">channel [0,n] only 0 will be used</param>
        public void OnNextSample(double sample, int channel)
        {
            if (channel != 0)
                return;

            fftState.input[current_sample_count] = sample;
            current_sample_count++;

            if (current_sample_count == fft_size)
            {
                //String r = ""; //debug output in CSV

                // apply the Hanning window
                for (int i = 0; i < fft_size; i++)
                {
                    fftState.re[i] = fftState.input[i] * fftState.window[i];
                    fftState.im[i] = 0;
                    //r += ";" + fftState.input[i];
                }

                // run the FFT
                fft.run(fftState.re, fftState.im);

                // because of nyquist, half of FFT is the result

                for (int i = 0; i < fft_size / 2; i++)
                {
                    // normalize output (imaginary part does not need this)
                    fftState.re[i] = fftState.re[i] / fft_size;
                    fftState.ComputeMagnitude(current_magnitudes_count, i);
                    //r += ";" + fftState.magnitudes[current_magnitudes_count, i];
                    //r += ";" + fftState.im[i];
                }
                //File.WriteAllText("fft-256.csv", r);

                current_magnitudes_count++;

                // overlaping window fft_size / 2
                Array.Copy(fftState.input, fft_size / 2, fftState.input, 0, fft_size / 2);
                current_sample_count = fft_size/2;
            }
        }
        /// <summary>
        /// Stretch the internal audio spectrum to the requested size
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Bitmap GenerateSpectrum(int width,int height)
        {
            // first we compute an audio spectrum to the same size of the data
            int spectrum_width = fftState.nbFFT;
            int spectrum_height = fft_size / 2;
            Bitmap spectrum = new Bitmap(spectrum_width, spectrum_height);
            LockBitmap lb = new LockBitmap(spectrum);
            lb.LockBits();
            for (int x = 0; x < spectrum_width; x++)
            {
                for (int y = 0; y < spectrum_height; y++)
                {
                    // pick up a color
                    Color col = sg.pickColor(fftState.magnitudes[x, y]);
                    // draw a point
                    lb.SetPixel(x, spectrum_height - y - 1, col);
                }
            }
            lb.UnlockBits();
            // then we stretch it to the requested size
            Bitmap rescaledSpectrum = RescaleSpectrum(spectrum, width, height);
            return rescaledSpectrum;
        }
        /// <summary>
        /// Stretch a bitmap to the requested size
        /// </summary>
        /// <param name="spectrum"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Bitmap RescaleSpectrum(Bitmap spectrum, int width, int height)
        {
            Bitmap rescaledSpectrum = new Bitmap(width, height,PixelFormat.Format24bppRgb);
            using (Graphics graph = Graphics.FromImage(rescaledSpectrum))
            {
                graph.InterpolationMode = InterpolationMode.High;
                graph.CompositingQuality = CompositingQuality.HighQuality;
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                graph.DrawImage(spectrum, new Rectangle(0, 0, width, height));
                return rescaledSpectrum;
            }
        }
        
    }    
}
