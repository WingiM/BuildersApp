using BuildersApp.Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace BuildersApp.Data.Repositories;

public class FileRepository : IFileRepository
{
    private readonly GridFSBucket _fileSystem;

    public FileRepository(MongoConnection connection)
    {
        _fileSystem = new GridFSBucket(connection.Database);
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
        return (await _fileSystem.FindAsync(FilterDefinition<GridFSFileInfo<ObjectId>>.Empty)).ToEnumerable()
            .FirstOrDefault(x => x.Filename == filename) is not null;
    }
}