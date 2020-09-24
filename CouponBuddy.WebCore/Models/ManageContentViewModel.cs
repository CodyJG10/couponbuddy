using CouponBuddy.Entities;
using CouponBuddy.Util;
using CouponBuddy.Web.Data;
using CouponBuddy.WebCore.Storage;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouponBuddy.Web.Models
{
    public class ManageContentViewModel : ViewModel
    {
        private Vendor _vendor;
        private ApplicationDbContext _context;

        public string LogoImageUrl { get; set; }
        public string InactiveImageUrl { get; set; }
        public string HomePageImageUrl { get; set; }

        public List<string> Media { get; set; } = new List<string>();

        private ImageLoader _imageLoader;

        public ManageContentViewModel(Vendor vendor, ApplicationDbContext context, ImageLoader imgLoader)
        {
            _vendor = vendor;
            _context = context;
            _imageLoader = imgLoader;
            LoadContent();
        }

        private void LoadContent()
        {
            string nameFormatted = VendorToContainer.GetContainerName(_vendor);

            var logoImagebytes = _imageLoader.LoadImage(nameFormatted, "logo").GetAwaiter().GetResult();
            var inactiveImageBytes = _imageLoader.LoadImage(nameFormatted, "inactive").GetAwaiter().GetResult();
            var homePageImageBytes = _imageLoader.LoadImage(nameFormatted, "home").GetAwaiter().GetResult();

            var vendorMedia = _context.VendorMedia.Where(x => x.VendorId == _vendor.Id);
            foreach (var media in vendorMedia)
            {
                var imageBytes = _imageLoader.LoadImage(nameFormatted, media.Guid.ToString()).GetAwaiter().GetResult();
                Media.Add("data:image/png;base64," + Convert.ToBase64String(imageBytes));
            }

            string notFound = "~/images/No-image-found.jpg";

            if (logoImagebytes != null)
                LogoImageUrl = "data:image/png;base64," + Convert.ToBase64String(logoImagebytes);
            else
                LogoImageUrl = notFound;
            if (inactiveImageBytes != null)
                InactiveImageUrl = "data:image;base64," + Convert.ToBase64String(inactiveImageBytes);
            else
                InactiveImageUrl = notFound;
            if (homePageImageBytes != null)
                HomePageImageUrl = "data:image;base64," + Convert.ToBase64String(homePageImageBytes);
            else
                HomePageImageUrl = notFound;
        }
    }
}