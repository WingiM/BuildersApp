namespace BuildersApp.Core.Repositories;

public interface IFileRepository
{
    public Task UploadFileAsync(string filename, Stream file);
    public Task<byte[]> DownloadFileToStreamAsync(string filename);
    public Task<bool> FileExistsAsync(string filename);
}