using BuildersApp.Core.Models;

namespace BuildersApp.Core.Repositories;

public interface IFileRepository
{
    public Task UploadFileAsync(string filename, Stream file);
    public Task<string> DownloadFileToStreamAsync(string filename);
    public Task<bool> FileExistsAsync(string filename);
    public Task<bool> FileExistsAsync(Document document);
    public Task<string> GetDocumentPath(Document document);
}