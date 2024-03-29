﻿using CouponBuddy.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CouponBuddy.Api.Interfaces
{
    public interface IDatabaseManager
    {
        Task<IEnumerable<Vendor>> GetVendors();
        Task<IEnumerable<Vendor>> GetVendors(string locationId);
        Task<Vendor> GetVendor(int vendorId);
        Task<IEnumerable<LocationAd>> GetAds();
        Task<IEnumerable<LocationAd>> GetAds(string locationId);
        Task<Location> GetLocation(string id);
        Task<IEnumerable<VendorMedia>> GetVendorMedia();
        Task<IEnumerable<VendorCoupon>> GetVendorCoupons(int vendorId);
        void AddImpression(Vendor vendor, string locationId);
        void AddClick(Vendor vendor, string locationId);
        void AddVendorCouponSent(Vendor vendor, string locationId);
        Task<Task> UpdateUptime(UptimeReport report);
        void AddUserContact(UserContactData contact);
    }
}