using Azure.Storage.Blobs.Specialized;
using CouponBuddy.Api;
using CouponBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CouponBuddy.Util
{
    public static class BitmapImageLoader
    {
        public static ImageSource ToImageSource(Uri uri)
        {
            BitmapImage img = new BitmapImage(uri);
            img.Freeze();
            return img;
        }

        public static ImageSource FromBitmap(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}