using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace testing_ne1_HWDR_in_Cs
{
    public partial class Form1 : Form
    {
        Bitmap mainbmp;
        Net net = new Net();
        MUXtoPCA mtp = new MUXtoPCA();
        IEnumerable<string> TrainedImages;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //resample the main image into dest image
            if (pictureBox1.Image != null)
            {
                byte[,] _data = new byte[90, 90];
                Bitmap srcImg = new Bitmap(pictureBox1.Image);
                //convert source image into bytes to resample
                for (int x = 0; x < 90; x++)
                {
                    for (int y = 0; y < 90; y++)
                    {
                        if (srcImg.GetPixel(x, y).R == Color.White.R)
                            _data[x, y] = 0;
                        else
                            _data[x, y] = 1;
                    }
                }
                byte[,] res = ResampleImage.Resample(_data);
                //show the resampled image on picturebox
                pictureBox2.Image = ResampleImage.GetImage(res, 28, 28);
                //reshape 28*28 array into 1*784 array
                Byte[] reshaped = ResampleImage.Reshape(res);
                //convert reshaped array from bytes into double
                double[] reshapeddb = new double[784];
                for (int i = 0; i < 784; i++)
                    reshapeddb[i] = (double)reshaped[i];
                lbl_output.Text = GetAns(reshapeddb);
                lbl_NetOutput.Text = net.m_NetOutput.ToString();
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == (MouseButtons.Left))
            {
                Bitmap bmp;
                bmp = new Bitmap(pictureBox1.Image);
                Graphics gra = Graphics.FromImage(bmp);
                gra.FillEllipse(Brushes.Black, new Rectangle(new Point((MousePosition.X - (this.Location.X + pictureBox1.Location.X) - 20) + 2, (MousePosition.Y - (this.Location.Y + pictureBox1.Location.Y) - 60)),
                    new Size(5, 5)));
                pictureBox1.Image = bmp;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = mainbmp;
            pictureBox2.Image = null;
            lbl_output.Text = "";
            lbl_NetOutput.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //initializing UserInput Box
            mainbmp = new Bitmap(90, 90);
            for (int x = 0; x < 90; x++)
                for (int y = 0; y < 90; y++)
                    mainbmp.SetPixel(x, y, Color.White);
            pictureBox1.Image = mainbmp;
            //reading TrainedImages
            TrainedImages = System.IO.File.ReadLines("TrainedData.txt");
        }

        private string GetAns(double[] dta_1_784 )
        {
            //PCA Data*image Data
            double[] re = mtp.Mux(dta_1_784);
            double ans = net.Calculate(re);
            return (ans < 0.15 ? "0" : "1");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
            pictureBox3.Image=ReadImage((int)(numericUpDown1.Value));

        }

        private Bitmap ReadImage(int index)
        {
            string []IMG=TrainedImages.ElementAt<string>(index-1).Split(',');
            Bitmap bmp=new Bitmap(28,28);
            //converting image from text into bitmap
            for (int i = 0; i < 28; i++)
                for (int y = 0; y < 28; y++)
                    if (IMG[i * 28+y] == "0")
                        bmp.SetPixel(i, y, Color.White);
                    else
                        bmp.SetPixel(i, y, Color.Black);
            double[] imgdata = new double[784];
            for (int i = 0; i < 784; i++)
                imgdata[i] = double.Parse(IMG[i]);
            label10.Text=GetAns(imgdata);
            label11.Text = net.m_NetOutput.ToString();

                return bmp;
        }
    }
}
