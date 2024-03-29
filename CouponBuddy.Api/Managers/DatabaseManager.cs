﻿using CouponBuddy.Api.Interfaces;
using CouponBuddy.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CouponBuddy.Api.Managers
{
    public class DatabaseManager : IDatabaseManager
    {
        public HttpClient _client;

        public DatabaseManager(string database, string username, string password)
        {
            InitClient(database, username, password);
        }

        private void InitClient(string database, string username, string password)
        {
            _client = new HttpClient();
            var token = GetAuthenticationToken(database, username, password).GetAwaiter().GetResult();

            if(database.EndsWith("/"))
                database = database.Substring(0, database.Length - 1); 
            _client = new HttpClient()
            {
                BaseAddress = new Uri(database + "/Api/")
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<string> GetAuthenticationToken(string url, string username, string password)
        {
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Username", username),
                new KeyValuePair<string, string>("Password", password)
            };

            var body = new FormUrlEncodedContent(keyValues);

            var result = await _client.PostAsync(url + "auth/token", body);
            var content = await result.Content.ReadAsStringAsync();
            var authResult = new { token = "" };
            string token = JsonConvert.DeserializeAnonymousType(content, authResult).token;
            return token;
        }

        public async Task<IEnumerable<Vendor>> GetVendors()
        {
            var result = await _client.GetAsync("Vendors");
            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Vendor>>(content);
        }

        public async Task<IEnumerable<LocationAd>> GetAds()
        {
            var result = await _client.GetAsync("Ads");
            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<LocationAd>>(content);
        }

        public async Task<Vendor> GetVendor(int vendorId)
        {
            var vendors = await GetVendors();
            return vendors.ToList().Single(x => x.Id == vendorId);
        }

        public async Task<IEnumerable<LocationAd>> GetAds(string locationId)
        {
            var result = await _client.GetAsync("Ads/" + locationId);
            var content = await result.Content.ReadAsStringAsync();
            var adList = JsonConvert.DeserializeObject<IEnumerable<LocationAd>>(content);
            return adList;
        }

        public async Task<IEnumerable<Vendor>> GetVendors(string locationId)
        {
            var result = await _client.GetAsync("Vendors/" + locationId);
            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Vendor>>(content);
        }

        public async Task<Location> GetLocation(string id)
        {
            var result = await _client.GetAsync("Location/" + id);
            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Location>(content);
        }

        public async Task<IEnumerable<VendorMedia>> GetVendorMedia()
        {
            var result = await _client.GetAsync("VendorMedia");
            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<VendorMedia>>(content);
        }

        public async Task<IEnumerable<VendorCoupon>> GetVendorCoupons(int vendorId)
        {
            var result = await _client.GetAsync("coupons/" + vendorId);
            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<VendorCoupon>>(content);
        }

        public async void AddImpression(Vendor vendor, string locationId)
        {
            await _client.PutAsync("AddVendorImpression/" + vendor.Id.ToString() + "/" + locationId, null);
        }

        public async void AddClick(Vendor vendor, string locationId)
        {
            await _client.PutAsync("AddVendorClick/" + vendor.Id.ToString() + "/" + locationId, null);
        }

        public async void AddVendorCouponSent(Vendor vendor, string locationId)
        {
            await _client.PutAsync("AddVendorCouponSent/" + vendor.Id.ToString() + "/" + locationId, null);
        }

        public async Task<Task> UpdateUptime(UptimeReport report)
        {
            var content = new StringContent(JsonConvert.SerializeObject(report));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await _client.PutAsync("uptime/UpdateUptime", content);
            return Task.CompletedTask;
        }

        public async void AddUserContact(UserContactData contact)
        {
            var content = new StringContent(JsonConvert.SerializeObject(contact));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await _client.PostAsync("AddUserContact", content);
        }
    }
}