using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponBuddy.Entities.Metadata
{
    public class CategoryInfo : Attribute
    {
        public string DisplayName { get; set; }
        public string VendorListScreenTitle { get; set; }

        public CategoryInfo(string displayName, string vendorListScreenTitle)
        {
            DisplayName = displayName;
            VendorListScreenTitle = vendorListScreenTitle;
        }
    }
}