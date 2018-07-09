using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlinePicturesV1
{
    static class OutlinePictures
    {


        /* This method is the crux of the algorithm. It takes an Image as input and returns a grey and black outline only representation
         * of the same image.  */

        public static System.Drawing.Image OutlineImage(System.Drawing.Image img)
        {
            //Image to return
            Bitmap ReturnImage = new Bitmap(img);

            //Bitmap representation of the image
            Bitmap BitmapImage = new Bitmap(img);

            //Nested loops through pixels of image
            for (int x = 0; x < img.Width; x += 1)
            {
                for (int y = 0; y < img.Height; y += 1)
                {
                    //Analysed pixel values
                    var MiddleColour = BitmapImage.GetPixel(x, y);
                    var LeftColour = BitmapImage.GetPixel(Math.Max(0, x-1), y);
                    var RightColour = BitmapImage.GetPixel(Math.Min(BitmapImage.Width-1, x+1), y);
                    var TopColour = BitmapImage.GetPixel(x, Math.Min(BitmapImage.Height-1, y+1));
                    var BottomColour = BitmapImage.GetPixel(x, Math.Max(0, y-1));

                    //Count for how many of the 4 pixels are different to the middle pixel
                    int DifferentCount = 0;
                    if (MiddleColour != LeftColour)
                    {
                        DifferentCount += 1;
                    }
                    if (MiddleColour != RightColour)
                    {
                        DifferentCount += 1;
                    }
                    if (MiddleColour != TopColour)
                    {
                        DifferentCount += 1;
                    }
                    if (MiddleColour != BottomColour)
                    {
                        DifferentCount += 1;
                    }

                    //If 3 or 1 are different to the centre pixel, we are on a colour border so we make it black
                    if (DifferentCount == 3 || DifferentCount == 1 || DifferentCount == 2)
                    {
                        ReturnImage.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        ReturnImage.SetPixel(x, y, Color.LightGray);
                    }


                }
            }



            return (System.Drawing.Image) ReturnImage;
        }




    }
}
