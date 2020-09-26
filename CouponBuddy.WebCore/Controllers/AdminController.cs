using System;
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

namespace CouponBuddy.Web.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        #region Members
        private readonly ApplicationDbContext _context;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        #endregion

        public AdminController(ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
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
        public IActionResult VendorsList()
        {
            var vendors = _context.Vendors.ToList();
            return View(vendors);
        }

        [HttpGet("AddVendorToLocation")]
        public IActionResult ViewAddVendorToLocation()
        {
            return View("AddVendorToLocation");
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
                UserId = user.Id
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
            return View("CreateLocationManager");
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
    }
}