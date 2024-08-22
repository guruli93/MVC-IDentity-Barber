
namespace Application.CloudService
{
    public interface IGoogleCloudStorageService
    {
          Task<string> UploadFileAsync(string objectName, Stream fileStream);
        Task<Stream> DownloadFileAsync(string objectName);
    }
}
