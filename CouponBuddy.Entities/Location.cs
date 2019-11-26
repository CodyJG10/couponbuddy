using System;
using System.Collections.Generic;
using System.Text;

namespace CouponBuddy.Entities
{
    public class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<LocationAd> LocationAds { get; set; }
    }
}