using System.IO;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.Runtime;
using Android.OS;
using Com.Theartofdev.Edmodo.Cropper;
using ImageCrop.Droid.Services;
using ImageCrop.Services;
using ImageCrop.ViewModels;
using Java.Lang;
using Plugin.CurrentActivity;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Xamarin.Forms;

namespace ImageCrop.Droid
{
    [Activity(Label = "ImageCrop", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public static App _application;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState); // add this line to your code
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
   
            _application = new App(new AndroidInitializer());
            LoadApplication(_application);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            try
            {
                if (requestCode == CropImage.CropImageActivityRequestCode)
                {
                    CropImage.ActivityResult result = CropImage.GetActivityResult(data);
                    if (resultCode == Result.Ok)
                    {
                        if(result?.Uri != null)
                        {
                            string uri = result.Uri.Path;

                            if (File.Exists(uri))
                            {

                                using (var fs = File.Open(uri, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                                {
                                    using (var imgMemStream = new MemoryStream())
                                    {
                                        fs.CopyTo(imgMemStream);
                                        var imgByteArr = imgMemStream.ToArray();

                                        _application?.CreateNewCroppedImageEvent(imgByteArr);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //TODO: REPORT
                        }
                    }
                    else if (resultCode == (Android.App.Result) CropImage.CropImageActivityResultErrorCode)
                    {
                        //TODO: REPORT
                        Exception error = result.Error;
                    }
                }

            }
            catch (Exception ex)
            {
                //TODO: REPORT
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry container)
            {
                container.RegisterSingleton<IImageCropper,ImageCropper>();
            }
        }
    }
}