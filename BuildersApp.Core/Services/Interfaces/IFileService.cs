using BuildersApp.Core.Models;

namespace BuildersApp.Core.Services.Interfaces;

public interface IFileService
{
    public Task<string> AddDocument(Document document, Stream stream);
    public Task<bool> FileExists(Document document);
    public Task<string> GetDocumentPath(Document document);
}