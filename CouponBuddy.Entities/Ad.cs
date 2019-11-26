//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Text;

//namespace CouponBuddy.Entities
//{
//    public class Ad
//    {
//        public const int IMAGE = 1;
//        public const int VIDEO = 2;

//        public int Id { get; set; }
//        public string ContentType { get; set; }
//        public string FileName { get; set; }
//        [ForeignKey("Vendor")]
//        public int VendorId { get; set; }
//        [JsonIgnore]
//        public virtual Vendor Vendor { get; set; }

//        public Ad()
//        {

//        }
//    }
//}