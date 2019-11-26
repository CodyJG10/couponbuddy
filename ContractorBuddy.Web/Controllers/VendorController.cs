using BrochureBuddy.Entities;
using BrochureBuddy.Web.Areas.Identity;
using BrochureBuddy.Web.Data;
using BrochureBuddy.Web.Models;
using BrochureBuddy.Web.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrochureBuddy.Web.Controllers
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

        [HttpPost("UploadHome")]
        public async Task<IActionResult> UploadHome(List<IFormFile> files)
        {
            if (files.Count == 0) return Content("Invalid File");
            await _fileManager.UploadFile(files[0], "home", _vendor);

            //var username = User.Identity.Name;
            //var vendor = _context.Vendors.Single(x => x.Username == username);
            //var userId = _userManager.FindByNameAsync(username).GetAwaiter().GetResult().Id;

            //if (_context.Ads.SingleOrDefault(x => x.VendorId == vendor.Id) != null)
            //{
            //    _context.Remove(_context.Ads.Single(x => x.VendorId == vendor.Id));
            //}

            //Ad ad = new Ad()
            //{
            //    ContentType = "PNG",
            //    FileName = vendor.Name.ToLower().Replace(" ", "") + ":home.png",
            //    VendorId = vendor.Id
            //};

            //vendor.Ad = ad;

            //_context.Update(vendor);
            //_context.SaveChanges();

            return RedirectToAction("ManageContent");
        }

        [HttpPost("UploadMedia")]
        public async Task<IActionResult> UploadMedia(List<IFormFile> files)
        {
            if (files.Count != 1) return Content("Invalid File");
            IFormFile file = files[0];
            Guid guid = Guid.NewGuid();
            await _fileManager.UploadFile(file, guid.ToString(), _vendor);
            VendorMedia media = new VendorMedia()
            {
                Guid = guid,
                VendorId = _vendor.Id
            };
            if (_vendor.VendorMedia == null) _vendor.VendorMedia = new List<VendorMedia>();
            _context.VendorMedia.Add(media);
            _vendor.VendorMedia.Add(media);

            _context.SaveChanges();

            return RedirectToAction("ManageContent");
        }

        [HttpGet("DeleteCoverImage/{mediaId}")]
        public IActionResult DeleteCoverImage([FromRoute]int mediaId)
        {
            var vendorMedia = _context.VendorMedia.Where(x => x.VendorId == _vendor.Id);
            var media = vendorMedia.ToList()[mediaId];
            _context.Remove(media);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

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