using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouponBuddy.Web.Models
{
    public class AddVendorToLocationViewModel
    {
        public string Username { get; set; }
        public string LocationId { get; set; }
        public Dictionary<string, string> Locations { get; set; }
    }
}