using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccionaCovid.Crosscutting
{
    public class MockFileStorageService : IFileStorageService
    {
        public Task<bool> CheckFileExistsAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetFileContentAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetFileContentStringAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> ListFilesAsync(string folderPath)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> ListFoldersAsync(string folderPath)
        {
            throw new NotImplementedException();
        }

        public Task WriteFileAsync(string filePath, byte[] content)
        {
            throw new NotImplementedException();
        }
    }
}
