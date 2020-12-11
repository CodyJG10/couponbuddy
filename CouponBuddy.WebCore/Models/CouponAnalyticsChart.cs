using CouponBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouponBuddy.WebCore.Models
{
    public class CouponAnalyticsChart
    {
        public List<KeyValue> Values { get; set; } = new List<KeyValue>();

        public Location Location { get; set; }

        public class KeyValue
        {
            public string Title { get; set; }
            public int Value { get; set; }
        }
    }
}