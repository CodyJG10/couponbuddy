using CouponBuddy.Entities;
using CouponBuddy.Web.Areas.Identity;
using CouponBuddy.Web.Data;
using CouponBuddy.Web.Models;
using CouponBuddy.Web.Storage;
using CouponBuddy.WebCore.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouponBuddy.Web.Controllers
{
    [Route("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class VendorController : Controller
    {
        #region Memebers
        private ApplicationDbContext _context;
        private FileManager _fileManager;
        private UserManager<User> _userManager;
        private ImageLoader _imageLoader;
        #endregion

        private static Vendor _vendor;

        public VendorController(ApplicationDbContext applicationContext,
                                FileManager fileManager,
                                UserManager<User> userManager,
                                ImageLoader imageLoader)
        {
            _context = applicationContext;
            _fileManager = fileManager;
            _userManager = userManager;
            _imageLoader = imageLoader;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var username = User.Identity.Name;
            _vendor = _context.Vendors.Single(x => x.Username.Equals(username));

            var locationIds = _vendor.GetLocations();
            List<Location> locations = new List<Location>();
            locationIds.ForEach(x =>
            {
                var location = _context.Locations.Single(y => y.Id == x);
                locations.Add(location);
            });

            return View(locations);
        }

        [Route("ManageContent")]
        public IActionResult ManageContent()
        {
            ManageContentViewModel model = new ManageContentViewModel(_vendor, _context, _imageLoader);
            return View(model);
        }

        #region Upload Functions

        [HttpPost("UploadLogo")]
        public async Task<ActionResult> UploadLogo(List<IFormFile> files)
        {
            if (files.Count == 0) return Content("Invalid File");
            await _fileManager.UploadFile(files[0], "logo", _vendor);
            return RedirectToAction("ManageContent");
        }

        [HttpPost("UploadInactive")]
        public async Task<IActionResult> UploadInactive(List<IFormFile> files)
        {
            if (files.Count == 0) return Content("Invalid File");
            await _fileManager.UploadFile(files[0], "inactive", _vendor);
            return RedirectToAction("ManageContent");
        }
        #region Coupons

        [HttpGet("CreateCoupon")]
        public IActionResult ViewCreateCoupon()
        {
            return View("CreateCoupon");
        }

        [HttpGet("DeleteCoupon")]
        public IActionResult ViewDeleteCoupon(int id)
        {
            var coupon = _context.VendorCoupons.Single(x => x.Id == id);
            return View("DeleteCoupon", coupon);
        }

        [HttpGet("EditCoupon")]
        public IActionResult ViewEditCoupon(int id)
        {
            var coupon = _context.VendorCoupons.Single(x => x.Id == id);
            return View("EditCoupon", coupon);
        }

        [HttpGet("Coupons")]
        public IActionResult ViewCoupons()
        {
            var coupons = _context.VendorCoupons.Where(x => x.VendorId == _vendor.Id);
            return View("ViewCoupons", coupons);
        }

        [HttpPost("CreateCoupon")]
        public async Task<IActionResult> CreateCoupon(VendorCoupon coupon)
        {

            coupon.IsActive = true;
            coupon.VendorId = _vendor.Id;
            _context.VendorCoupons.Add(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageContent");
        }
        
        [HttpPost("EditCoupon")]
        public async Task<IActionResult> EditCoupon(VendorCoupon coupon)
        {
            _context.Update(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageContent");
        }

        [HttpPost("DeleteCoupon")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var coupon = _context.VendorCoupons.Single(x => x.Id == id);
            _context.Remove(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageContent");
        }

        #endregion

        #endregion

        #region Edit Functions

        [HttpGet("Edit")]
        public IActionResult ViewEdit()
        {
            string username = User.Identity.Name;
            Vendor vendor = _context.Vendors.Single(x => x.Username == username);
            return View("Edit", vendor);
        }

        [HttpPost("Edit")]
        public IActionResult Edit(Vendor vendor)
        {
            _context.Update(vendor);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        //#region Analytics Functions
        //[HttpGet("Analytics")]
        //public IActionResult ViewAnalytics()
        //{
        //    var locationIds = _vendor.GetLocations();
        //    List<Location> locations = new List<Location>();
        //    locationIds.ForEach(x =>
        //    {
        //        var location = _context.Locations.Single(y => y.Id == x);
        //        locations.Add(location);
        //    });

        //    return View("Analytics", locations);
        //}

        //[HttpGet("LocationAnalytics")]
        //public IActionResult ViewDetailedAnalytics(string locationId)
        //{
        //    var location = _context.Locations.Single(x => x.Id == locationId);
        //    return View("DetailedAnalytics", location);
        //}
        //#endregion

        #region Analytics Functions
        [HttpGet("Analytics")]
        public IActionResult ViewAnalytics()
        {
            var locationIds = _vendor.GetLocations();
            List<Location> locations = new List<Location>();
            locationIds.ForEach(x =>
            {
                var location = _context.Locations.Single(y => y.Id == x);
                locations.Add(location);
            });

            return View("Analytics", locations);
        }

        [HttpGet("LocationAnalytics")]
        public IActionResult ViewDetailedAnalytics(string locationId)
        {
            var location = _context.Locations.Single(x => x.Id == locationId);
            return View("DetailedAnalytics", location);
        }

        [HttpGet("GetChartData")]
        public string GetChartData(int type, string locationId)
        {
            var currentUsername = User.Identity.Name;
            var vendor = _context.Vendors.Single(x => x.Username == currentUsername);
            var analytics = _context.VendorAnalytics.Single(x => x.VendorId == vendor.Id.ToString() && x.LocationId == locationId);

            List<dynamic> results = new List<dynamic>();

            void CacluateAnalytics(DateTime minimumDate)
            {
                var impressions = analytics.GetImpressions();
                var clicks = analytics.GetClicks();
                var maxDate = DateTime.Now;
                int totalDays = (int)Math.Round((maxDate - minimumDate).TotalDays);
                for (int i = 0; i <= totalDays; i++)
                {
                    var day = minimumDate.AddDays(i);
                    int totalImpressionsForDay = impressions.Where(x => x.Day == day.Day && x.Month == day.Month && x.Year == day.Year).Count();
                    int totalClicksForDay = clicks.Where(x => x.Day == day.Day && x.Month == day.Month && x.Year == day.Year).Count();
                    var resultObject = new
                    {
                        Day = day,
                        Impressions = totalImpressionsForDay,
                        Clicks = totalClicksForDay
                    };
                    results.Add(resultObject);
                }
            }

            if (type == 1)
            {
                CacluateAnalytics(DateTime.Now.AddMonths(-1));
            }
            else if (type == 2)
            {
                CacluateAnalytics(DateTime.Now.AddMonths(-3));
            }
            else if (type == 3)
            {
                CacluateAnalytics(DateTime.Now.AddYears(-1));
            }
            else if (type == 4)
            {
                var earliestDate = DateTime.Now;
                foreach (var impression in analytics.GetImpressions())
                {
                    if (DateTime.Compare(impression, earliestDate) < 0) earliestDate = impression;
                }
                CacluateAnalytics(earliestDate);
            }

            return JsonConvert.SerializeObject(results);
        }

        #endregion
    }
}