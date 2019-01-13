using System.Diagnostics;
using System.IO;
using ImageCrop.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace ImageCrop.ViewModels
{
    class PhotoCropPageViewModel : BindableBase, INavigationAware
    {

        private IImageCropper _imageCropper;

        public PhotoCropPageViewModel(IImageCropper imageCropper)
        {
            _imageCropper = imageCropper;

            StartCropCommand = new DelegateCommand(OnStartCropCommandExecuted);
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
                Stream stream = new MemoryStream(imgByteArr);
                var imgSource = ImageSource.FromStream(()=> stream);
                ImageSource = imgSource;
            }
            else
            {
                Debug.WriteLine("We got no image");
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            
        }
    }
}
