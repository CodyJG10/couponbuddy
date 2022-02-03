using System;
using System.Collections.Generic;
using System.Text;

namespace CouponBuddy.Entities
{
    public class VendorMedia
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public Guid Guid { get; set; }
    }
}