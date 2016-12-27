using System;
using System.Drawing;

namespace SpartialFilters
{
    public class Smoothing
    {



        public static Bitmap Smooth(Bitmap bmpInput, int m)
        {

            Color c;
            float sum = 0;
          
            int w = 0, h = 0, x, y;
            int width = bmpInput.Width;
            int height = bmpInput.Height;
            Bitmap bmpOutput = new Bitmap(width, height);
            int[,] arrGray = null;
            /* Convert color image to gray image */
            Common common = new Common();
            arrGray = common.ConvertToGray(bmpInput);
          
            common.ResizeArray(ref arrGray, m-1, m -1, m -1 , m -1);

            for ( w = m -1 ; w < width + m -1 ; w++)
            {
                for ( h = m -1 ; h < height + m -1  ; h++)
                {
                    for ( x = w - m + 1  ; x < w ; x++)
                    {
                        for ( y = h - m + 1; y < h ; y++)
                        {
      
                            sum += arrGray[x,y];
                        }
                    }

                    int color = (int)System.Math.Round((sum / (m * m)), 10);
                    if (color > 255) { color = 255; }

                    bmpOutput.SetPixel(w - m + 1 , h - m + 1 , Color.FromArgb(color, color, color));
  
                    sum = 0;

                }
            }
            return bmpOutput;


        }

    }
}
