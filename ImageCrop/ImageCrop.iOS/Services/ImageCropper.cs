using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using ImageCrop.Services;
using UIKit;
using Wapps.TOCrop;
using Xamarin.Essentials;

namespace ImageCrop.iOS.Services
{
    public class ImageCropper : IImageCropper
    {
        public Task<byte[]> CropImage(string path)
        {
            try
            {
                return Task.Run(() =>
                {
                    UIImage image;
                    //This code is for loading images from web, for local files it's easier UIImage.FromFile()
                    using (var url = new NSUrl(path))
                    {
                        using (var data = NSData.FromUrl(url))
                        {
                            image = UIImage.LoadFromData(data);
                        }
                    }

                    var cropVCDelegate = new CropVCDelegate();
                    
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        var cropVC = new TOCropViewController(TOCropViewCroppingStyle.Circular, image);

                        cropVC.Toolbar.CancelTextButton.SetTitle("Cancelar", UIControlState.Focused);
                        cropVC.Toolbar.CancelTextButton.SetTitle("Cancelar", UIControlState.Normal);
                        cropVC.Toolbar.CancelTextButton.SetTitle("Cancelar", UIControlState.Selected);
                        cropVC.Toolbar.CancelTextButton.SetTitle("Cancelar", UIControlState.Highlighted);
                        cropVC.Toolbar.CancelTextButton.SetTitle("Cancelar", UIControlState.Disabled);

                        cropVC.Toolbar.DoneTextButton.SetTitle("Ok", UIControlState.Focused);
                        cropVC.Toolbar.DoneTextButton.SetTitle("Ok", UIControlState.Normal);
                        cropVC.Toolbar.DoneTextButton.SetTitle("Ok", UIControlState.Selected);
                        cropVC.Toolbar.DoneTextButton.SetTitle("Ok", UIControlState.Highlighted);
                        cropVC.Toolbar.DoneTextButton.SetTitle("Ok", UIControlState.Disabled);

                        cropVC.Delegate = cropVCDelegate;

                        var window = UIApplication.SharedApplication.KeyWindow;
                        window.RootViewController.PresentViewController(cropVC, true, null);
                    });

                    while (!cropVCDelegate.Finished)
                    {
                        // lock up thread until cropping has finished
                    }

                    return cropVCDelegate.ImageBytes;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public class CropVCDelegate : TOCropViewControllerDelegate
        {
            public bool Finished = false;
            public byte[] ImageBytes;

            //public override void DidCropImageToRect(TOCropViewController cropViewController, CGRect cropRect, nint angle)
            //{
            //    try
            //    {
            //        cropViewController.PresentingViewController.DismissViewController(true, null);
            //        var finalImage = cropViewController.FinalImage;
            //        ImageBytes = finalImage.AsJPEG().ToArray();
            //    }
            //    finally
            //    {
            //        Finished = true;
            //    }
            //}

            public override void DidCropToCircularImage(TOCropViewController cropViewController, UIImage image, CGRect cropRect, nint angle)
            {
                try
                {
                    cropViewController.PresentingViewController.DismissViewController(true, null);
                    var finalImage = cropViewController.FinalImage;

                    var resizedImage = MaxResizeImage(finalImage, 300, 300);
                    ImageBytes = resizedImage.AsJPEG().ToArray();
                }
                finally
                {
                    Finished = true;
                }
            }

            public static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
            {
                var sourceSize = sourceImage.Size;
                var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
                if (maxResizeFactor > 1) return sourceImage;
                var width = maxResizeFactor * sourceSize.Width;
                var height = maxResizeFactor * sourceSize.Height;
                UIGraphics.BeginImageContext(new CGSize((nfloat)width, (nfloat)height));
                sourceImage.Draw(new CGRect(0, 0, (nfloat)width, (nfloat)height));
                var resultImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                return resultImage;
            }

            //public override void DidCropToImage(TOCropViewController cropViewController, UIImage image, CGRect cropRect, nint angle)
            //{
            //    Debug.WriteLine("DidCropToImage called");
            //}

            public override void DidFinishCancelled(TOCropViewController cropViewController, bool cancelled)
            {
                try
                {
                    cropViewController.PresentingViewController.DismissViewController(true, null);
                }
                finally
                {
                    Finished = true;
                }
            }
        }
    }
}
