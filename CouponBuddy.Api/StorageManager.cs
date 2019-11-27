using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace CouponBuddy.Api
{
    public class StorageManager
    {
        private CloudBlobClient _client;

        public StorageManager()
        {
            ConnectToClient();
        }

        private void ConnectToClient()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=couponbuddy;AccountKey=HUT5dj8HeOL2EGVf116GjO3y/EcP2Yf+IfFjFwVH/WH3WDKv3lNjmwuyqHftL48/7Wh+7EfTTG0Xqt1dwX4OgA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
            _client = account.CreateCloudBlobClient();
        }

        public CloudBlobClient GetClient()
        {
            return _client;
        }
    }
}