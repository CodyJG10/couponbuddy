using CommonServiceLocator;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Api.Managers;
using CouponBuddy.Entities;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CouponBuddy.Ads
{
    public class AdManager
    {
        private static AdManager _instance;
        public static AdManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AdManager();
                }
                return _instance;
            }
        }

        private List<ImageSource> adImages = new List<ImageSource>();
        private int currentIndex = 0;

        public async Task<Task> LoadAds()
        {
            IDatabaseManager _db = ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as IDatabaseManager;

            var id = Properties.Resources.LOCATION_ID;
            var locationAds = (await (ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as IDatabaseManager).GetAds(id).ConfigureAwait(true)).ToList();

            BitmapImageLoader imgLoader = new BitmapImageLoader();

            foreach (var ad in locationAds)
            {
                string locationId = Properties.Resources.LOCATION_ID;
                Location location = await _db.GetLocation(locationId).ConfigureAwait(true);
                string container = VendorToContainer.GetContainerName(location.Name);
                string fileName = ad.Name;
                var img = await imgLoader.LoadImageSource(container, fileName);
                adImages.Add(img);
            }

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
            Console.WriteLine("[INDEX]" + currentIndex);
            return adImages[currentIndex];
        }
    }
}