namespace Make_a_Drop.Application.Services
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);

        Task<Stream> DownloadFileAsync(string fileName);

        Task DeleteFileAsync(string fileName);
    }
}
