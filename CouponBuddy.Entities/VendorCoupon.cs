using System;
using System.Collections.Generic;
using System.Text;

namespace CouponBuddy.Entities
{
    public class VendorCoupon
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
    }
}