using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrochureBuddy.Web.Areas.Identity
{
    public class User : IdentityUser
    {
        public class LocationReference
        {
            public int Id { get; set; }
            public string LocationId { get; set; }
            public string UserId { get; set; }

            public LocationReference()
            {
                Console.WriteLine("created new location ref object");
            }
        }
        public virtual ICollection<LocationReference> UserLocations { get; set; }
    }
}