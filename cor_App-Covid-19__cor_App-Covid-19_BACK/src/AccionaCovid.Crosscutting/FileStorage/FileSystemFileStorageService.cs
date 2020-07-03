using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AccionaCovid.Crosscutting.FileStorage
{
    public class FileSystemFileStorageService : IFileStorageService
    {
        public async Task<bool> CheckFileExistsAsync(string filePath)
        {
            return await Task.Run(() => File.Exists(filePath)).ConfigureAwait(false);
        }

        public async Task<byte[]> GetFileContentAsync(string filePath)
        {
            return await File.ReadAllBytesAsync(filePath).ConfigureAwait(false);
        }

        public async Task<string> GetFileContentStringAsync(string filePath)
        {
            return await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
        }

        public async Task<IEnumerable<string>> ListFilesAsync(string folderPath)
        {
            return await Task.Run(() => Directory.EnumerateFiles(folderPath)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<string>> ListFoldersAsync(string folderPath)
        {
            return await Task.Run(() => Directory.EnumerateFiles(folderPath)).ConfigureAwait(false);
        }

        public async Task WriteFileAsync(string filePath, byte[] content)
        {
            await File.WriteAllBytesAsync(filePath, content).ConfigureAwait(false);
        }
    }
}
