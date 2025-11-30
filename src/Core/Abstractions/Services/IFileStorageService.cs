using Core.Model;

namespace Core.Abstractions.Services;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(FileModel file, CancellationToken cancellationToken = default);
    Task<FileModel> DownloadFileAsync(string[] path, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string[] path, CancellationToken cancellationToken = default);
}
