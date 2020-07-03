using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AccionaCovid.Crosscutting
{
    public sealed class AzureFileStorageService : IFileStorageService
    {
        private readonly BlobServiceClient blobServiceClient;
        public AzureFileStorageService(string connectionString)
        {
            blobServiceClient = new BlobServiceClient(connectionString);
        }

        /// <summary>
        /// Comprueba si un fichero existe en la ruta seleccionada
        /// </summary>
        /// <param name="filePath">ruta completa al fichero</param>
        /// <returns></returns>
        public async Task<bool> CheckFileExistsAsync(string filePath)
        {
            var azureFile = AzureFilePath.FromFilePath(filePath);
            var containerService = blobServiceClient.GetBlobContainerClient(azureFile.Container);
            return await containerService.GetBlobClient(azureFile.FileName).ExistsAsync();
        }

        /// <summary>
        /// Obtiene el contenido de un fichero como un array de bytes
        /// </summary>
        /// <param name="filePath">Ruta completa al fichero</param>
        /// <returns></returns>
        public async Task<byte[]> GetFileContentAsync(string filePath)
        {
            var azureFile = AzureFilePath.FromFilePath(filePath);
            var containerService = blobServiceClient.GetBlobContainerClient(azureFile.Container);
            BlobClient blobClient = containerService.GetBlobClient(azureFile.FileName);

            if (!await blobClient.ExistsAsync()) throw new FileNotFoundException();

            using (MemoryStream stream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Obtiene el contenido de un fichero como una cadena de texto en UFT8
        /// </summary>
        /// <param name="filePath">Ruta completa al fichero</param>
        /// <returns></returns>
        public async Task<string> GetFileContentStringAsync(string filePath)
        {
            var azureFile = AzureFilePath.FromFilePath(filePath);
            var containerService = blobServiceClient.GetBlobContainerClient(azureFile.Container);
            BlobClient blobClient = containerService.GetBlobClient(azureFile.FileName);

            if (!await blobClient.ExistsAsync()) throw new FileNotFoundException();

            using (MemoryStream stream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Enumera las carpetas existentes dentro de otra
        /// </summary>
        /// <param name="folderPath">ruta a la carpeta</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListFilesAsync(string folderPath)
        {
            if (!folderPath.StartsWith("/")) throw new ArgumentException("First character on folderPath must be / (root).", nameof(folderPath));
            string[] filePathSplitted = folderPath.Split("/");

            if (filePathSplitted.Length != 2) throw new ArgumentException("FolderPath correct format is \"/{container}\".", nameof(folderPath));

            var containerService = blobServiceClient.GetBlobContainerClient(folderPath.Replace("/", ""));

            List<string> files = new List<string>();
            await foreach (var contItem in containerService.GetBlobsAsync())
            {
                files.Add(contItem.Name);
            }
            return files;
        }

        /// <summary>
        /// Lista los ficheros contenidos en una carpeta
        /// </summary>
        /// <param name="folderPath">ruta a la carpeta contenedora</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListFoldersAsync(string folderPath)
        {
            if (!folderPath.StartsWith("/")) throw new ArgumentException("First character on folderPath must be / (root).", nameof(folderPath));
            if (folderPath.Length > 1) throw new ArgumentException("ONly can list folders from / (root).", nameof(folderPath));

            List<string> folders = new List<string>();
            await foreach (var contItem in blobServiceClient.GetBlobContainersAsync())
            {
                folders.Add(contItem.Name);
            }
            return folders;
        }

        /// <summary>
        /// Escribe un fichero en la ruta especfificada
        /// </summary>
        /// <param name="filePath">Ruta completa al fichero junto al nombre del archivo y su extensión</param>
        /// <param name="content">Contenido del fichero</param>
        public async Task WriteFileAsync(string filePath, byte[] content)
        {
            var azureFile = AzureFilePath.FromFilePath(filePath);
            var containerService = blobServiceClient.GetBlobContainerClient(azureFile.Container);
            BlobClient blobClient = containerService.GetBlobClient(azureFile.FileName);
            using(MemoryStream stream = new MemoryStream(content))
            {
                stream.Position = 0;
                await blobClient.UploadAsync(stream, true);
            }
        }

        class AzureFilePath
        {
            /// <summary>
            /// Nombre del contenedor
            /// </summary>
            public string Container { get; set; }

            /// <summary>
            /// Nombre del archivo
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// Crea un AzureFilePath desde una ruta a un fichero
            /// </summary>
            /// <param name="filePath">Ruta completa al fichero</param>
            /// <returns></returns>
            public static AzureFilePath FromFilePath(string filePath)
            {
                Regex fpRegex = new Regex("^\\/([\\w- ]+\\/)+[\\w- ]+\\.\\w+$");
                if (!fpRegex.IsMatch(filePath)) 
                    throw new ArgumentException("FilePath correct format is \"/{container}/{filename}\".", nameof(filePath));
                
                if (!filePath.StartsWith("/")) throw new ArgumentException("First character on filePath must be / (root).", nameof(filePath));

                string[] filePathSplitted = filePath.Split("/");

                if(filePathSplitted.Length != 3) throw new ArgumentException("FilePath correct format is \"/{container}/{filename}\".", nameof(filePath));

                return new AzureFilePath() {
                    Container = filePathSplitted[1],
                    FileName = filePathSplitted[2]
                };
            }
        }
    }
}
