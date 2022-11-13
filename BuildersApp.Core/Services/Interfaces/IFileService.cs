using BuildersApp.Core.Models;

namespace BuildersApp.Core.Services.Interfaces;

public interface IFileService
{
    public Task AddDocument(Document document, Stream stream);
    public Task<byte[]> GetDocumentDownloadingStream(Document document);
}