using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using ImageCrop.Droid.Services;
using ImageCrop.Services;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;

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

        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry container)
            {
                // Register any platform specific implementations
                container.RegisterSingleton<IImageCropper, ImageCropper>();
            }
        }
    }
}