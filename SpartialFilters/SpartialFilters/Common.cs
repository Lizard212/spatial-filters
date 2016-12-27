using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpartialFilters
{
  public  class Common
    {
        public Common()
        {

        }
        public int[,] ConvertToGray(Bitmap bmpInput)
        {
            Color c;
            int width = bmpInput.Width;
            int height = bmpInput.Height;
            int[,] arrGray = new int[width, height];

            /* Convert color image to gray image */
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    c = bmpInput.GetPixel(w, h);
                    byte gray = (byte)(.333 * c.R + .333 * c.G + .333 * c.B);
                    arrGray[w,h] = gray;
                }
            }
            return arrGray;

        }

        public static Bitmap ColorToGray(Bitmap image)
        {
            Color c;
            Bitmap grayImage = (Bitmap)image.Clone();
            int width = image.Width;
            int height = image.Height;
            int[,] arrGray = new int[width, height];

            /* Convert color image to gray image */
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    c = image.GetPixel(w, h);
                    byte gray = (byte)(.333 * c.R + .333 * c.G + .333 * c.B);
                    grayImage.SetPixel(w, h, Color.FromArgb(gray, gray, gray));
                }
            }
            return grayImage;
        }
        public  void ResizeArray<T>(
            ref T[,] array, int padLeft, int padRight, int padTop, int padBottom)
        {
            int ow = array.GetLength(0);
            int oh = array.GetLength(1);
            int nw = ow + padLeft + padRight;
            int nh = oh + padTop + padBottom;

            int x0 = padLeft;
            int y0 = padTop;
            int x1 = x0 + ow - 1;
            int y1 = y0 + oh - 1;
            int u0 = -x0;
            int v0 = -y0;

            if (x0 < 0) x0 = 0;
            if (y0 < 0) y0 = 0;
            if (x1 >= nw) x1 = nw - 1;
            if (y1 >= nh) y1 = nh - 1;

            T[,] nArr = new T[nw, nh];
            for (int y = y0; y <= y1; y++)
            {
                for (int x = x0; x <= x1; x++)
                {
                    nArr[x, y] = array[u0 + x, v0 + y];
                }
            }
            array = nArr;
        }
    }
}
