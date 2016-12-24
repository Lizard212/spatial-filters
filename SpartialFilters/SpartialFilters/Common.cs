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
            float sum = 0;
            int[,] arrGray = new int[bmpInput.Width, bmpInput.Height];
            /* Convert color image to gray image */
            for (int w = 0; w < bmpInput.Width; w++)
            {
                for (int h = 0; h < bmpInput.Height; h++)
                {
                    c = bmpInput.GetPixel(w, h);
                    byte gray = (byte)(.333 * c.R + .333 * c.G + .333 * c.B);
                    arrGray[w,h] = gray;

                }
            }
            return arrGray;

        }
    }
}
