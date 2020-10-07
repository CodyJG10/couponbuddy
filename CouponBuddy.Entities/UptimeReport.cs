using System;
using System.Collections.Generic;
using System.Text;

namespace CouponBuddy.Entities
{
    public class UptimeReport
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int DeviceId { get; set; }
        public bool Active { get; set; }
    }
}