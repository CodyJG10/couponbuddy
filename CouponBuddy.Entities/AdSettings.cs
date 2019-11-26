using System;
using System.Collections.Generic;
using System.Text;

namespace CouponBuddy.Entities
{
    public class AdSettings
    {
        public int Id { get; set; }
        public float AdDuration { get; set; }
        public bool IgnoreAdDurationForVideos { get; set; }
    }
}