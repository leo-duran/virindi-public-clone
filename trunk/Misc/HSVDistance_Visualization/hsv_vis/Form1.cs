using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace hsv_vis
{
    public partial class Form1 : Form
    {
        struct HSVColor
        {
            public double H;
            public double S;
            public double V;

            public void AddToH(double a)
            {
                H += a;
                if (H >= 360d) H -= 360d;
                if (H < 0d) H += 360d;
            }

            public Color ToColor()
            {
                double m, n, f, h;
                int i;
                h = H * 6d / 360d;
                i = (int)Math.Floor(h);
                f = h - i;
                if ((i & 1) == 0) f = 1 - f;
                m = V * (1 - S);
                n = V * (1 - S * f);

                double R = 0d, G = 0d, B = 0d;
                switch (i)
                {
                    case 6:
                    case 0: R = V; G = n; B = m; break;
                    case 1: R = n; G = V; B = m; break;
                    case 2: R = m; G = V; B = n; break;
                    case 3: R = m; G = n; B = V; break;
                    case 4: R = n; G = m; B = V; break;
                    case 5: R = V; G = m; B = n; break;
                }
                R *= 256d;
                G *= 256d;
                B *= 256d;
                return Color.FromArgb((int)R, (int)G, (int)B);
            }

            public HSVColor(Color c)
            {
                ColorToHSV(c, out H, out S, out V);
            }

            public static void ColorToHSV(System.Drawing.Color color, out double hue, out double saturation, out double value)
            {
                int max = Math.Max(color.R, Math.Max(color.G, color.B));
                int min = Math.Min(color.R, Math.Min(color.G, color.B));

                hue = color.GetHue();
                saturation = (max == 0) ? 0 : 1d - (1d * min / max);
                value = max / 255d;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        int x = 0;
        int y = 0;
        void box(Graphics g, Color c)
        {
            using (Brush b = new SolidBrush(c))
                g.FillRectangle(b, x, y, 50, 50);
            x += 50;
            if (x >= pictureBox1.Width)
            {
                x = 0;
                y += 50;
            }
        }

        bool Match(HSVColor ex, double hd, double sd, HSVColor test)
        {
            if (Math.Abs(ex.H - test.H) > hd) return false;
            double ss = ex.S - test.S;
            double vv = ex.V - test.V;
            double svdist = Math.Sqrt(ss * ss + vv * vv);

            return svdist <= sd;
        }

        Random rnd = new Random();
        private void button1_Click(object sender, EventArgs e)
        {
            Color excolorrgb = Color.FromArgb(int.Parse(txtColor.Text, System.Globalization.NumberStyles.AllowHexSpecifier));
            excolorrgb = Color.FromArgb(255, excolorrgb.R, excolorrgb.G, excolorrgb.B);
            double huediff = double.Parse(txtHueDiff.Text);
            double svdiff = double.Parse(txtSVDist.Text);

            x = 0;
            y = 0;
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.Clear(Color.White);
                box(g, excolorrgb);
                HSVColor excolor = new HSVColor(excolorrgb);

                int m = 0;
                while (m < 144)
                {
                    Color q = Color.FromArgb(255, rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
                    HSVColor qq = new HSVColor(q);

                    if (!Match(excolor, huediff, svdiff, qq)) continue;

                    box(g, q);

                    ++m;
                }
            }
        }
    }
}