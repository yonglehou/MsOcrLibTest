
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;


namespace OCRTest
{

    public sealed partial class MainPage : Page, IMainView
    {
        private OCRHelper _ocrHelper;
        private FileOpenPicker _fileOpenPicker;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            _ocrHelper = new OCRHelper(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            RegisterFileOpenPicker();
        }



        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            _fileOpenPicker.PickSingleFileAndContinue();
        }

        private void RegisterFileOpenPicker()
        {
            var view = CoreApplication.GetCurrentView();

            _fileOpenPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            _fileOpenPicker.FileTypeFilter.Add(".jpg");
            _fileOpenPicker.FileTypeFilter.Add(".jpeg");
            _fileOpenPicker.FileTypeFilter.Add(".png");


            view.Activated += async (applicationView, args) =>
            {
                var fileOpenPickerArgs = args as FileOpenPickerContinuationEventArgs;


                if (fileOpenPickerArgs != null && fileOpenPickerArgs.Files.Count > 0)
                {
                    var selectedImageFile = fileOpenPickerArgs.Files[0];
                    await _ocrHelper.ProcessImage(selectedImageFile, OcrLanguage.SelectedItem.ToString());
                }
            };
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
