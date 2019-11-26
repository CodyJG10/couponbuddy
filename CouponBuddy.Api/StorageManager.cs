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
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=CouponBuddy;AccountKey=W7K57XumvbroU0fIwdlOz/8da9Y+YXAkprA/n9UqtSu/j+o4uE9ehojmzwhcyZ52LXeqXJj0IflQYV5JHt7EtQ==;EndpointSuffix=core.windows.net";
            CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
            _client = account.CreateCloudBlobClient();
        }

        public CloudBlobClient GetClient()
        {
            return _client;
        }
    }
}