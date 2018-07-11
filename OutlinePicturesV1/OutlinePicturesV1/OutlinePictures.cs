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

        /** This is a relative specificity to compare colours. That is, it should be between 0 and 255, (and I make no guarantees if it isnt)
        * and realistically it should be between 1 and 10 ish for any decent results. It represents the difference that two colours' RGB
        * values can be to still be considered the same. Ie, if COLOUR_SENEITIVTY = 10, Colour1's RGB values +- 10 are compared to Colour2's RGB values.
        * For complex images a suitable number seems to be around 2 */
        private static readonly int COLOUR_SENSITIVITY = 2;



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

                    if (ColourEquals(MiddleColour, LeftColour))
                    {
                        DifferentCount += 1;
                    }
                    if (ColourEquals(MiddleColour, RightColour))
                    {
                        DifferentCount += 1;
                    }
                    if (ColourEquals(MiddleColour, TopColour))
                    {
                        DifferentCount += 1;
                    }
                    if (ColourEquals(MiddleColour, BottomColour))
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


        /* This method compares two colours. It returns true if each RGB value of Colour1 is within COLOUR_SENSITIVITY percentage of Colour2 */
        private static bool ColourEquals(Color Color1, Color Color2)
        {
            //Return true when, for each of RGB, the second colour is between lower and upper bounds of first colour, OR vice versa
            if (    ((Color1.R - COLOUR_SENSITIVITY) <= Color2.R) && (Color2.R <= (Color1.R + COLOUR_SENSITIVITY)) || ((Color2.R - COLOUR_SENSITIVITY) <= Color1.R) && (Color1.R <= (Color2.R + COLOUR_SENSITIVITY))  &&
                    ((Color1.G - COLOUR_SENSITIVITY) <= Color2.G) && (Color2.G <= (Color1.G + COLOUR_SENSITIVITY)) || ((Color2.G - COLOUR_SENSITIVITY) <= Color1.G) && (Color1.G <= (Color2.G + COLOUR_SENSITIVITY))  &&
                    ((Color1.B - COLOUR_SENSITIVITY) <= Color2.B) && (Color2.B <= (Color1.B + COLOUR_SENSITIVITY)) || ((Color2.B - COLOUR_SENSITIVITY) <= Color1.B) && (Color1.B <= (Color2.B + COLOUR_SENSITIVITY))   )
            {
                return true;
            } else
            {
                return false;
            }
        }



    }
}


//Next step is probably a cutoff point for pixels that are slightly different but not very different
