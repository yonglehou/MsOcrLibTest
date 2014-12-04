using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace OCRTest
{
    public static class ImageHelper
    {
        public static async Task<IRandomAccessStream> BitmapScale(BitmapDecoder decoder, uint newHeight, uint newWidth)
        {
            // create a new stream and encoder for the new image
            var ras = new InMemoryRandomAccessStream();
            var enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);

            // convert the entire bitmap to a 100px by 100px bitmap
            enc.BitmapTransform.ScaledHeight = newHeight;
            enc.BitmapTransform.ScaledWidth = newWidth;


            var bounds = new BitmapBounds
            {
                Height = newHeight,
                Width = newWidth,
                X = 0,
                Y = 0
            };
            enc.BitmapTransform.Bounds = bounds;

            // write out to the stream
            try
            {
                await enc.FlushAsync();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }

            return ras;
        }

    }
}
