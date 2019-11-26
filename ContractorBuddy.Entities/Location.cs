using System;
using System.Collections.Generic;
using System.Text;

namespace BrochureBuddy.Entities
{
    public class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<LocationAd> LocationAds { get; set; }
    }
}