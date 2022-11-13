using BuildersApp.Core.Models;
using BuildersApp.Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BuildersApp.Data.Repositories;

public class FileRepository : IFileRepository
{
    private const string PathToSave = "wwwroot/documents";

    private readonly GridFSBucket _fileSystem;

    public FileRepository(MongoConnection connection)
    {
        _fileSystem = new GridFSBucket(connection.Database);
    }

    public async Task UploadFileAsync(string filename, Stream file)
    {
        await _fileSystem.UploadFromStreamAsync(filename, file);
    }

    public async Task<string> DownloadFileToStreamAsync(string filename)
    {
        var fs = new FileStream(PathToSave + $"/{filename}", FileMode.CreateNew);
        await _fileSystem.DownloadToStreamByNameAsync(filename, fs);

        return PathToSave + $"/{filename}";
    }

    public async Task<bool> FileExistsAsync(string filename)
    {
        return (await _fileSystem.FindAsync(FilterDefinition<GridFSFileInfo<ObjectId>>.Empty)).ToEnumerable()
            .FirstOrDefault(x => x.Filename == filename) is not null;
    }

    public Task<bool> FileExistsAsync(Document document)
    {
        return Task.FromResult(File.Exists(PathToSave + $"/{document.Id}-{document.Name}.pdf"));
    }

    public Task<string> GetDocumentPath(Document document)
    {
        return Task.FromResult(PathToSave + $"/{document.Id}-{document.Name}.pdf");
    }
}