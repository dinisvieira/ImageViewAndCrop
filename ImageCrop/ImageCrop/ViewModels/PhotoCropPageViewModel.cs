using System.Diagnostics;
using System.IO;
using ImageCrop.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace ImageCrop.ViewModels
{
    class PhotoCropPageViewModel : BindableBase, INavigationAware
    {

        private IImageCropper _imageCropper;
        private readonly IEventAggregator _eventAggregator;

        public PhotoCropPageViewModel(IImageCropper imageCropper, IEventAggregator eventAggregator)
        {
            _imageCropper = imageCropper;
            _eventAggregator = eventAggregator;

            StartCropCommand = new DelegateCommand(OnStartCropCommandExecuted);
        }

        private void OnReceiveCroppedImageResult(byte[] imgByteArr)
        {
            if (imgByteArr != null && imgByteArr.Length > 0)
            {
                LoadNewImage(imgByteArr);
            }
        }

        private ImageSource _imageSource = null;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }

        public DelegateCommand StartCropCommand { get; }

        private async void OnStartCropCommandExecuted()
        {
            var imgByteArr = await _imageCropper.CropImage("https://photos2.insidercdn.com/iphone4scamera-111004-full.JPG");
            if (imgByteArr != null && imgByteArr.Length > 0)
            {
                Debug.WriteLine($"We got an image with {imgByteArr.Length} Length");
                LoadNewImage(imgByteArr);
            }
            else
            {
                Debug.WriteLine("We got no image");
            }
        }

        private void LoadNewImage(byte[] imgByteArr)
        {
            Stream stream = new MemoryStream(imgByteArr);
            var imgSource = ImageSource.FromStream(() => stream);
            ImageSource = imgSource;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<App, byte[]>(this, "NewCroppedImageResult");
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            MessagingCenter.Subscribe<App, byte[]>(this, "NewCroppedImageResult", (sender, arg) =>
            {
                OnReceiveCroppedImageResult(arg);
            });
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            
        }
    }

    public class NewCroppedImageResult : PubSubEvent<string> { }
}
