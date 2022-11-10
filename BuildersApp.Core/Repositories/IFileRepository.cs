namespace BuildersApp.Core.Repositories;

public interface IFileRepository
{
    public Task UploadFileAsync(string filename, Stream file);
    public Task<Stream> DownloadFileToStreamAsync(string filename);
    public Task<bool> FileExistsAsync(string filename);
}