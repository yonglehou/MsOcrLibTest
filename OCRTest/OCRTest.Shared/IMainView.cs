using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OCRTest
{
    public interface IMainView
    {
        void ClearCanvas();
        void SetText(string txt);
        void SetImage(BitmapImage bitmapImage);
        Canvas GetCanvas();
    }
}
