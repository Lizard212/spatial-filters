using System;
using System.Drawing;

namespace SpartialFilters
{
    public class Smoothing
    {



        public static void Smooth(Bitmap bmpInput,Bitmap bmpOuput, int m)
        {

            Color c;
            float sum = 0;
          
            int w = 0, h = 0, x, y;
            int width = bmpInput.Width;
            int height = bmpInput.Height;
            int[,] arrGray = null;
            int[,] newArrGray = new int[width + m, height + m];
            /* Convert color image to gray image */
            Common common = new Common();
            arrGray = common.ConvertToGray(bmpInput);

             common.ResizeArray(ref arrGray, m-1, m -1, m -1 , m -1);
            for ( w = 0; w < width  ; w++)
            {
                for ( h = 0; h < height  ; h++)
                {
                    for ( x = w ; x <= w + m; x++)
                    {
                        for ( y = h ; y <= h + m; y++)
                        {
       
                            sum += arrGray[x,y];
                        }
                    }

                    int color = (int)System.Math.Round((sum / (m * m)), 10);
                    if (color > 255) { color = 255; }
                    bmpOuput.SetPixel(w , h , Color.FromArgb(color, color, color));
                    sum = 0;

                }
            }


        }

    }
}
