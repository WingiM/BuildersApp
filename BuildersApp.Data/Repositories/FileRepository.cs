using BuildersApp.Core.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BuildersApp.Data.Repositories;

public class FileRepository : IFileRepository
{
    private readonly GridFSBucket _fileSystem;

    public FileRepository(GridFSBucket fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public async Task UploadFileAsync(string filename, Stream file)
    {
        await _fileSystem.UploadFromStreamAsync(filename, file);
    }

    public async Task<Stream> DownloadFileToStreamAsync(string filename)
    {
        return await _fileSystem.OpenDownloadStreamByNameAsync(filename);
    }

    public async Task<bool> FileExistsAsync(string filename)
    {
        var filter = Builders<GridFSFileInfo>.Filter.Where(x => x.Filename == filename);
        return await (await _fileSystem.FindAsync(filter)).AnyAsync();
    }
}