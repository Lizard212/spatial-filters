using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
        string fileSave = null;
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
                    Bitmap bmp = new Bitmap(img);
                    
                    pictureBox_src.Image = Common.ColorToGray(bmp);

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(filePath !=null)
            {
                Image img = Image.FromFile(filePath);
                Bitmap bmp = new Bitmap(img);

               
               
                pictureBox2.Image = Smoothing.Smooth(bmp, 3);
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Image Files (*.bmp)|*.BMP";
            dialog.DefaultExt = "BMP";
            dialog.AddExtension = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileSave = dialog.FileName;
                if(fileSave != null)
                {

                    pictureBox2.Image.Save(fileSave, ImageFormat.Bmp);
                }            

            }
        }
    }
}
