using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartialFilters
{
  public  class Sharpening
    {
     
   public static void Sharpen(Bitmap bmpInput, Bitmap bmpOutput)
        {
            Bitmap sharpenImage = (Bitmap)bmpInput.Clone();

            int filterWidth = 3;
            int filterHeight = 3;
            int width = bmpInput.Width;
            int height = bmpInput.Height;
            int[,] arrGray = null;
            int x, y, i, j;
            double pixel_value = 0;
            int new_value;

            // Create sharpening filter
            double[,] filter = new double[filterWidth, filterHeight];
            filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = 1;
            filter[1, 1] = - 8;

            double bias = 1.5;
            Common common = new Common();
            arrGray = common.ConvertToGray(sharpenImage);
            for ( i = 1; i < width -1; i++)
            {
                for ( j = 1; j < height -1 ; j++)
                {
                    pixel_value = 0.0;
                    for ( x = -1; x < filterWidth -1; x++)
                    {
                        for ( y = -1; y < filterHeight -1; y++)
                        {
                            pixel_value += filter[x + 1, y + 1] * arrGray[i + x, j + y];

                        }
                    }
                    new_value = (int)(arrGray[i, j] - bias * pixel_value);
                    if (new_value < 0) new_value = 0;
                    if (new_value > 255) new_value = 255;
                    bmpOutput.SetPixel(i, j , Color.FromArgb(new_value, new_value, new_value));
                }
            }
           
        }
    }
}
