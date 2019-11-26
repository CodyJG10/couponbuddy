using CommonServiceLocator;
using CouponBuddy.Api.Interfaces;
using CouponBuddy.Entities;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CouponBuddy.Controllers
{
    public class VendorController
    {
        private IDatabaseManager _db;
        private readonly Dictionary<int, Vendor> vendors = new Dictionary<int, Vendor>();

        private readonly Dictionary<int, VendorMedia> media = new Dictionary<int, VendorMedia>();

        private static VendorController _instance;
        public static VendorController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VendorController();
                }
                return _instance;
            }
        }
        public static bool isInitialized = false;

        public VendorController()
        {
            _db = ServiceLocator.Current.GetService(typeof(IDatabaseManager)) as IDatabaseManager;
        }

        public struct VendorMedia
        {
            public ImageSource homeImage;
            public ImageSource logoImage;
            public ImageSource inactiveImage;
            public List<ImageSource> VendorMediaObjects;
        }

        #region Initialization

        public async Task<Task> LoadVendors(List<Vendor> vendorsToAdd)
        {
            var vendorMediaObjectsList = await _db.GetVendorMedia(); 
            BitmapImageLoader imgLoader = new BitmapImageLoader();
            foreach (var vendor in vendorsToAdd)
            {
                try { 
                    if (vendors.Keys.ToList().Contains(vendor.Id)) continue;

                    Console.WriteLine("Succesfully loaded vendor data for " + vendor.Name);

                    string containerName = VendorToContainer.GetContainerName(vendor);

                    VendorMedia vendorMedia = new VendorMedia();

                    var vendorOwnedMedia = vendorMediaObjectsList.Where(x => x.VendorId == vendor.Id).ToList();
                    vendorMedia.VendorMediaObjects = new List<ImageSource>();
                    foreach (var media in vendorOwnedMedia)
                    {
                        ImageSource mediaImage = await imgLoader.LoadImageSource(vendor, media.Guid.ToString());
                        vendorMedia.VendorMediaObjects.Add(mediaImage);
                    }

                    if (vendorOwnedMedia.Count == 0) continue;

                    vendor.VendorMedia = vendorOwnedMedia;

                    //var homeImg = await imgLoader.LoadImageSource(containerName, "home");
                    //vendorMedia.homeImage = homeImg;

                    var inactiveImg = await imgLoader.LoadImageSource(containerName, "inactive");
                    vendorMedia.inactiveImage = inactiveImg;

                    var logoImg = await imgLoader.LoadImageSource(containerName, "logo");
                    vendorMedia.logoImage = logoImg;

                    media.Add(vendor.Id, vendorMedia);
                    vendors.Add(vendor.Id, vendor);

                    Console.WriteLine("Succesfully loaded vendor media for vendor " + vendor.Name);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Caught error while attempting to load vendor " + vendor.Name);
                    continue;
                }
            }
            isInitialized = true;
            return Task.CompletedTask;
        }

        #endregion

        #region Reading

        public Vendor GetVendor(int id)
        {
            return vendors[id];
        }

        public VendorMedia GetVendorMedia(int id)
        {
            try
            {
                return media[id];
            }
            catch (Exception)
            {
                return new VendorMedia();
            }
        }

        public List<Vendor> GetAllVendors()
        {
            return vendors.Values.ToList();
        }
        #endregion
    }
}