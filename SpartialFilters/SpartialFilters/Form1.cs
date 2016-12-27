using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpartialFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string filePath = null;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            dialog.InitialDirectory = @"C:\";
            dialog.Title = "Please select an image to process";

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                 filePath = dialog.FileName;
                if (filePath != null)
                {
                    Image img = Image.FromFile(filePath);
                    pictureBox_src.Image = img;

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(filePath !=null)
            {
                Image img = Image.FromFile(filePath);
                Bitmap bmp = new Bitmap(img);

                Bitmap bmpOutput = bmp;
                     
     
                Smoothing.Smooth(bmp, bmpOutput, 10);
                pictureBox2.Image = bmpOutput;
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (filePath != null)
            {
                Image img = Image.FromFile(filePath);
                Bitmap bmp = new Bitmap(img);

                Bitmap bmpOutput = bmp;


                Sharpening.Sharpen(bmp, bmpOutput);


                pictureBox2.Image = bmpOutput;
            }
        }
    }
}
