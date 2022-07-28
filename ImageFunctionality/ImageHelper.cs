using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace PhotoEditingApplication.ImageFunctionality;

[SupportedOSPlatform("windows")]
public static class ImageHelper
{
    
    /// <summary>
    /// Converts an image to grey scale by using the method of average values.
    /// </summary>
    /// <param name="imgBitmap"></param>
    /// <returns></returns>
    public static void ConvertToGrayscale(Bitmap imgBitmap)
    {
        var imgHeight = imgBitmap.Height;
        var imgWidth = imgBitmap.Width;

        for (var y = 0; y < imgHeight; y++)
        {
            for (var x = 0; x < imgWidth; x++)
            {
                var currentPixel = imgBitmap.GetPixel(x, y);
                var redValue =  0.299 * currentPixel.R;
                var greenValue =  0.587 * currentPixel.G;
                var blueValue =  0.114 * currentPixel.B;

                var sumValue =  redValue + greenValue + blueValue;
                
                var newPixel = Color.FromArgb(currentPixel.A, (int) sumValue, (int) sumValue, (int) sumValue);
                
                imgBitmap.SetPixel(x, y, newPixel);
            }
        }
    }

    /// <summary>
    /// Converts an image to just black and white. Works by effectively rounding down the average pixel value
    /// across R, G, and B and then going to 0 or 255 based off that (and thus black and white).
    /// </summary>
    /// <param name="imgBitmap"></param>
    /// <returns></returns>
    public static void ConvertToBlackWhite(Bitmap imgBitmap)
    {
        var imgHeight = imgBitmap.Height;
        var imgWidth = imgBitmap.Width;

        for (var y = 0; y < imgHeight; y++)
        {
            for (var x = 0; x < imgWidth; x++)
            {
                var currentPixel = imgBitmap.GetPixel(x, y);

                var sumOfPixels = currentPixel.R + currentPixel.G + currentPixel.B;

                imgBitmap.SetPixel(x, y, sumOfPixels > 128 ? Color.Black : Color.White);
                
            }
        }
    }

    public static Bitmap Neonify(Bitmap imgBitmap)
    {
        var neonifyKernel = new [,] {{-0.5, -1.0, -0.5}, {-1.0, 7, -1.0}, {-0.5, -1.0, -0.5}};
        var returnBitmap = ApplyKernel(imgBitmap, neonifyKernel);

        return returnBitmap;
    }

    public static Bitmap GaussianBlur(Bitmap imgBitmap)
    {
        var blurringKernel = new [,] {{0.0625, 0.125, 0.0625}, {0.125, 0.25, 0.125}, {0.0625, 0.125, 0.0625}};
        var returnBitmap = ApplyKernel(imgBitmap, blurringKernel);

        return returnBitmap;
    }
    
    public static Bitmap Dreamify(Bitmap imgBitmap)
    {
        var dreamifiedKernel = new [,] {{0, 1.0, 0}, {2.0, -2.5, 2.0}, {0, 1.0, 0}};
        var returnBitmap = ApplyKernel(imgBitmap, dreamifiedKernel);

        return returnBitmap;
    }

    public static double[,] GenerateRandomKernel()
    {
        var randomKernelValues = new double[3,3];
        var random = new Random();
        
        for (var i = 0; i < 9; i++)
        {
            var rDouble = random.NextDouble();
            var rRangeDouble = rDouble * (1);
        
            Console.WriteLine("Range double: " + rRangeDouble);
        }
        
        return randomKernelValues;
    }


    /// <summary>
    /// Function inspired from the following Stackoverflow comment: https://stackoverflow.com/a/1319999 
    /// </summary>
    /// <param name="bitmapImg"></param>
    /// <param name="kernel"></param>
    /// <returns></returns>
    private static Bitmap ApplyKernel(Image bitmapImg, double[,] kernel)
    {
        var sharpenedImage = (Bitmap)bitmapImg.Clone();

        const int filterWidth = 3;
        const int filterHeight = 3;
        var width = bitmapImg.Width;
        var height = bitmapImg.Height;

        const double factor = 1.0;
        const double bias = 0.0;

        var result = new Color[bitmapImg.Width, bitmapImg.Height];

        // Lock image bits for read/write.
        var pbits = sharpenedImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

        // Declare an array to hold the bytes of the bitmap.
        var bytes = pbits.Stride * height;
        var rgbValues = new byte[bytes];

        // Copy the RGB values into the array.
        System.Runtime.InteropServices.Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

        int rgb;
        
        // Fill the color array with the new sharpened color values.
        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                double red = 0.0, green = 0.0, blue = 0.0;

                for (var filterX = 0; filterX < filterWidth; filterX++)
                {
                    for (var filterY = 0; filterY < filterHeight; filterY++)
                    {
                        var imageX = (x - filterWidth / 2 + filterX + width) % width;
                        var imageY = (y - filterHeight / 2 + filterY + height) % height;

                        rgb = imageY * pbits.Stride + 3 * imageX;

                        red += rgbValues[rgb + 2] * kernel[filterX, filterY];
                        green += rgbValues[rgb + 1] * kernel[filterX, filterY];
                        blue += rgbValues[rgb + 0] * kernel[filterX, filterY];
                    }
                    
                    var r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                    var g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                    var b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                    result[x, y] = Color.FromArgb(r, g, b);
                }
            }
        }

        // Update the image with the sharpened pixels.
        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                rgb = y * pbits.Stride + 3 * x;

                rgbValues[rgb + 2] = result[x, y].R;
                rgbValues[rgb + 1] = result[x, y].G;
                rgbValues[rgb + 0] = result[x, y].B;
            }
        }

        // Copy the RGB values back to the bitmap.
        System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);

        sharpenedImage.UnlockBits(pbits);

        return sharpenedImage;
    }
}