using BuildersApp.Core.Models;
using BuildersApp.Core.Repositories;
using BuildersApp.Core.Services.Interfaces;

namespace BuildersApp.Core.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;

    public FileService(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task AddDocument(Document document, Stream stream)
    {
        var fileName = GetDocumentFileName(document);
        await _fileRepository.UploadFileAsync(fileName, stream);
    }

    public async Task<byte[]> GetDocumentDownloadingStream(Document document)
    {
        var fileName = GetDocumentFileName(document);
        return await _fileRepository.DownloadFileToStreamAsync(fileName);
    }

    private string GetDocumentFileName(Document document)
    {
        return $"{document.Id}-{document.Name}-{document.DateCreated}";
    }
}