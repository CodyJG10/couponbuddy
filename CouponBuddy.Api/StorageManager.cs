using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CouponBuddy.Api
{
    public class StorageManager
    {
        private BlobServiceClient _client;

        public StorageManager()
        {
            ConnectToClient();
        }

        private void ConnectToClient()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=couponbuddy;AccountKey=HUT5dj8HeOL2EGVf116GjO3y/EcP2Yf+IfFjFwVH/WH3WDKv3lNjmwuyqHftL48/7Wh+7EfTTG0Xqt1dwX4OgA==;EndpointSuffix=core.windows.net";
            _client = new BlobServiceClient(storageConnectionString);
        }

        public BlobServiceClient GetClient()
        {
            return _client;
        }
    }
}