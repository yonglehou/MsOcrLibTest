using System;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using Windows.UI.Xaml.Shapes;

namespace OCRTest
{
    public sealed partial class MainPage : Page, IMainView
    {
        private OCRHelper _ocrHelper;

        public MainPage()
        {
            this.InitializeComponent();
            _ocrHelper = new OCRHelper(this);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var view = CoreApplication.GetCurrentView();

            var fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            var selectedImageFile = await fileOpenPicker.PickSingleFileAsync();
            await _ocrHelper.ProcessImage(selectedImageFile);
        }

        public void ClearCanvas()
        {
            MyCanvas.Children.Clear();
        }

        public void SetText(string text)
        {
            txtText.Text = text;
        }

        public void SetImage(BitmapImage bitmapImage)
        {
            SelectedImage.Source = bitmapImage;
        }


        public Canvas GetCanvas()
        {
            return MyCanvas;
        }
    }
}
