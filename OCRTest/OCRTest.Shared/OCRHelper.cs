using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using WindowsPreview.Media.Ocr;

namespace OCRTest
{
    public class OCRHelper
    {
        private IMainView _view;
        private int _imageHeight;
        private int _imageWidth;
        private const uint MAXHEIGHT = 2600;
        private const uint MAXWIDTH = 2600;


        public OCRHelper(IMainView view)
        {
            _view = view;
        }


        public async Task ProcessImage(StorageFile selectedImageFile)
        {
            if (selectedImageFile != null)
            {
                using (var fileStream = await selectedImageFile.OpenAsync(FileAccessMode.Read))
                {
                    var originalDecoder = await BitmapDecoder.CreateAsync(fileStream);
                    IRandomAccessStream stream = null;
                    if (originalDecoder.OrientedPixelHeight > MAXHEIGHT || originalDecoder.OrientedPixelWidth > MAXWIDTH)
                    {
                        stream = await ImageHelper.BitmapScale(originalDecoder, MAXHEIGHT, MAXWIDTH);
                    }
                    else
                    {
                        stream = fileStream;
                    }

                    var bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(stream);
                    _imageHeight = bitmapImage.PixelHeight;
                    _imageWidth = bitmapImage.PixelWidth;
                    _view.SetImage(bitmapImage);
                    await ProcessOCR(stream);
                }
            }
        }


        private async Task ProcessOCR(IRandomAccessStream stream)
        {
            _view.ClearCanvas();
            var imageData = await ImageData.CreateFromStream(stream);
            var ocrEngine = new OcrEngine(OcrLanguage.Russian);
            //var ocrEngine = new OcrEngine(OcrLanguage.Swedish);
            //var ocrEngine = new OcrEngine(OcrLanguage.English);

            var rec = await ocrEngine.RecognizeAsync(imageData.OrientedPixelHeight, imageData.OrientedPixelWidth, imageData.Pixels);

            var text = "";
            if (rec.Lines != null)
            {
                foreach (var ocrLine in rec.Lines)
                {
                    foreach (var ocrWord in ocrLine.Words)
                    {
                        text += ocrWord.Text + " ";
                        DrawRectangle(ocrWord.Left, ocrWord.Top, ocrWord.Width, ocrWord.Height, rec.TextAngle, _imageHeight, _imageWidth);
                    }
                    text += "\n";
                }
            }

            _view.SetText(text);
        }

        public void DrawRectangle(int left, int top, int width, int height, double? textAngle, int originalImageHeight, int originalImageWidth)
        {
            var theCanvas = _view.GetCanvas();
            var uniformScaleX = 1.0;
            var uniformScaleY = 1.0;
            var offsetY = 0;
            var offsetX = 0;
            var imgHeight = theCanvas.ActualHeight;
            var imgWidth = theCanvas.ActualHeight;
            if (originalImageWidth > originalImageHeight)
            {
                uniformScaleY = ((float)originalImageHeight / originalImageWidth);
                offsetY = (int)((1.0 - (float)originalImageHeight / originalImageWidth) * imgHeight / 2);
            }
            else
            {
                uniformScaleX = ((float)originalImageWidth / originalImageHeight);
                offsetX = (int)((1.0 - (float)originalImageWidth / originalImageHeight) * imgWidth / 2);
            }

            var scaleY = (imgHeight / originalImageHeight) * uniformScaleY;
            var scaleX = imgWidth / originalImageWidth * uniformScaleX;

            var rectangle = new Rectangle
            {
                Height = height * scaleY,
                Width = width * scaleX,
                Stroke = new SolidColorBrush(Colors.White),
            };

            Canvas.SetLeft(rectangle, left * scaleX + offsetX);
            Canvas.SetTop(rectangle, top * scaleY + offsetY);
            theCanvas.Children.Add(rectangle);
        }


    }


}
