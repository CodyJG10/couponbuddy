using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using CouponBuddy.Entities;
using CouponBuddy.Web.Areas.Identity;
using CouponBuddy.Web.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CouponBuddy.Web.Controllers
{
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Vendors")]
        public IEnumerable<Vendor> GetVendors()
        {
            return _context.Vendors.ToList();
        }

        [HttpGet("Ads")]
        public IEnumerable<LocationAd> GetAds()
        {
            return _context.LocationAds.ToList();
        }

        [HttpGet("Ads/{id}")]
        public IEnumerable<LocationAd> GetAds([FromRoute]string id)
        {
            var ads = _context.LocationAds.ToList();
            return ads.Where(x => x.LocationId == id);
        }

        [HttpGet("Vendors/{id}")]
        public IEnumerable<Vendor> GetVendors([FromRoute]string id)
        {
            var vendors = _context.Vendors.ToList();
            List<Vendor> vendorsAtLocation = new List<Vendor>();
            foreach (var vendor in vendors)
            {
                var locations = vendor.GetLocations();
                if (locations.Contains(id))
                {
                    vendorsAtLocation.Add(vendor);
                }
            }
            return vendorsAtLocation;
        }

        [HttpGet("Coupons/{id}")]
        public IEnumerable<VendorCoupon> GetCoupons([FromRoute]int id)
        {
            var coupons = _context.VendorCoupons.Where(x => x.VendorId == id);
            return coupons;
        }

        [HttpGet("Location/{id}")]
        public Location GetLocation([FromRoute]string id)
        {
            var location = _context.Locations.SingleOrDefault(x => x.Id == id);
            return location;
        }

        [HttpGet("VendorMedia")]
        public IEnumerable<VendorMedia> GetVendorMedia()
        {
            var media = _context.VendorMedia.ToList();
            return media;
        }

        [HttpPut("AddVendorClick/{vendorId}/{locationId}")]
        public IActionResult AddClick([FromRoute] int vendorId, [FromRoute] string locationId)
        {
            try
            { 
            if (_context.VendorAnalytics.Any(x => x.VendorId == vendorId.ToString() &&
                                                                 x.LocationId == locationId.ToString()))
                {
                    var analytics = _context.VendorAnalytics.Single(x => x.VendorId == vendorId.ToString() &&
                                                                   x.LocationId == locationId.ToString());
                    analytics.AddClick(DateTime.Now);
                    _context.Update(analytics);
                    _context.SaveChanges();
                }
                else
                {
                    VendorAnalytics analytics = new VendorAnalytics()
                    {
                        LocationId = locationId.ToString(),
                        VendorId = vendorId.ToString(),
                        Id = Guid.NewGuid().ToString()
                    };
                    analytics.AddClick(DateTime.Now);
                    _context.VendorAnalytics.Add(analytics);
                    _context.SaveChanges();
                }

                return Content("Succesfully updated vendor analytics");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPut("AddVendorImpression/{vendorId}/{locationId}")]
        public IActionResult AddImpression([FromRoute] int vendorId, [FromRoute] string locationId)
        {
            //First find if analytics exist
            try
            {
                if (_context.VendorAnalytics.Any(x => x.VendorId == vendorId.ToString() &&
                                                                     x.LocationId == locationId))
                {
                    var analytics = _context.VendorAnalytics.Single(x => x.VendorId == vendorId.ToString() &&
                                                                    x.LocationId == locationId);
                    analytics.AddImpression(DateTime.Now);
                    _context.Update(analytics);
                    _context.SaveChanges();
                }
                else
                {
                    VendorAnalytics analytics = new VendorAnalytics()
                    {
                        LocationId = locationId,
                        VendorId = vendorId.ToString(),
                        Id = Guid.NewGuid().ToString()
                    };
                    analytics.AddImpression(DateTime.Now);
                    _context.VendorAnalytics.Add(analytics);
                    _context.SaveChanges();
                }
                return Content("Succesfully updated vendor analytics");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("AddVendorCouponSent/{vendorId}/{locationId}")]
        public IActionResult AddVendorCouponSent([FromRoute] int vendorId, [FromRoute] string locationId)
        {
            //First find if analytics exist
            try
            {
                if (_context.VendorAnalytics.Any(x => x.VendorId == vendorId.ToString() &&
                                                        x.LocationId == locationId))
                {
                    var analytics = _context.VendorAnalytics.Single(x => x.VendorId == vendorId.ToString() &&
                                                                    x.LocationId == locationId);
                    analytics.AddCouponSent(DateTime.Now);
                    _context.Update(analytics);
                    _context.SaveChanges();
                }
                else
                {
                    //if analytics don't exist, we create a new one
                    VendorAnalytics analytics = new VendorAnalytics()
                    {
                        LocationId = locationId,
                        VendorId = vendorId.ToString(),
                        Id = Guid.NewGuid().ToString()
                    };
                    analytics.AddCouponSent(DateTime.Now);
                    _context.VendorAnalytics.Add(analytics);
                    _context.SaveChanges();
                }
                return Content("Succesfully updated vendor analytics");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);  
            }
        }

        [HttpPost("AddUserContact")]
        public IActionResult AddUserContact([FromBody] UserContactData contact)
        {
            _context.Add(contact);
            _context.SaveChanges();
            return Ok("Succesfully created user contact");
        }
    }
}