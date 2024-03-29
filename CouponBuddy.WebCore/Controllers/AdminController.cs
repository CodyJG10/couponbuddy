﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CouponBuddy.Web.Data;
using Microsoft.AspNetCore.Identity;
using CouponBuddy.Web.Models;
using CouponBuddy.Web.Areas.Identity;
using static CouponBuddy.Web.Areas.Identity.User;
using Newtonsoft.Json;
using CouponBuddy.Entities;
using CouponBuddy.WebCore.PageUtil;
using Microsoft.AspNetCore.Http;
using CouponBuddy.Web.Storage;
using CouponBuddy.WebCore.Storage;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using CouponBuddy.WebCore.Models;

namespace CouponBuddy.Web.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        #region Members
        private readonly ApplicationDbContext _context;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private FileManager _fileManager;
        private ImageLoader _imageLoader;
        #endregion

        public AdminController(ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            FileManager fileManager,
            ImageLoader imageLoader)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _fileManager = fileManager;
            _imageLoader = imageLoader;
        }

        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vendors.ToListAsync());
        }

        #region Roles

        [HttpGet("AddUserToRole")]
        public IActionResult ViewAddUserToRole()
        {
            return View("AddUserToRole");
        }

        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return BadRequest(404);
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                return BadRequest(404);
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (result.Succeeded)
            {
                return Content("Succesfully added user to role. You may now leave this page.");
            }
            else
            {
                string errors = "";
                foreach (var error in result.Errors)
                {
                    errors += error.Description + "\n";
                }
                return Content("Error:\n" + errors);
            }
        }

        [HttpGet("CreateRole")]
        public IActionResult ViewCreateRole()
        {
            return View("CreateRole");
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
            if (result.Succeeded)
            {
                return Content("Succesfully created role.");
            }
            else
            {
                return BadRequest(400);
            }
        }

        #endregion

        #region Vendor

        [HttpGet("CreateVendor")]
        public IActionResult ViewCreateVendor()
        {
            return View("CreateVendor");
        }

        [HttpPost("CreateVendor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Address,Phone,CategoryId,Username,Website")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                //vendor.HomePageAdId = -1;
                _context.Add(vendor);
                var user = await _userManager.FindByNameAsync(vendor.Username);
                await _userManager.AddToRoleAsync(user, "Vendor");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendor);
        }

        [HttpGet("VendorsList")]
        public IActionResult VendorsList(string sortOrder,
                                         string searchString,
                                         string currentFilter,
                                         int? pageNumber)
        {
            ViewData["CategorySortParm"] = sortOrder == "Category" ? "cat_desc" : "Category";
            ViewData["LocationSortParm"] = sortOrder == "Location" ? "location_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var vendors = _context.Vendors.ToList().AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                vendors = vendors.Where(v => v.GetLocations().Count > 0
                     && _context.Locations.ToList()
                         .Where(x => x.Id == v.GetLocations()[0])
                         .SingleOrDefault().Name.ToLower()
                         .Contains(searchString.ToLower())
                     || Categories.GetCategory(v.CategoryId).DisplayName.Contains(searchString)
                     || v.Name.ToLower().Contains(searchString.ToLower())
                     || v.Username.ToLower().Contains(searchString.ToLower()))
                    .AsQueryable();
            }
            switch (sortOrder)
            {
                case "cat_desc":
                    vendors = vendors.OrderByDescending(v => v.CategoryId);
                    break;
                case "Category":
                    vendors = vendors.OrderBy(v => v.CategoryId);
                    break;
                case "location_desc":
                    vendors = vendors.AsEnumerable().OrderByDescending(x => x.GetLocations()[0].Contains(searchString)).AsQueryable();
                    break;
                default:
                    vendors = vendors.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 10;
            return View(PaginatedList<Vendor>.CreateAsync(vendors.ToList(), pageNumber ?? 1, pageSize));
        }

        [HttpGet("AddVendorToLocation")]
        public IActionResult ViewAddVendorToLocation()
        {
            Dictionary<string, string> locations = new Dictionary<string, string>();
            foreach (var x in _context.Locations.ToList())
            {
                locations.Add(x.Id, x.Name);
            }
            return View("AddVendorToLocation", new AddVendorToLocationViewModel() { Locations = locations });
        }

        [HttpPost("AddVendorToLocation")]
        public async Task<IActionResult> AddVendorToLocation(AddVendorToLocationViewModel model)
        {
            //Find Vendor
            Vendor vendor = _context.Vendors.SingleOrDefault(x => x.Username == model.Username);
            var user = await _userManager.FindByNameAsync(model.Username);

            //Find Location
            Location location = _context.Locations.SingleOrDefault(x => x.Id == model.LocationId);

            //Failsafes
            if (user == null || location == null)
            {
                return Content("Invalid request. Please confirm username and location ID are valid.");
            }

            //Add vendor to location
            LocationReference locationRef = new LocationReference()
            {
                LocationId = model.LocationId,
                UserId = user.Id,
            };

            if (user.UserLocations == null) user.UserLocations = new List<LocationReference>();
            user.UserLocations.Add(locationRef);

            var vendorLocations = vendor.GetLocations();
            vendorLocations.Add(model.LocationId);

            vendor.LocationsJson = JsonConvert.SerializeObject(vendorLocations);

            await _context.SaveChangesAsync();
            await _userManager.UpdateAsync(user);

            //Return
            return View("Index");
        }

        [HttpGet("VendorDetails/{id}")]
        public IActionResult VendorDetails([FromRoute]int id)
        {
            var vendor = _context.Vendors.SingleOrDefault(x => x.Id == id);
            return View(vendor);
        }

        #region Manage Vendor Content

        [HttpGet("ManageVendorContent/{id}")]
        public IActionResult ViewManageVendorContent([FromRoute] int id)
        {
            TempData["ID"] = id;
            var vendor = _context.Vendors.Single(x => x.Id == id);
            return View("ManageVendorContent", new ManageContentViewModel(vendor, _context, _imageLoader));
        }

        [HttpPost("UploadLogo/{id}")]
        public async Task<ActionResult> UploadLogo([FromRoute]int id, List<IFormFile> files)
        {
            var vendor = _context.Vendors.Single(x => x.Id == id);
            if (files.Count == 0) return Content("Invalid File");
            await _fileManager.UploadFile(files[0], "logo", vendor);
            return RedirectToAction("ManageVendorContent", new { id = id });
        }

        [HttpPost("UploadInactive/{id}")]
        public async Task<IActionResult> UploadInactive([FromRoute]int id, List<IFormFile> files)
        {
            var vendor = _context.Vendors.Single(x => x.Id == id);
            if (files.Count == 0) return Content("Invalid File");
            await _fileManager.UploadFile(files[0], "inactive", vendor);
            return RedirectToAction("ManageVendorContent", new { id = id });
        }

        [HttpPost("UploadHome/{id}")]
        public async Task<IActionResult> UploadHome([FromRoute] int id, List<IFormFile> files)
        {
            var vendor = _context.Vendors.Single(x => x.Id == id);
            if (files.Count == 0) return Content("Invalid File");
            await _fileManager.UploadFile(files[0], "home", vendor);
            return RedirectToAction("ManageVendorContent", new { id = id });
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
            int id = (int)TempData["ID"];
            TempData.Keep();
            var vendor = _context.Vendors.Single(x => x.Id == id);
            var coupons = _context.VendorCoupons.Where(x => x.VendorId == vendor.Id);
            return View("ViewCoupons", coupons);
        }

        [HttpPost("CreateCoupon")]
        public async Task<IActionResult> CreateCoupon(VendorCoupon coupon)
        {
            coupon.IsActive = true;
            int id = (int)TempData["ID"];
            TempData.Keep();
            coupon.VendorId = id;
            _context.VendorCoupons.Add(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction("VendorsList");
        }

        [HttpPost("EditCoupon")]
        public async Task<IActionResult> EditCoupon(VendorCoupon coupon)
        {
            _context.Update(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction("VendorsList");
        }

        [HttpPost("DeleteCoupon")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var coupon = _context.VendorCoupons.Single(x => x.Id == id);
            _context.Remove(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction("VendorsList");
        }

        #endregion

        #endregion

        [HttpGet("DeleteVendor/{id}")]
        public IActionResult ViewDeleteVendor([FromRoute]int id)
        {
            var vendor = _context.Vendors.Single(x => x.Id == id);
            return View("DeleteVendor", vendor);
        }

        [HttpPost("DeleteVendor/{id}")]
        public IActionResult DeleteVendor([FromRoute]int id)
        {
            var vendor = _context.Vendors.Single(x => x.Id == id);
            _context.Vendors.Remove(vendor);

            //foreach (var ad in _context.Ads.Where(x => x.VendorId == id).ToList())
            //{
            //    _context.Ads.Remove(ad);
            //}

            foreach (var userLocation in _context.UserLocations.Where(x => x.UserId == vendor.Username))
            {
                _context.UserLocations.Remove(userLocation);
            }

            _context.SaveChanges();

            return View("Index");
        }

        [HttpGet("EditVendor/{id}")]
        public IActionResult ViewEditVendor([FromRoute]int id)
        {
            var vendor = _context.Vendors.Single(x => x.Id == id);
            return View("EditVendor", vendor);
        }

        [HttpPost("EditVendor")]
        public IActionResult EditVendor(Vendor vendor)
        {
            _context.Update(vendor);
            _context.SaveChanges();
            return View("Index");
        }
        #endregion

        #region Locations

        [HttpGet("ViewLocations")]
        public IActionResult ViewLocations()
        {
            var locations = _context.Locations.ToList();
            return View("LocationsIndex", locations);
        }

        [HttpGet("ViewLocation")]
        public IActionResult ViewLocation(string id)
        {
            var location = _context.Locations.SingleOrDefault(x => x.Id == id);
            //var vendors = _context.Vendors.Where(x => x.GetLocations().Contains(id));
            return View("Location", location);
        }

        [HttpGet("ViewCreateLocation")]
        public IActionResult ViewCreateLocation()
        {
            return View("CreateLocation");
        }

        [HttpPost("CreateLocation")]
        public IActionResult CreateLocation(Location location)
        {
            location.Id = Guid.NewGuid().ToString();
            _context.Locations.Add(location);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("ViewEditLocation")]
        public IActionResult ViewEditLocation(string id)
        {
            var location = _context.Locations.SingleOrDefault(x => x.Id == id);
            return View("EditLocation", location);
        }

        [HttpPost("EditLocation")]
        public IActionResult EditLocation(Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Update(location);
                _context.SaveChanges();
                return View("ViewLocations");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("ViewDeleteLocation")]
        public IActionResult ViewDeleteLocation(string id)
        {
            var location = _context.Locations.SingleOrDefault(x => x.Id == id);
            return View("DeleteLocation", location);
        }

        [HttpPost("DeleteLocation")]
        public IActionResult DeleteLocation(Location location)
        {
            _context.Remove(location);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Location Manager

        [HttpGet("CreateLocationManager")]
        public IActionResult ViewCreateCreateLocationManager()
        {
            Dictionary<string, string> locations = new Dictionary<string, string>();
            foreach (var x in _context.Locations.ToList())
            {
                locations.Add(x.Id, x.Name);
            }
            return View("CreateLocationManager", locations);
        }

        [HttpPost("CreateLocationManager")]
        public async Task<IActionResult> CreateLocationManager(string username, string locationId)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                if (user.UserLocations == null) user.UserLocations = new List<LocationReference>();
                List<LocationReference> locations = new List<LocationReference>();
                LocationReference locationReference = new LocationReference()
                {
                    LocationId = locationId
                };
                locations.Add(locationReference);
                user.UserLocations = locations;
                await _userManager.AddToRoleAsync(user, "LocationManager");
                await _userManager.UpdateAsync(user);
                return Content("Succesfully promoted user to location manager of location: " + locationId);
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region Analytics
        [Route("CouponAnalytics")]
        public IActionResult CouponAnalytics(string locationId)
        {
            CouponAnalyticsChart model = new CouponAnalyticsChart();

            var location = _context.Locations.Single(x => x.Id == locationId);

            model.Location = location;

            //var vendorsAtLocation = _context.Vendors.AsEnumerable().Where(x => x.GetLocations().Contains(location.Id));
            //var allAnalytics = _context.VendorAnalytics.AsNoTracking().AsEnumerable();

            ////Day, coupons sent
            //Dictionary<int, int> couponsSent = new Dictionary<int, int>();

            //for (int i = 0; i < DateTime.Now.Day; i++)
            //{
            //    couponsSent.Add(i, 0);
            //}

            //foreach (var vendor in vendorsAtLocation)
            //{
            //    var analytics = allAnalytics.SingleOrDefault(x => x.VendorId == vendor.Id.ToString());
            //    if (analytics == null)
            //        continue;
            //    var couponsSentThisMonth = analytics.GetCurrentMonthCouponsSent();

            //    foreach (var date in couponsSentThisMonth)
            //    {
            //        couponsSent[date.Day] += 1;
            //    }
            //}

            //for (int i = 0; i < couponsSent.Count; i++)
            //{
            //    var day = couponsSent.Keys.ToList()[i];
            //    int value = couponsSent.Values.ToList()[i];
            //    var valueModel = new CouponAnalyticsChart.KeyValue()
            //    {
            //        Title = DateTime.Now.AddDays((-DateTime.Now.Day) + day).ToString(),
            //        Value = value
            //    };
            //    model.Values.Add(valueModel);
            //}
            //return View(model);
            return View(model);
        }

        //public string GetCouponsSentThisMonth(string locationId)
        //{
        //    CouponAnalyticsChart model = new CouponAnalyticsChart();

        //    var location = _context.Locations.Single(x => x.Id == locationId);

        //    model.Location = location;

        //    var vendorsAtLocation = _context.Vendors.AsEnumerable().Where(x => x.GetLocations().Contains(location.Id));
        //    var allAnalytics = _context.VendorAnalytics.AsNoTracking().AsEnumerable();

        //    Dictionary<int, int> values = new Dictionary<int, int>();

        //    for (int i = 1; i < DateTime.Now.Day; i++)
        //    {
        //        values.Add(i, 0);
        //    }

        //    foreach (var vendor in vendorsAtLocation)
        //    {
        //        var vendorAnalytics = _context.VendorAnalytics.AsEnumerable().Single(x => x.VendorId == vendor.Id.ToString());
        //        var couponsSentThisMonthFromVendor = vendorAnalytics.GetCurrentMonthCouponsSent();
        //        foreach (var date in couponsSentThisMonthFromVendor)
        //        {
        //            var day = date.Day;
        //            values[day]++;
        //        }
        //    }

        //    return JsonConvert.SerializeObject(values);
        //}
        #endregion
    }
}