using System.Threading.Tasks;
using Xamarin.Forms;

namespace ImageCrop
{
    public partial class PhotoViewPage : ContentPage
    {
        public PhotoViewPage()
        {
            InitializeComponent();

            //saveButton.Command = new DelegateCommand(async () =>
            //{
            //    try
            //    {
            //        var result = await cropView.GetImageAsJpegAsync();
            //        byte[] bytes = null;

            //        using (MemoryStream ms = new MemoryStream())
            //        {
            //            result.CopyTo(ms);
            //            bytes = ms.ToArray();
            //        }

            //        var imageSource = ImageSource.FromStream(() =>
            //        {
            //            return new MemoryStream(bytes);
            //        });

            //        ((MainPageViewModel)BindingContext).SavedImage = imageSource;
            //    }
            //    catch (System.Exception ex)
            //    {
            //        await DisplayAlert("Error", ex.Message, "Ok");
            //    }
            //});
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
