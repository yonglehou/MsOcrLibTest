using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WindowsPreview.Media.Ocr;

namespace OCRTest
{
    public class OCRHelper
    {
        private IMainView _view;
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
                    var stream = await ImageHelper.BitmapScale(originalDecoder, MAXHEIGHT, MAXWIDTH);

                    var bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(stream);
                    _view.SetImage(bitmapImage);
                    await ProcessOCR(stream);
                }
            }
        }


        public async Task ProcessOCR(IRandomAccessStream stream)
        {
            _view.ClearCanvas();
            var imageData = await ImageData.CreateFromStream(stream);
            var ocrEngine = new OcrEngine(OcrLanguage.English);

            var rec = await ocrEngine.RecognizeAsync(imageData.OrientedPixelHeight, imageData.OrientedPixelWidth, imageData.Pixels);

            var text = "";
            if (rec.Lines != null)
            {
                foreach (var ocrLine in rec.Lines)
                {
                    foreach (var ocrWord in ocrLine.Words)
                    {
                        text += ocrWord.Text + " ";
                        _view.DrawRectangle(ocrWord.Left, ocrWord.Top, ocrWord.Width, ocrWord.Height, rec.TextAngle);
                    }
                    text += "\n";
                }
            }

            _view.SetText(text);
        }

    }


}
