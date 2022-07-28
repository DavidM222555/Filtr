using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using PhotoEditing.ImageProcessing;
using PhotoEditingApplication.ImageFunctionality;


namespace PhotoEditingApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string _currentFile = "";
        private Bitmap? _bitmapOfCurrentImg;
        
        public MainWindow()
        {
            
            InitializeComponent();
        }

        // Function taken from the following stackoverflow comment: https://stackoverflow.com/a/34590774
        private static BitmapImage Convert(Bitmap src)
        {
            var ms = new MemoryStream();
            src.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
        
        
        private void SelectNeuromancer_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFile == "") return;

            try
            {
                if (_bitmapOfCurrentImg != null)
                {
                    ImageHelper.ConvertToBlackWhite(_bitmapOfCurrentImg);
                    ImageHelper.GaussianBlur(_bitmapOfCurrentImg);
                    IntensifyHelper.Intensify(_bitmapOfCurrentImg, 35, 0, 0);
                
                    ImageBox.Source = Convert(_bitmapOfCurrentImg);
                }
                else
                {
                    var bitmapOfImg = new Bitmap(_currentFile);
                    ImageHelper.ConvertToBlackWhite(bitmapOfImg);
                    BlurHelper.BoxDistort(bitmapOfImg, 2, 2);
                    IntensifyHelper.Intensify(bitmapOfImg, 35, 0, 0);

                    ImageBox.Source = Convert(bitmapOfImg);
                    _bitmapOfCurrentImg = bitmapOfImg;
                }
            }
            catch
            {
                Console.WriteLine("Failed to load image");
            }
        }
        
        private void SelectDread_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFile == "") return;

            try
            {
                if (_bitmapOfCurrentImg != null)
                {
                    ImageHelper.ConvertToGrayscale(_bitmapOfCurrentImg);
                    IntensifyHelper.IntensifyRed(_bitmapOfCurrentImg, 50);
                    BlurHelper.VerticalBlur(_bitmapOfCurrentImg, 3);

                    ImageBox.Source = Convert(_bitmapOfCurrentImg);
                }
                else
                {
                    var bitmapOfImg = new Bitmap(_currentFile);
                    ImageHelper.ConvertToGrayscale(bitmapOfImg);
                    IntensifyHelper.IntensifyRed(bitmapOfImg, 50);
                    BlurHelper.VerticalBlur(bitmapOfImg, 3);

                    ImageBox.Source = Convert(bitmapOfImg);
                    _bitmapOfCurrentImg = bitmapOfImg;
                }
            }
            catch
            {
                Console.WriteLine("Failed to load image");
            }
        }

        private void SelectMotionSick_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFile == "") return;
            
            try
            {
                if (_bitmapOfCurrentImg != null)
                {
                    var blurredImage = ImageHelper.GaussianBlur(_bitmapOfCurrentImg);
                    IntensifyHelper.IntensifyRed(blurredImage, 30);
                    
                    ImageBox.Source = Convert(blurredImage);
                    _bitmapOfCurrentImg = blurredImage;
                }
                else
                {
                    var bitmapOfImg = new Bitmap(_currentFile);
                    
                    var blurredImage = ImageHelper.GaussianBlur(bitmapOfImg);
                    IntensifyHelper.IntensifyRed(blurredImage, 30);

                    ImageBox.Source = Convert(blurredImage);
                    _bitmapOfCurrentImg = blurredImage;
                }
            }
            catch
            {
                Console.WriteLine("Failed to load image");
            }
        }

        private void SelectColorPop_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFile == "") return;
            
            try
            {
                if (_bitmapOfCurrentImg != null)
                {
                    var sharpenedImage = ImageHelper.Neonify(_bitmapOfCurrentImg);
                    ImageBox.Source = Convert(sharpenedImage);

                    _bitmapOfCurrentImg = sharpenedImage;
                }
                else
                {
                    var bitmapOfImg = new Bitmap(_currentFile);
                    var sharpenedImage = ImageHelper.Neonify(bitmapOfImg);
                    
                    ImageBox.Source = Convert(sharpenedImage);
                    _bitmapOfCurrentImg = sharpenedImage;
                }
            }
            catch
            {
                Console.WriteLine("Failed to load image");
            }
        }

        private void BrightChasm_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFile == "") return;
            
            try
            {
                if (_bitmapOfCurrentImg != null)
                {
                    var sharpenedImage = ImageHelper.Neonify(_bitmapOfCurrentImg);
                    ImageBox.Source = Convert(sharpenedImage);

                    _bitmapOfCurrentImg = sharpenedImage;
                }
                else
                {
                    var bitmapOfImg = new Bitmap(_currentFile);
                    var sharpenedImage = ImageHelper.Neonify(bitmapOfImg);
                    
                    ImageBox.Source = Convert(sharpenedImage);
                    _bitmapOfCurrentImg = sharpenedImage;
                }
            }
            catch
            {
                Console.WriteLine("Failed to load image");
            }
        }

        private void DarkNova_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFile == "") return;
            
            try
            {
                if (_bitmapOfCurrentImg != null)
                {
                    var dreamifiedImage = ImageHelper.Dreamify(_bitmapOfCurrentImg);
                    var blurredImage = ImageHelper.GaussianBlur(dreamifiedImage);

                    ImageBox.Source = Convert(blurredImage);

                    _bitmapOfCurrentImg = blurredImage;
                }
                else
                {
                    var bitmapOfImg = new Bitmap(_currentFile);
                    var dreamifiedImage = ImageHelper.Dreamify(bitmapOfImg);
                    var blurredImage = ImageHelper.GaussianBlur(dreamifiedImage);

                    ImageBox.Source = Convert(blurredImage);
                    _bitmapOfCurrentImg = blurredImage;
                }
            }
            catch
            {
                Console.WriteLine("Failed to load image");
            }
        }

        
        private void Random_Click(object sender, RoutedEventArgs e)
        {
            
        }
        

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
            };

            openFileDialog.ShowDialog();
            var fileName = openFileDialog.FileName;

            if (fileName == "") return;
            var newImage = new BitmapImage(new Uri(fileName));

            ImageBox.Source = newImage;
            _bitmapOfCurrentImg = null;
            _currentFile = fileName;
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            if (_bitmapOfCurrentImg == null) return;
            
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
            };

            saveFileDialog.ShowDialog();
            
            _bitmapOfCurrentImg.Save(saveFileDialog.FileName);
        }
    }
}