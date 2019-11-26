using BrochureBuddy.Entities;
using BrochureBuddy.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrochureBuddy.Web.Storage
{
    public class FileManager
    {
        private IConfiguration _config;

        public FileManager(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> UploadFile(IFormFile file, string fileName, Vendor vendor)
        {
            string storageConnectionString = _config.GetConnectionString("Storage");
            CloudStorageAccount storage = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storage.CreateCloudBlobClient();
            string nameFormatted = VendorToContainer.GetContainerName(vendor);
            var container = blobClient.GetContainerReference(nameFormatted);
            await container.CreateIfNotExistsAsync();
            using (var stream = file.OpenReadStream())
            {
                var blobRef = container.GetBlockBlobReference(fileName);
                await blobRef.UploadFromStreamAsync(stream);
            }
            return true;
        }

        public async Task<bool> UploadFile(IFormFile file, string fileName, string containerName)
        {
            string storageConnectionString = _config.GetConnectionString("Storage");
            CloudStorageAccount storage = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storage.CreateCloudBlobClient();
            containerName = containerName.ToLower().Replace(" ", "");
            var container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            using (var stream = file.OpenReadStream())
            {
                var blobRef = container.GetBlockBlobReference(fileName);
                await blobRef.UploadFromStreamAsync(stream);
            }
            return true;
        }
    }
}