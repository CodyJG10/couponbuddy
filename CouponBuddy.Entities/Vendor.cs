using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CouponBuddy.Entities
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int CategoryId { get; set; }
        public string Username { get; set; }
        public string LocationsJson { get; set; }
        public virtual ICollection<VendorMedia> VendorMedia { get; set; }
        public string WebsiteUrl { get; set; }
        #region Helpers
        public List<string> GetLocations()
        {
            if (LocationsJson != null)
            {
                var locationsList = JsonConvert.DeserializeObject<List<string>>(LocationsJson);
                return locationsList;
            }
            else
            {
                return new List<string>();
            }
        }

        public void UpdateLocations(List<string> locations)
        {
            LocationsJson = JsonConvert.SerializeObject(locations);
        }
        #endregion
    }
}