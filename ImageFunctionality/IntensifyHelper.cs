using System.Drawing;
using System.Runtime.Versioning;

namespace PhotoEditing.ImageProcessing;

[SupportedOSPlatform("windows")]
public static class IntensifyHelper
{
    
    public static Bitmap Intensify(Bitmap imgBitmap, int redIntensity, int greenIntensity, int blueIntensity)
    {
        var imgWidth = imgBitmap.Width;
        var imgHeight = imgBitmap.Height;

        for (var y = 0; y < imgHeight; y++)
        {
            for (var x = 0; x < imgWidth; x++)
            {
                var currentPixel = imgBitmap.GetPixel(x, y);
                var redValue = currentPixel.R + redIntensity < 255 ? currentPixel.R + redIntensity : 255;
                var greenValue = currentPixel.G + greenIntensity < 255 ? currentPixel.G + greenIntensity : 255;
                var blueValue = currentPixel.B + blueIntensity < 255 ? currentPixel.B + blueIntensity : 255;

                var modifiedColor = Color.FromArgb(currentPixel.A, redValue, greenValue, blueValue);
                
                imgBitmap.SetPixel(x, y, modifiedColor);
            }
        }
        
        return imgBitmap;
    }

    public static Bitmap Intensify(string imgPath, int redIntensity, int greenIntensity, int blueIntensity)
    {
        var imgBitmap = new Bitmap(imgPath);
        var resultBitmap = Intensify(imgBitmap, redIntensity, greenIntensity, blueIntensity);

        return resultBitmap;
    }
    
    public static void IntensifyRed(Bitmap imgBitmap, int intensity)
    {
        Intensify(imgBitmap, intensity, 0, 0);
    }

    private static Color RedIntensifyHelper(Color color, int intensity)
    {
        var redValue = color.R;
        var modifiedRedValue = redValue + intensity < 255 ? redValue + intensity : 255;

        return Color.FromArgb(color.A, modifiedRedValue, color.G, color.B);
    }
    
    public static void IntensifyGreen(Bitmap imgBitmap, int intensity)
    {
        Intensify(imgBitmap, 0, intensity, 0);
    }

    private static Color GreenIntensifyHelper(Color color, int intensity)
    {
        var greenValue = color.G;
        var modifiedGreenValue = greenValue + intensity < 255 ? greenValue + intensity : 255;

        return Color.FromArgb(color.A, color.R, modifiedGreenValue, color.B);
    }
    
    
    public static void IntensifyBlue(Bitmap imgBitmap, int intensity)
    {
        Intensify(imgBitmap, 0, 0, intensity);
    }

    private static Color BlueIntensifyHelper(Color color, int intensity)
    {
        var blueValue = color.B;
        var modifiedBlueValue = blueValue + intensity < 255 ? blueValue + intensity : 255;

        return Color.FromArgb(color.A, color.R, color.G, modifiedBlueValue);
    }
    
}