using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;


namespace Make_a_Drop.Application.Services.Impl
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly string _bucketName;
        private readonly GoogleCredential _credentials;
        private readonly StorageClient _storageClient;

        public FirebaseStorageService(IConfiguration configuration)
        {
            _bucketName = configuration.GetValue<string>("FirebaseStorage:BucketName");
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebasecredentials.json");
            _credentials = GoogleCredential.FromFile(filePath);
            _storageClient = StorageClient.Create(_credentials);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var objectName = $"{Guid.NewGuid()}-{fileName}";
            await _storageClient.UploadObjectAsync(_bucketName, objectName, null, fileStream);
            return objectName;
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var stream = new MemoryStream();
            await _storageClient.DownloadObjectAsync(_bucketName, fileName, stream);
            stream.Position = 0;
            return stream; 
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _storageClient.DeleteObjectAsync(_bucketName, fileName);
        }
    }
}
