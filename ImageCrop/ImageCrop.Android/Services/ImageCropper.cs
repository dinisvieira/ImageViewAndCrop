using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Com.Theartofdev.Edmodo.Cropper;
using ImageCrop.Services;
using Plugin.CurrentActivity;
using Xamarin.Forms;

namespace ImageCrop.Droid.Services
{
    public class ImageCropper : IImageCropper
    {
        public Task<byte[]> CropImage(string path)
        {
            var result = new byte[0];

            var activity = CrossCurrentActivity.Current.Activity;
            Com.Theartofdev.Edmodo.Cropper.CropImage.Builder()
                .SetCropShape(CropImageView.CropShape.Oval)
                .SetFixAspectRatio(true)
                .SetGuidelines(CropImageView.Guidelines.Off)
                .SetActivityTitle("Recortar Imagem")
                .SetCropMenuCropButtonTitle("Ok")
                .SetAllowFlipping(false)
                .SetRequestedSize(300, 300)
                .Start(activity);

            //var intent = new Intent(Forms.Context, typeof(Com.Theartofdev.Edmodo.Cropper.CropImage));
            //Forms.Context.StartActivity(intent);

            //Com.Theartofdev.Edmodo.Cropper.CropImage.

            //var cropImageView = new CropImageView(Context);
            //cropImageView.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            //cropImageView.SetCropShape(CropImageView.CropShape.Oval);
            //cropImageView.SetFixedAspectRatio(true);
            //Bitmap bitmp = BitmapFactory.DecodeByteArray(page.Image, 0, page.Image.Length);
            //cropImageView.SetImageBitmap(bitmp);

            return Task.FromResult(result);
            //setGuidelines(CropImageView.Guidelines.On)
            //.start(this);

            // start picker to get image for cropping and then use the image in cropping activity
            //Com.Theartofdev.Edmodo.Cropper.CropImage.Activity()
            //    .setGuidelines(CropImageView.Guidelines.On)
            //    .start(this);

            // start cropping activity for pre-acquired image saved on the device
            //CropImage.Activity(imageUri)
            //    .start(this);

            // for fragment (DO NOT use `getActivity()`)
            //CropImage.Activity()
            //    .start(getContext(), this);
        }
    }
}