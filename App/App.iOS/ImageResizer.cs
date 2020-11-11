using System.Linq;
using App.iOS;
using Xamarin.Auth;
using Xamarin.Forms;
using App.Interfaces;
using System.IO;
using UIKit;
using System;
using CoreGraphics;
using System.Drawing;

[assembly: Dependency(typeof(ImageResizer))]
namespace App.iOS
{    
    public class ImageResizer:IImageResizer
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {

            UIImage originalImage = ImageFromByteArray(imageData);

          

            var originalHeight = originalImage.Size.Height;
            var originalWidth = originalImage.Size.Width;

            nfloat newHeight = 0;
            nfloat newWidth = 0;

            if (originalHeight > originalWidth)
            {
                newHeight = height;
                nfloat ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                nfloat ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            width = (float)newWidth;
            height = (float)newHeight;

            UIGraphics.BeginImageContext(new SizeF(width, height));
            originalImage.Draw(new RectangleF(0, 0, width, height));
            var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            var bytesImagen = resizedImage.AsJPEG().ToArray();
            resizedImage.Dispose();
            return bytesImagen;
        }

        private UIImage ImageFromByteArray(byte[] imageData)
        {
            throw new NotImplementedException();
        }
    }
}
