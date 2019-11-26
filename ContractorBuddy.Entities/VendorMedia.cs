using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BrochureBuddy.Entities
{
    public class VendorMedia
    {
        public int Id { get; set; }
        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public Guid Guid { get; set; }
    }
}