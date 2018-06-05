using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Prism.Commands;
using PropertyChanged;

namespace CV_Edge
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public sealed partial class MainPage : Page
    {
        private const string ModelPath = @"ms-appx:///Assets/HeroModel.onnx";
        public HeroModelInput ModelInput { get; set; } = new HeroModelInput();
        public HeroModelOutput ModelOutput { get; set; } = new HeroModelOutput();
        public HeroModel Model { get; set; } = new HeroModel();

        public ICommand AnalyzeCommand { get; set; }
        public ICommand BrowseCommand { get; set; }

        public string Results { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this;

            AnalyzeCommand = new DelegateCommand(AnalyzeImage);
            BrowseCommand = new DelegateCommand(BrowseForImage);

            LoadModel();
        }

        private async void LoadModel()
        {
            var modelFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModelPath));
            Model = await HeroModel.CreateHeroModel(modelFile);
        }


        private async void BrowseForImage()
        {
            Results = "Press Analyze to process image...";
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                using (IRandomAccessStream fileStream =
                    await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();

                    bitmapImage.SetSource(fileStream);
                    MainImage.Source = bitmapImage;
                }
            }
        }

        private async void AnalyzeImage()
        {
            var rtBitmap = new RenderTargetBitmap();
            await rtBitmap.RenderAsync(MainImage);
            var buffer = await rtBitmap.GetPixelsAsync();
            var softwareBitmap = SoftwareBitmap.CreateCopyFromBuffer(buffer, BitmapPixelFormat.Bgra8,
                rtBitmap.PixelWidth, rtBitmap.PixelHeight);

            buffer = null;
            rtBitmap = null;

            var frame = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);
            //var croppedFrame = await CropAndDisplayInputImageAsync(frame);

            ModelInput.data = frame;
            ModelOutput = await Model.EvaluateAsync(ModelInput);

            if (ModelOutput.classLabel.Count > 0)
            {
                Results = $"{ModelOutput.classLabel.First()} - ";
            }
            else
            {
                Results = "could not identify image";
            }
        }
    }
}
