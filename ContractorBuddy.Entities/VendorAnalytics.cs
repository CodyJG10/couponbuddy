using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrochureBuddy.Entities
{
    public class VendorAnalytics
    {
        public string Id { get; set; }
        public string VendorId { get; set; }
        public string LocationId { get; set; }
        public string VendorImpressionsJson { get; set; }
        public string VendorClicksJson { get; set; }

        #region Helpers
        public List<DateTime> GetImpressions()
        {
            if (!string.IsNullOrEmpty(VendorImpressionsJson))
            {
                return JsonConvert.DeserializeObject<List<DateTime>>(VendorImpressionsJson);
            }
            else
            {
                return new List<DateTime>();
            }
        }

        public List<DateTime> GetClicks()
        {
            if (!string.IsNullOrEmpty(VendorClicksJson))
            {
                return JsonConvert.DeserializeObject<List<DateTime>>(VendorClicksJson);
            }
            else
            {
                return new List<DateTime>();
            }
        }

        public void AddImpression(DateTime dateTime)
        {
            var impressions = GetImpressions();
            impressions.Add(dateTime);
            VendorImpressionsJson = JsonConvert.SerializeObject(impressions);
        }

        public void AddClick(DateTime dateTime)
        {
            var clicks = GetClicks();
            clicks.Add(dateTime);
            VendorClicksJson = JsonConvert.SerializeObject(clicks);
        }

        public List<DateTime> GetCurrentMonthsImpressions()
        {
            var allImpressions = GetImpressions();
            int month = DateTime.Now.Month;
            List<DateTime> thisMonthImpressions = new List<DateTime>();
            foreach (var impression in allImpressions)
            {
                if (impression.Month == month)
                {
                    thisMonthImpressions.Add(impression);
                }
            }
            return thisMonthImpressions;
        }

        public List<DateTime> GetCurrentMonthClicks()
        {
            var allClicks = GetClicks();
            int month = DateTime.Now.Month;
            List<DateTime> thisMonthClicks = new List<DateTime>();
            foreach (var click in allClicks)
            {
                if (click.Month == month)
                {
                    thisMonthClicks.Add(click);
                }
            }
            return thisMonthClicks;
        }
        #endregion
    }
}