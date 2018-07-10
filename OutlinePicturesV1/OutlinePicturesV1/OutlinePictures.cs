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

        /** This is a PERCENTAGE specificity to compare colours. That is, it should be between 0 and 100, (and I make no guarantees if it isnt)
        * and realistically it should be between 1 and 10 for any decent results. It represents the percentage different that two colours' RGB
        * values can be to still be considered the same. Ie, if Colour1 RGB is within X percentage of Colour2 RGB values */
        private static readonly double COLOUR_SENSITIVITY = 1.5;



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
            //Factors for comparison
            double UpperFactor = 1.00 + (COLOUR_SENSITIVITY / 100);
            double LowerFactor = 1.00 - (COLOUR_SENSITIVITY / 100);
            //Console.WriteLine(UpperFactor);
            //Console.WriteLine(LowerFactor);

            //Return true when, for each of RGB, the second colour is between lower and upper bounds of first colour, OR vice versa
            if (    ((Color1.R * LowerFactor) <= Color2.R) && (Color2.R <= (Color1.R * UpperFactor)) || ((Color2.R * LowerFactor) <= Color1.R) && (Color1.R <= (Color2.R * UpperFactor))  &&
                    ((Color1.G * LowerFactor) <= Color2.G) && (Color2.G <= (Color1.G * UpperFactor)) || ((Color2.G * LowerFactor) <= Color1.G) && (Color1.G <= (Color2.G * UpperFactor))  &&
                    ((Color1.B * LowerFactor) <= Color2.B) && (Color2.B <= (Color1.B * UpperFactor)) || ((Color2.B * LowerFactor) <= Color1.B) && (Color1.B <= (Color2.B * UpperFactor))   )
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
