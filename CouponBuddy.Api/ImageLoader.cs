using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CouponBuddy.Api
{
    public class ImageLoader
    {
        public async Task<byte[]> LoadImage(string container, string fileName)
        {
            try
            {
                StorageManager manager = new StorageManager();
                var blockBlob = manager.GetClient()
                                    .GetBlobContainerClient(container)
                                    .GetBlockBlobClient(fileName);

                var properties = blockBlob.GetProperties();
                long fileByteLength = properties.Value.ContentLength;
                byte[] byteArray = new byte[fileByteLength];
                MemoryStream stream = new MemoryStream();
                await blockBlob.DownloadToAsync(stream);
                byteArray = stream.ToArray();
                return byteArray;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}