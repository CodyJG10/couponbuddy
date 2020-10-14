using System;
using System.Collections.Generic;
using System.Text;

namespace CouponBuddy.Entities
{
    public class UserContactData
    {
        public int Id { get; set; }
        public string Contact { get; set; }
        public DateTime Date { get; set; }
        public string LocationId { get; set; }
    }
}
