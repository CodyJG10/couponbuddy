﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CouponBuddy.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CouponBuddy.Web.Controllers
{
    [Route("[controller]")]
    public class CouponsController : Controller
    {
        private ApplicationDbContext _context;

        public CouponsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("")]
        public IActionResult Index(int id)
        {
            var coupon = _context.VendorCoupons.SingleOrDefault(x => x.Id == id);
            if (coupon != null)
            {
                return View("Coupon", coupon);
            }
            else
            {
                return BadRequest(404);
            }
        }
    }
}