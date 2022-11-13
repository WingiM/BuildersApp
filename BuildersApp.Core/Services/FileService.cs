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

    public async Task<string> AddDocument(Document document, Stream stream)
    {
        var fileName = GetDocumentFileName(document);
        await _fileRepository.UploadFileAsync(fileName, stream);
        return await _fileRepository.DownloadFileToStreamAsync(fileName);
    }

    public async Task<bool> FileExists(Document document)
    {
        return await _fileRepository.FileExistsAsync(document);
    }

    public async Task<string> GetDocumentPath(Document document)
    {
        return await _fileRepository.GetDocumentPath(document);
    }


    private string GetDocumentFileName(Document document)
    {
        return $"{document.Id}-{document.Name}.pdf";
    }
}