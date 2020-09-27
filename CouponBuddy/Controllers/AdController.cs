using CouponBuddy.Properties;
using CommonServiceLocator;
using CouponBuddy.Api;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Api.Managers;
using CouponBuddy.Entities;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.PropertyGridInternal;
using System.Windows.Media;

namespace CouponBuddy.Controllers
{
    public class AdController
    {
        private static AdController _instance;
        public static AdController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AdController();
                }
                return _instance;
            }
        }

        private List<ImageSource> adImages = new List<ImageSource>();
        private int currentIndex = 0;

        public async Task<Task> LoadAds()
        {
            Console.WriteLine("[Ads] attempting to load location ads");
            IDatabaseManager _db = ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as IDatabaseManager;

            var id = Settings.Default.LOCATION_ID;
            var locationAds = (await (ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as IDatabaseManager).GetAds(id).ConfigureAwait(true)).ToList();
            var imgLoader = ServiceLocator.Current.GetService(typeof(ImageLoader)) as ImageLoader;

            foreach (var ad in locationAds)
            {
                string locationId = Settings.Default.LOCATION_ID;
                Location location = await _db.GetLocation(locationId).ConfigureAwait(true);
                string container = VendorToContainer.GetContainerName(location.Name);
                string fileName = ad.Name;
                var imageUri = await imgLoader.DownloadBlob(container, fileName).ConfigureAwait(true);
                var imageSource = BitmapImageLoader.ToImageSource(imageUri);
                adImages.Add(imageSource);
            }

            Console.WriteLine("[Ads] succesfully loaded all (" + locationAds.Count + ") location ads");
            return Task.CompletedTask;
        }

        public ImageSource GetNextAd()
        {
            if (adImages.Count == 0) return null;
            currentIndex++;
            if (currentIndex >= adImages.Count)
            {
                currentIndex = 0;
            }
            Console.WriteLine("[Ads] current ad index:" + currentIndex);
            return adImages[currentIndex];
        }
    }
}