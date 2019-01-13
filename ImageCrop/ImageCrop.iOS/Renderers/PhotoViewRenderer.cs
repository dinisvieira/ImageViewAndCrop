using System;
using System.ComponentModel;
using Foundation;
using ImageCrop.CustomControls;
using ImageCrop.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(NativePhotoView), typeof(PhotoViewRenderer))]

namespace ImageCrop.iOS.Renderers
{
    public class PhotoViewRenderer : ViewRenderer<NativePhotoView, UIView>
    {

        private UIScrollView _scrollViewer;
        private UIImageView _imageView;
        private UIImage _image;

        public UIView ScrollSubView(UIScrollView sv)
        {
            return sv.Subviews[0];
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NativePhotoView> args)
        {
            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                if (Control == null)
                {               
                    
                    var imagePath = args.NewElement.ImagePath;

                    //This code is for loading images from web, for local files it's easier UIImage.FromFile()
                    using (var url = new NSUrl(imagePath))
                    {
                        using (var data = NSData.FromUrl(url))
                        {
                            _image = UIImage.LoadFromData (data);
                        }
                    }

                    _imageView = new UIImageView (_image);
                    _imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                    _imageView.Frame = new CoreGraphics.CGRect (0,0, NativeView.Frame.Width, NativeView.Frame.Height);

                    _scrollViewer = new UIScrollView();
                    _scrollViewer.Frame = new CoreGraphics.CGRect (0,0, NativeView.Frame.Width, NativeView.Frame.Height);
                    _scrollViewer.MinimumZoomScale = 1f;
                    _scrollViewer.MaximumZoomScale = 5f;
                    _scrollViewer.BouncesZoom = true;
                    _scrollViewer.SetZoomScale(1, false);
                    _scrollViewer.ShowsHorizontalScrollIndicator = false;
                    _scrollViewer.ShowsVerticalScrollIndicator = false;            
                    _scrollViewer.AddSubview(_imageView);

                    //Double tap (default zoom)
                    UITapGestureRecognizer tap = new UITapGestureRecognizer(() =>
                    {
                        _scrollViewer.SetZoomScale(0, true);
                    });
                    tap.NumberOfTapsRequired = 2;
                    if (_scrollViewer.Subviews.Length > 0 && _scrollViewer.Subviews[0] != null)
                    {
                        if (_scrollViewer.Subviews[0].GestureRecognizers == null)
                        {
                            _scrollViewer.Subviews[0].AddGestureRecognizer(tap);
                        }
                    }

                    //Keep events from stacking up
                    _scrollViewer.ViewForZoomingInScrollView = ScrollSubView;

                    SetNativeControl(_scrollViewer);
                }
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            //This method is where we need to adjust the imageView and scrollview Frames depending on the Aspect and Size we want to show the image with
            //Current code mantains image in a AspectFit and the Scrollview is sized to have the same ratio as the image

            var imgWidth = _image.Size.Width;
            var imgHeight = _image.Size.Height;

            double aspectWidth = Bounds.Width / imgWidth;
            double aspectHeight = Bounds.Height / imgHeight;
            double aspectRatio = Math.Min(aspectWidth, aspectHeight); //For AspectFill this is likely to be Math.Max

            var newFrame = Bounds;
            newFrame.Width = (nfloat)(imgWidth * aspectRatio);
            newFrame.Height = (nfloat)(imgHeight * aspectRatio);
            newFrame.X = (Bounds.Width - newFrame.Width) / 2f;
            newFrame.Y = (Bounds.Height - newFrame.Height) / 2f;
      
            _scrollViewer.Frame = newFrame; //For AspectFill scrollviewer probably should fill the entire screen of parent (Bounds size)

            var imgNewFrame = newFrame;
            imgNewFrame.X = 0;
            imgNewFrame.Y = 0;
            _imageView.Frame = imgNewFrame;  
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

            if (args.PropertyName == nameof(Element.ImagePath))
            {
                //Update imageview with new image
            }
        }
    }
}