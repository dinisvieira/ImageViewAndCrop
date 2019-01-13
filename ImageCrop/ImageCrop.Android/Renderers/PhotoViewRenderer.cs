using System.ComponentModel;
using System.Net;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using ImageCrop.CustomControls;
using ImageCrop.Droid.Renderers;
using ImageViews.Photo;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ARelativeLayout = Android.Widget.RelativeLayout;

[assembly: ExportRenderer(typeof(NativePhotoView), typeof(PhotoViewRenderer))]

namespace ImageCrop.Droid.Renderers
{
    public class PhotoViewRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<NativePhotoView, ARelativeLayout>
    {

        private PhotoView photoView;

        public PhotoViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<NativePhotoView> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                if (Control == null)
                {
                    // Save the VideoView for future reference
                    photoView = new PhotoView(Context);

                    // Put the VideoView in a RelativeLayout
                    ARelativeLayout relativeLayout = new ARelativeLayout(Context);
                    relativeLayout.AddView(photoView);

                    // Center the VideoView in the RelativeLayout
                    ARelativeLayout.LayoutParams layoutParams = new ARelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                    layoutParams.AddRule(LayoutRules.CenterInParent);
                    photoView.LayoutParameters = layoutParams;
                    photoView.Zoomable = true;

                    if (!string.IsNullOrWhiteSpace(args.NewElement.ImagePath))
                    {
                        var imagePath = args.NewElement.ImagePath;
                        var imgBitmap = GetImageBitmapFromUrl(imagePath);
                        photoView.SetImageBitmap(imgBitmap);
                    }

                    // Use the RelativeLayout as the native control
                    SetNativeControl(relativeLayout);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Element == null || Control == null) { return; }

            if (e.PropertyName == nameof(Element.ImagePath))
            {
                if (photoView != null)
                {
                    var imagePath = Element.ImagePath;
                    var imgBitmap = GetImageBitmapFromUrl(imagePath);
                    photoView.SetImageBitmap(imgBitmap);
                }
            }
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                photoView?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}