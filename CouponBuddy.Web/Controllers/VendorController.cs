using CouponBuddy.Entities;
using CouponBuddy.Web.Areas.Identity;
using CouponBuddy.Web.Data;
using CouponBuddy.Web.Models;
using CouponBuddy.Web.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        #endregion

        private static Vendor _vendor;

        public VendorController(ApplicationDbContext applicationContext,
                                FileManager fileManager,
                                UserManager<User> userManager)
        {
            _context = applicationContext;
            _fileManager = fileManager;
            _userManager = userManager;
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
            ManageContentViewModel model = new ManageContentViewModel(_vendor, _context);
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
        #endregion
    }
}