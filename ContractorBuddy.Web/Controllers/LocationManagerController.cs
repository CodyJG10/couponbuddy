using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BrochureBuddy.Entities;
using BrochureBuddy.Web.Areas.Identity;
using BrochureBuddy.Web.Data;
using BrochureBuddy.Web.Models;
using BrochureBuddy.Web.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static BrochureBuddy.Web.Areas.Identity.User;

namespace BrochureBuddy.Web.Controllers
{
    [Authorize(Roles = "LocationManager")]
    [Route("LocationManager")]
    public class LocationManagerController : Controller
    {
        #region Members
        private ApplicationDbContext _context;
        private FileManager _fileManager;
        private UserManager<User> _userManager;
        #endregion

        public LocationManagerController(ApplicationDbContext context, FileManager fileManager, UserManager<User> userManager)
        {
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var locationId = await GetLocationId();
            var location = await GetLocation();

            List<Vendor> vendors = new List<Vendor>();
            var locationRefs = _context.UserLocations.Where(x => x.LocationId == locationId);

            locationRefs.ToList().ForEach(x =>
            {
                string userId = x.UserId;
                var aspUser = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
                string username = aspUser.UserName;
                if (username == null) return;
                Vendor vendor = _context.Vendors.SingleOrDefault(y => y.Username == username);
                if (vendor == null) return;
                vendors.Add(vendor);
            });

            LocationManagerIndexViewModel viewModel = new LocationManagerIndexViewModel()
            {
                Location = location,
                Vendors = vendors
            };

            return View(viewModel);
        }

        [Route("Settings")]
        public IActionResult Settings()
        {
            return View(_context.AdSettings.ToList()[0]);
        }

        #region Vendors List

        [HttpGet("VendorDetails/{id}")]
        public IActionResult ViewVendorDetails([FromRoute]int id)
        {
            var vendor = _context.Vendors.Single(x => x.Id == id);
            return View("VendorDetails", vendor);
        }

        [HttpGet("RemoveVendor/{id}")]
        public IActionResult ViewRemoveVendor([FromRoute]int id)
        {
            var vendor = _context.Vendors.Single(x => x.Id == id);
            return View("RemoveVendor", vendor);
        }

        [HttpPost("RemoveVendor/{id}")]
        public async Task<IActionResult> RemoveVendor([FromRoute]int id)
        {
            var vendor = _context.Vendors.Single(x => x.Id == id);
            var locationId = await GetLocationId();
            var locationsList = vendor.GetLocations();
            locationsList.Remove(locationId);
            vendor.UpdateLocations(locationsList);
            var user = await _userManager.FindByNameAsync(vendor.Username);
            var locationRef = _context.UserLocations.Single(x => x.LocationId == locationId && x.UserId == user.Id);
            _context.Remove(locationRef);
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("AddVendor")]
        public IActionResult ViewAddVendor()
        {
            return View("AddVendor");
        }

        [HttpPost("AddVendor")]
        public async Task<IActionResult> AddVendor(LocationManagerAddVendorViewModel viewModel)
        {
            var locationId = await GetLocationId();
            string username = viewModel.Username;
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                if (user.UserLocations == null)
                {
                    user.UserLocations = new List<LocationReference>();
                }

                LocationReference locationRef = new LocationReference()
                {
                    LocationId = locationId,
                    UserId = user.Id
                };

                user.UserLocations.Add(locationRef);

                var vendor = _context.Vendors.Single(x => x.Username == user.UserName);
                var vendorLocations = vendor.GetLocations();
                vendorLocations.Add(locationId);

                vendor.LocationsJson = JsonConvert.SerializeObject(vendorLocations);

                await _context.SaveChangesAsync();
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region Ads

        [HttpGet("ViewAds")]
        public async Task<IActionResult> ViewAds()
        {
            Location location = await GetLocation();
            var ads = _context.LocationAds.Where(x => x.LocationId == location.Id).ToList();
            return View(ads);
        }

        [HttpGet("CreateAd")]
        public IActionResult ViewCreateAd()
        {
            return View("CreateAd");
        }

        [HttpPost("CreateAd")]
        public async Task<IActionResult> CreateAd(List<IFormFile> files)
        {
            var file = files[0];
            Guid guid = Guid.NewGuid();
            var location = await GetLocation();
            await _fileManager.UploadFile(file, guid.ToString(), location.Name);
            LocationAd locationAd = new LocationAd()
            {
                LocationId = location.Id,
                Name = guid.ToString()
            };
            _context.LocationAds.Add(locationAd);

            await _context.SaveChangesAsync();

            List<Vendor> vendors = new List<Vendor>();
            var locationRefs = _context.UserLocations.Where(x => x.LocationId == location.Id);

            LocationManagerIndexViewModel model = new LocationManagerIndexViewModel()
            {
                Location = location,
                Vendors = vendors
            };

            return View("Index", model);
        }

        [HttpGet("DeleteAd/{id}")]
        public IActionResult ViewDeleteAd([FromRoute]int id)
        {
            LocationAd ad = _context.LocationAds.SingleOrDefault(x => x.Id == id);
            return View("DeleteAd", ad);
        }

        [HttpPost("DeleteAd/{id}")]
        public IActionResult DeleteAd([FromRoute]int id)
        {
            LocationAd ad = _context.LocationAds.SingleOrDefault(x => x.Id == id);
            _context.LocationAds.Remove(ad);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Helpers
        private async Task<Location> GetLocation()
        {
            var locationId = await GetLocationId();
            Location location = _context.Locations.SingleOrDefault(x => x.Id == locationId);
            return location;
        }

        private async Task<string> GetLocationId()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            _context.UserLocations.ToArray();
            var location = user.UserLocations.ToArray()[0];
            return location.LocationId;
        }
        #endregion
    }
}