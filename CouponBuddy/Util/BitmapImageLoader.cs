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
    public class BitmapImageLoader : ImageLoader
    {
        public async Task<ImageSource> LoadImageSource(string container, string fileName)
        {
            var bytes = await LoadImage(container, fileName);

            if (bytes == null) return null;

            BitmapImage img = new BitmapImage
            {
                CacheOption = BitmapCacheOption.OnLoad
            };

            MemoryStream ms = new MemoryStream(bytes);
            img.BeginInit();
            img.StreamSource = ms;
            img.EndInit();
            img.Freeze();

            ImageSource imgSrc = img as ImageSource;

            return imgSrc;
        }

        public async Task<ImageSource> LoadImageSource(Vendor vendor, string fileName)
        {
            string container = VendorToContainer.GetContainerName(vendor);
            var bytes = await LoadImage(container, fileName);

            BitmapImage img = new BitmapImage
            {
                CacheOption = BitmapCacheOption.OnLoad
            };

            MemoryStream ms = new MemoryStream(bytes);
            img.BeginInit();
            img.StreamSource = ms;
            img.EndInit();
            img.Freeze();

            ImageSource imgSrc = img as ImageSource;

            return imgSrc;
        }

        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
