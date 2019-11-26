using CouponBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponBuddy.Util
{
    public static class VendorToContainer
    {
        public static string GetContainerName(Vendor vendor)
        {
            return vendor.Name.ToLower().Replace(" ", "").Replace("'", "").Replace("\"","");
        }

        public static string GetContainerName(string name)
        {
            return name.ToLower().Replace(" ", "").Replace("'", "").Replace("\"", "");
        }
    }
}