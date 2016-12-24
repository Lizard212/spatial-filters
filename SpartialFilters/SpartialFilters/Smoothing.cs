using System.Drawing;

namespace SpartialFilters
{
    public class Smoothing
    {
        public Smoothing() { }


        public void Smooth(Bitmap bmpInput,Bitmap bmpOuput, int m)
        {

            Color c;
            float sum = 0;
            int[,] arrGray = null;
            int w = 0, h = 0, x, y;
            /* Convert color image to gray image */
            Common common = new Common();
            arrGray = common.ConvertToGray(bmpInput);

            for ( w = 0; w < bmpInput.Width - m; w++)
            {
                for ( h = 0; h < bmpInput.Height - m; h++)
                {
                    for ( x = w; x <= w + m; x++)
                    {
                        for ( y = h; y <= h + m; y++)
                        {
                            sum += arrGray[x,y];
 
                        }
                    }

                    int color = (int)System.Math.Round((sum / (m * m)), 10);
                    if (color > 255) { color = 255; }
                    bmpOuput.SetPixel(w + 1, h + 1, Color.FromArgb(color, color, color));
                    sum = 0;

                }
            }


        }

    }
}
