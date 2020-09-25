using CouponBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouponBuddy.Web.Models
{
    public class LocationManagerIndexViewModel
    {
        public IEnumerable<Vendor> Vendors { get; set; }
        public Location Location { get; set; }
    }
}