using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Versioning;

namespace PhotoEditingApplication.ImageFunctionality;

[SupportedOSPlatform("windows")]
public static class ImageHelper
{
    public static Bitmap ConvertToGrayscale(Bitmap imgBitmap)
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
        
        
        return imgBitmap;
    }

    public static Bitmap ConvertToBlackWhite(Bitmap imgBitmap)
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
        
        return imgBitmap;
    }

    public static Bitmap ConvertToBlackWhite(string imgPath)
    {
        var imgBitmap = new Bitmap(imgPath);
        var resultBitmap = ConvertToBlackWhite(imgBitmap);

        return resultBitmap;
    }
    
}