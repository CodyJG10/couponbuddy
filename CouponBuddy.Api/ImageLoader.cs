using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using CouponBuddy.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CouponBuddy.Api
{
    public class ImageLoader
    {
        protected BlobServiceClient _client;

        public ImageLoader(string connectionString)
        {
            _client = new BlobServiceClient(connectionString);
        }

        public async Task<Uri> DownloadBlob(string container, string fileName, string fileType = "")
        {
            Console.WriteLine("[Blob] Downloading blob: " + fileName);

            var blockBlob = _client
                .GetBlobContainerClient(container)
                .GetBlobClient(fileName);

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "media"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "media");
            }
            try
            {
                BlobDownloadInfo download = await blockBlob.DownloadAsync();

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "media\\" + MediaToFileName.GetFileName(container, fileName) + fileType;
                using (FileStream downloadFileStream = File.OpenWrite(filePath))
                {
                    download.Content.CopyToAsync(downloadFileStream).GetAwaiter().GetResult();
                    downloadFileStream.Close();
                }

                return new Uri(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Blob] Error encountered when downloading blob: " + e.Message);
                return null;
            }
        }
    }
}