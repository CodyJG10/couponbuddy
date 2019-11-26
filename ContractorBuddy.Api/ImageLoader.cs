using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BrochureBuddy.Api
{
    public class ImageLoader
    {
        public async Task<byte[]> LoadImage(string container, string fileName)
        {
            try
            {
                StorageManager manager = new StorageManager();
                var blockBlob = manager.GetClient()
                                    .GetContainerReference(container)
                                    .GetBlockBlobReference(fileName);
                await blockBlob.FetchAttributesAsync();
                long fileByteLength = blockBlob.Properties.Length;
                byte[] byteArray = new byte[fileByteLength];
                await blockBlob.DownloadToByteArrayAsync(byteArray, 0);
                return byteArray;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}