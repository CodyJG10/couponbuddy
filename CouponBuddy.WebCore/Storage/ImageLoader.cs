using Azure.Storage.Blobs;
using CouponBuddy.Entities;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CouponBuddy.WebCore.Storage
{
    public class ImageLoader
    {
        private BlobServiceClient _client;

        public ImageLoader(string connectionString)
        {
            _client = new BlobServiceClient(connectionString);
        }

        public async Task<byte[]> LoadImage(string container, string fileName)
        {
            try
            {
                await _client
                    .GetBlobContainerClient(container)
                    .CreateIfNotExistsAsync();

                var blockBlob = _client
                    .GetBlobContainerClient(container)
                    .GetBlobClient(fileName);

                var response = await blockBlob.DownloadAsync();
                var stream = response.Value.Content;
                var bytes = StreamToByteArray(stream);
                return bytes;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static byte[] StreamToByteArray(Stream inputStream)
        {
            byte[] bytes = new byte[16384];
            using MemoryStream memoryStream = new MemoryStream();
            int count;
            while ((count = inputStream.Read(bytes, 0, bytes.Length)) > 0)
            {
                memoryStream.Write(bytes, 0, count);
            }
            return memoryStream.ToArray();
        }

        public async void DeleteVendorMediaBlob(Vendor vendor, VendorMedia media)
        {
            var container = VendorToContainer.GetContainerName(vendor);

            await _client
                   .GetBlobContainerClient(container)
                   .CreateIfNotExistsAsync();

            var blockBlob = _client
                .GetBlobContainerClient(container)
                .GetBlobClient(media.Guid.ToString());

            await blockBlob.DeleteAsync();
        }
    }
}