using System.Linq;
using Xamarin.Auth;
using Xamarin.Forms;
using App.Interfaces;
using App.Droid;
using System.Collections.Generic;
using System.IO;
using Android.Graphics;
using Java.Lang;
using Android.Util;


[assembly: Dependency(typeof(ImageResizer))]
namespace App.Droid
{
    public class ImageResizer:IImageResizer
    {        
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            // Load the bitmap 
            BitmapFactory.Options options = new BitmapFactory.Options();// Create object of bitmapfactory's option method for further option use
            options.InPurgeable = true; // inPurgeable is used to free up memory while required
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);

            float newHeight = 0;
            float newWidth = 0;

            var originalHeight = originalImage.Height;
            var originalWidth = originalImage.Width;

            if (originalHeight > originalWidth)
            {
                newHeight = height;
                float ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                float ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, true);

            originalImage.Recycle();

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);

                resizedImage.Recycle();

                return ms.ToArray();
            }

        }
        //public byte[] ResizeImage(byte[] imageData, float width, float height)
        //{
        //    // Load the bitmap 
        //    Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
        //    //
        //    float ZielHoehe = 0;
        //    float ZielBreite = 0;
        //    //
        //    var Hoehe = originalImage.Height;
        //    var Breite = originalImage.Width;
        //    //
        //    if (Hoehe > Breite) // Höhe (71 für Avatar) ist Master
        //    {
        //        ZielHoehe = height;
        //        float teiler = Hoehe / height;
        //        ZielBreite = Breite / teiler;
        //    }
        //    else // Breite (61 für Avatar) ist Master
        //    {
        //        ZielBreite = width;
        //        float teiler = Breite / width;
        //        ZielHoehe = Hoehe / teiler;
        //    }
        //    //
        //    Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)ZielBreite, (int)ZielHoehe, false);
        //    // 
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
        //        return ms.ToArray();
        //    }
        //}
    }
}