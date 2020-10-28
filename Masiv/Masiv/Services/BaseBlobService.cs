using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Masiv.Services
{
    public class BaseBlobService<T>
    {
        internal BlobServiceClient _blobServiceClient;
        internal BlobContainerClient _containerClient;
        internal BlobClient _blobClient;

        public BaseBlobService(string connectionString, string containerName)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task Add(string id, T model)
        {
            _blobClient = _containerClient.GetBlobClient($"{id}.json");

            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(JsonConvert.SerializeObject(model));
            streamWriter.Flush();
            memoryStream.Position = 0;

            await _blobClient.UploadAsync(memoryStream, true);

            streamWriter.Dispose();
            memoryStream.Dispose();
        }

        public async Task AddList(string id, IEnumerable<T> model)
        {
            _blobClient = _containerClient.GetBlobClient($"{id}.json");

            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(JsonConvert.SerializeObject(model));
            streamWriter.Flush();
            memoryStream.Position = 0;

            await _blobClient.UploadAsync(memoryStream, true);

            streamWriter.Dispose();
            memoryStream.Dispose();
        }

        public async Task Delete(string id)
        {
            _blobClient = _containerClient.GetBlobClient($"{id}.json");
            await _blobClient.DeleteIfExistsAsync();
        }

        public async Task<T> Get(string id)
        {
            _blobClient = _containerClient.GetBlobClient($"{id}.json");

            if (await _blobClient.ExistsAsync())
            {
                BlobDownloadInfo download = await _blobClient.DownloadAsync();

                using (FileStream file = File.OpenWrite($"./{id}.json"))
                {
                    await download.Content.CopyToAsync(file);
                }

                JObject obj = JObject.Parse(File.ReadAllText($"./{id}.json"));
                File.Delete($"./{id}.json");

                return obj.ToObject<T>();
            }

            return default;
        }

        public async Task<ICollection<T>> GetList(string blobName)
        {
            _blobClient = _containerClient.GetBlobClient($"{blobName}.json");
            BlobDownloadInfo download = await _blobClient.DownloadAsync();

            using (FileStream file = File.OpenWrite($"{blobName}.json"))
                await download.Content.CopyToAsync(file);

            ICollection<T> list = JToken.Parse(File.ReadAllText($"./{blobName}.json")).ToObject<ICollection<T>>();
            File.Delete($"./{blobName}.json");

            return list;
        }
    }
}
