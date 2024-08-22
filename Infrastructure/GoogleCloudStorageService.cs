

using Application.CloudService;
using Google.Cloud.Storage.V1;

namespace Infrastructure
{
    public class GoogleCloudStorageService : IGoogleCloudStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;
        private readonly string _baseUrl = "https://storage.googleapis.com";

        // Constructor accepting StorageClient and bucketName
        public GoogleCloudStorageService(StorageClient storageClient, string bucketName)
        {
            _storageClient = storageClient;
            _bucketName = bucketName;
        }

        public async Task<string> UploadFileAsync(string objectName, Stream fileStream)
        {
            await _storageClient.UploadObjectAsync(
                _bucketName, objectName, null, fileStream
            );

            return $"{_baseUrl}/{_bucketName}/{objectName}";
        }

        public async Task<Stream> DownloadFileAsync(string objectName)
        {
            var memoryStream = new MemoryStream();
            await _storageClient.DownloadObjectAsync(_bucketName, objectName, memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
