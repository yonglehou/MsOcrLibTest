using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace OCRTest
{
    public interface IMainView
    {
        void ClearCanvas();
        void DrawRectangle(int left, int top, int width, int height, double? textAngle);
        void SetText(string txt);

        void SetImage(BitmapImage bitmapImage);
    }
}
