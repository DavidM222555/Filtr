using System.Drawing;
using System.Runtime.Versioning;

namespace PhotoEditing.ImageProcessing;

[SupportedOSPlatform("windows")]
public static class IntensifyHelper
{
    /// <summary>
    /// Takes a given bitmap and adds an additive constant to all pixels for their RGB values
    /// </summary>
    /// <param name="imgBitmap">Bitmap of a given image we want to modify</param>
    /// <param name="redIntensity">Constant value to scale the red values of pixels by</param>
    /// <param name="greenIntensity">Constant value to scale the green values of pixels by</param>
    /// <param name="blueIntensity">Constant value to scale the blue values of pixels by</param>
    /// <returns>Nothing. Modifies bitmap</returns>
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

    /// <summary>
    /// Helper function that calls Intensify on an image stored at imgPath -- works by converting imgPath to a bitmap
    /// </summary>
    /// <param name="imgPath"></param>
    /// <param name="redIntensity"></param>
    /// <param name="greenIntensity"></param>
    /// <param name="blueIntensity"></param>
    /// <returns>Returns a bitmap that represents the image at the imgPath with modified pixel values</returns>
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

    /// <summary>
    /// Modifies a pixel's R value by adding intensity to it. If the value would be greater than 255 we make it 255
    /// </summary>
    /// <param name="color"></param>
    /// <param name="intensity"></param>
    /// <returns>Nothing. Modifies color</returns>
    private static Color RedIntensifyHelper(Color color, int intensity)
    {
        var redValue = color.R;
        var modifiedRedValue = redValue + intensity < 255 ? redValue + intensity : 255;

        return Color.FromArgb(color.A, modifiedRedValue, color.G, color.B);
    }
    
    /// <summary>
    /// Modifies a pixel's G value by adding intensity to it. If the value would be greater than 255 we make it 255
    /// </summary>
    /// <param name="color"></param>
    /// <param name="intensity"></param>
    /// <returns>Nothing. Modifies color</returns>
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
    
    /// <summary>
    /// Modifies a pixel's B value by adding intensity to it. If the value would be greater than 255 we make it 255
    /// </summary>
    /// <param name="color"></param>
    /// <param name="intensity"></param>
    /// <returns>Nothing. Modifies color</returns>
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