using Azure.Storage.Blobs;
using CouponBuddy.Entities;
using CouponBuddy.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouponBuddy.Web.Storage
{
    public class FileManager
    {
        private readonly IConfiguration _config;
        private BlobServiceClient _blobServiceClient;

        public FileManager(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("Storage");
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<bool> UploadFile(IFormFile file, string fileName, Vendor vendor)
        {
            string containerName = VendorToContainer.GetContainerName(vendor);
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            using (var stream = file.OpenReadStream())
            {
                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(stream);
            }
            return true;
        }

        public async Task<bool> UploadFile(IFormFile file, string fileName, string containerName)
        {
            containerName = VendorToContainer.GetContainerName(containerName);
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            using (var stream = file.OpenReadStream())
            {
                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(stream);
            }
            return true;
        }
    }
}