using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace test_MNIST
{
    public partial class Form1 : Form
    {
        //System.IO.File a;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap bmp=new Bitmap(28,28);
            byte[] contents = System.IO.File.ReadAllBytes(@"I:\MNist Database\New folder\train-images.idx3-ubyte");
            byte[] ContentsLabel = System.IO.File.ReadAllBytes(@"I:\MNist Database\New folder\train-labels.idx1-ubyte");
            System.IO.StreamWriter strWr = new System.IO.StreamWriter(@"I:\MNist Database\New folder\Train_images.txt");
           // var strWr = System.IO.File.CreateText(@"I:\MNist Database\New folder\Train_images.txt");
            int contentIndex= 16;
            int contentLabelIndex = 8;
            //int index=0;
            //string[] img=new string[801];
            for (int count = 0; count < 60000; count++)
            {
                //index = 0;
                {
                    //get image
                    for (int x = 0; x < 28; x++)
                    {
                        for (int y = 0; y < 28; y++)
                        {
                            if (contents[contentIndex] == (byte)0)
                                bmp.SetPixel(y, x, Color.White);
                            else
                                bmp.SetPixel(y, x, Color.Black);
                            contentIndex++;
                        }
                    }
                }
                {
                    //write image into new file
                    for (int x = 0; x < 28; x++)
                    {
                        for (int y = 0; y < 28; y++)
                        {
                            if (bmp.GetPixel(x,y).R==Color.White.R)
                                strWr.Write("0,");
                            else
                                strWr.Write("1,");
                        }
                    }
                    strWr.Write( ContentsLabel[contentLabelIndex++].ToString());
                    strWr.WriteLine();



                }
            }
            strWr.Close();
       }
    }
}
