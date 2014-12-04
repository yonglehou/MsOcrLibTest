using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace OCRTest
{
    public class ImageData
    {
        private readonly BitmapDecoder _decoder;
        public byte[] Pixels { get; private set; }

        public uint OrientedPixelHeight { get { return _decoder.OrientedPixelHeight; } }
        public uint OrientedPixelWidth { get { return _decoder.OrientedPixelWidth; } }

        private ImageData(BitmapDecoder decoder, byte[] pixels)
        {
            Pixels = pixels;
            _decoder = decoder;
        }


        public static async Task<ImageData> CreateFromStream(IRandomAccessStream stream)
        {
            var bitmapDecoder = await BitmapDecoder.CreateAsync(stream);
            var pixelData = await bitmapDecoder.GetPixelDataAsync();
            var pixels = pixelData.DetachPixelData();
            return new ImageData(bitmapDecoder, pixels);
        }
    }
}
