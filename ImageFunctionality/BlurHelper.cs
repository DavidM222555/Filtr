using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Versioning;

namespace PhotoEditingApplication.ImageFunctionality;

[SupportedOSPlatform("windows")]
public static class BlurHelper
{
    /// <summary>
    ///  Distorts an image by blocking elements together into windows and then turning all pixels
    ///  in that window into the average, creating a 'blocky' distorted image
    /// </summary>
    /// <param name="bitmap"></param>
    /// <param name="windowLength">Length of window that we will slide across the pixels of the image with</param>
    /// <param name="windowHeight">Height of window that we will slide across the pixels of the image with</param>
    public static Bitmap BoxDistort(Bitmap bitmap, int windowLength, int windowHeight)
    {
        var imgWidth = bitmap.Width;
        var imgHeight = bitmap.Height;
        
        var xDivs = imgWidth / windowLength;
        var yDivs = imgHeight / windowHeight;

        for (var y = 0; y < yDivs; y++)
        {
            for (var x = 0; x < xDivs; x++)
            {
                // Get our current position in the array
                var yStartPos = y * windowHeight;
                var xStartPos = x * windowLength;

                var listOfPixels = new List<Color>();
                var indicesToModify = new List<Tuple<int, int>>();

                var yCounter = 0;
                var xCounter = 0;

                while (yCounter < windowHeight)
                {
                    while (xCounter < windowLength)
                    {
                        var yPos = yStartPos + yCounter;
                        var xPos = xStartPos + xCounter;

                        indicesToModify.Add(Tuple.Create(xPos, yPos));
                        listOfPixels.Add(bitmap.GetPixel(xPos, yPos));
                        xCounter += 1;
                    }

                    yCounter += 1;
                    xCounter = 0; // Reset the x counter by going to the next row and then iterating over the cols
                }

                var pixelAvg = GetPixelAverage(listOfPixels);

                foreach (var (item1, item2) in indicesToModify)
                {
                    bitmap.SetPixel(item1, item2, pixelAvg);
                }
                
            }
        }

        return bitmap;
    }
    
    /// <summary>
    /// Performs a blur along horizontal lines of pixels. Works by getting the average of the neighbors of a pixel
    /// and then 
    /// </summary>
    /// <param name="imgBitmap"></param>
    /// <param name="numberOfRounds">Number of times to repeat the process</param>
    public static Bitmap HorizontalBlur(Bitmap imgBitmap, int numberOfRounds)
    {
        var imgWidth = imgBitmap.Width;
        var imgHeight = imgBitmap.Height;

        for (var i = 0; i < numberOfRounds; i++)
        {
            for (var y = 0; y < imgHeight; y++)
            {
                for (var x = 1; x < imgWidth - 2; x++)
                {
                    var listOfPixels = new List<Color>
                    {
                        imgBitmap.GetPixel(x - 1, y),
                        imgBitmap.GetPixel(x, y),
                        imgBitmap.GetPixel(x + 1, y)
                    };

                    var averagedPixel = GetPixelAverage(listOfPixels);

                    imgBitmap.SetPixel(x, y, averagedPixel);
                }
            }
        }
        

        return imgBitmap;
    }
    
    public static Bitmap HorizontalBlur(string imgPath, int numberOfRounds)
    {
        var imgBitmap = new Bitmap(imgPath);
        var resultBitmap = HorizontalBlur(imgBitmap, numberOfRounds);
        
        return resultBitmap;
    }
    
    /// <summary>
    /// Performs a blur along vertical lines of pixels.
    /// </summary>
    /// <param name="imgBitmap"></param>
    /// <param name="numberOfRounds">Number of times to repeat the process</param>
    /// <returns>Modifies imgBitmap</returns>
    public static Bitmap VerticalBlur(Bitmap imgBitmap, int numberOfRounds)
    {

        var imgWidth = imgBitmap.Width;
        var imgHeight = imgBitmap.Height;

        for (var i = 0; i < numberOfRounds; i++)
        {
            for (var y = 1; y < imgHeight - 2; y++)
            {
                for (var x = 0; x < imgWidth; x++)
                {
                    
                    var listOfPixels = new List<Color>
                    {
                        imgBitmap.GetPixel(x, y - 1),
                        imgBitmap.GetPixel(x, y),
                        imgBitmap.GetPixel(x, y + 1)
                    };

                    var averagedPixel = GetPixelAverage(listOfPixels);

                    imgBitmap.SetPixel(x, y, averagedPixel);
                }
            }
        }
        

        return imgBitmap;
    }

    public static Bitmap VerticalBlur(string imgPath, int numberOfRounds)
    {
        var imgBitmap = new Bitmap(imgPath);
        var resultBitmap = VerticalBlur(imgBitmap, numberOfRounds);

        return resultBitmap;
    }

    /// <summary>
    /// Blurs along both the vertical and horizontal axis using both VerticalBlur and HorizontalBlur, effectively
    /// implementing a box blur algorithm.
    /// </summary>
    /// <param name="imgBitmap"></param>
    /// <param name="numberOfRounds">Number of times to do the process</param>
    /// <returns></returns>
    public static Bitmap Blur(Bitmap imgBitmap, int numberOfRounds)
    {
        var resultBitmap = imgBitmap;
        
        for (var i = 0; i < numberOfRounds; i++)
        {
            resultBitmap = HorizontalBlur(resultBitmap, numberOfRounds);
            resultBitmap = VerticalBlur(resultBitmap, numberOfRounds);
        }
        

        return resultBitmap;
    }

    public static Bitmap Blur(string imgPath, int numberOfRounds)
    {
        var bitmap = new Bitmap(imgPath);
        var resultBitmap = Blur(bitmap, numberOfRounds);

        return resultBitmap;
    }
    
    /// <summary>
    /// Given a list of pixel values it averages the A, R, G, and B values.
    /// </summary>
    /// <param name="pixels"></param>
    /// <returns>A color object with the averaged values of A, R, G, and B.</returns>
    private static Color GetPixelAverage(List<Color> pixels)
    {
        var (rSum, gSum, bSum, aSum) = (0, 0, 0, 0);
        var numOfPixels = pixels.Count;
        
        foreach(var pixel in pixels)
        {
            var r = pixel.R;
            rSum += r;
            
            var g = pixel.G;
            gSum += g;
            
            var b = pixel.B;
            bSum += b;
            
            var a = pixel.A;
            aSum += a;
        }

        var aAvg = aSum / numOfPixels;
        var rAvg = rSum / numOfPixels;
        var gAvg = gSum / numOfPixels;
        var bAvg = bSum / numOfPixels;

        return Color.FromArgb(aAvg, rAvg, gAvg, bAvg);
    }
    
}