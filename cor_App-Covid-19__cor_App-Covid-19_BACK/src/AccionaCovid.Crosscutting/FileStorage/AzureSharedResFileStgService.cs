using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage;
//using Azure.Storage.Files.Shares;
using Microsoft.Azure.Storage.File;
using Microsoft.Azure.Storage;
using System.Linq;

namespace AccionaCovid.Crosscutting
{
    public sealed class AzureSharedResFileStgService : IFileStorageService
    {
        private readonly CloudFileClient fileClient;
        public AzureSharedResFileStgService(string connectionString)
        {
            fileClient = CloudStorageAccount.Parse(connectionString).CreateCloudFileClient();
        }

        /// <summary>
        /// Comprueba si un fichero existe en la ruta seleccionada
        /// </summary>
        /// <param name="filePath">ruta completa al fichero</param>
        /// <returns></returns>
        public async Task<bool> CheckFileExistsAsync(string filePath)
        {
            return await AccessToFileAsync<bool>(filePath, async cf => await cf.ExistsAsync().ConfigureAwait(false));
        }

        private async Task<T> AccessToFileAsync<T>(string filePath, Func<CloudFile, Task<T>> delFunction)
        {
            var azureFile = AzureFilePath.FromFilePath(filePath);

            // Get a reference to the file share we created previously.
            CloudFileShare share = fileClient.GetShareReference(azureFile.ShareReference);

            // Ensure that the share exists.
            if (true || await share.ExistsAsync().ConfigureAwait(false)) //Obviamos esta comprobación porque puede que no se tenga privilegios suficientes
            {
                // Get a reference to the root directory for the share.
                CloudFileDirectory rootDir = share.GetRootDirectoryReference();

                // Get a reference to the directory we created previously.
                if (azureFile.Folders.Any())
                {
                    CloudFileDirectory sampleDir = rootDir.GetDirectoryReference(azureFile.Folders[0]);
                    if (!sampleDir.Exists()) throw new Exception("Incorrect route path.");
                    for (int i = 1; i < azureFile.Folders.Count; i++)
                    {
                        sampleDir = sampleDir.GetDirectoryReference(azureFile.Folders[i]);
                        if (!sampleDir.Exists()) throw new Exception("Incorrect route path.");
                    }
                    CloudFile file = sampleDir.GetFileReference(azureFile.FileName);

                    // Ensure that the file exists.
                    return  await delFunction(file).ConfigureAwait(false);
                }
                else
                {
                    CloudFile file = rootDir.GetFileReference(azureFile.FileName);

                    // Ensure that the file exists.
                    return await delFunction(file).ConfigureAwait(false);
                }
            }
            else throw new Exception("Share not found.");
        }

        /// <summary>
        /// Obtiene el contenido de un fichero como un array de bytes
        /// </summary>
        /// <param name="filePath">Ruta completa al fichero</param>
        /// <returns></returns>
        public async Task<byte[]> GetFileContentAsync(string filePath)
        {
            return await AccessToFileAsync(filePath, async cf => {
                using (MemoryStream stream = new MemoryStream())
                {
                    await cf.DownloadToStreamAsync(stream);
                    return stream.ToArray();
                }
            });
        }

        /// <summary>
        /// Obtiene el contenido de un fichero como una cadena de texto en UFT8
        /// </summary>
        /// <param name="filePath">Ruta completa al fichero</param>
        /// <returns></returns>
        public async Task<string> GetFileContentStringAsync(string filePath)
        {
            return await AccessToFileAsync(filePath, async cf => {
                using (MemoryStream stream = new MemoryStream())
                {
                    await cf.DownloadToStreamAsync(stream);
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            });
        }

        /// <summary>
        /// Enumera las carpetas existentes dentro de otra
        /// </summary>
        /// <param name="folderPath">ruta a la carpeta</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListFilesAsync(string folderPath)
        {
            return await ListFilesOrDirectoriesAsync(folderPath, false).ConfigureAwait(false);
        }

        /// <summary>
        /// Lista los ficheros contenidos en una carpeta
        /// </summary>
        /// <param name="folderPath">ruta a la carpeta contenedora</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListFoldersAsync(string folderPath)
        {
            return await ListFilesOrDirectoriesAsync(folderPath, true).ConfigureAwait(false);
        }

        private async Task<IEnumerable<string>> ListFilesOrDirectoriesAsync(string folderPath, bool isDirectory)
        {
            if (!folderPath.StartsWith("/")) throw new ArgumentException("First character on folderPath must be / (root).", nameof(folderPath));
            string[] filePathSplitted = folderPath.Split("/");

            if (filePathSplitted.Length < 2) throw new ArgumentException("FolderPath correct format is \"/{folder/s}\".", nameof(folderPath));

            // Get a reference to the file share we created previously.
            CloudFileShare share = fileClient.GetShareReference(filePathSplitted[1]);

            // Ensure that the share exists.
            if (true || await share.ExistsAsync().ConfigureAwait(false)) //Obviamos esta comprobación porque puede que no se tenga privilegios suficientes
            {
                // Get a reference to the root directory for the share.
                CloudFileDirectory selDir = share.GetRootDirectoryReference();
                if (!selDir.Exists()) throw new Exception("Incorrect route path.");
                // Get a reference to the directory we created previously.
                if (filePathSplitted.Length > 2)
                {
                    CloudFileDirectory tempDir = selDir.GetDirectoryReference(filePathSplitted[2]);
                    if (!tempDir.Exists()) throw new Exception("Incorrect route path.");
                    for (int i = 3; i < filePathSplitted.Length; i++)
                    {
                        tempDir = tempDir.GetDirectoryReference(filePathSplitted[i]);
                        if (!tempDir.Exists()) throw new Exception("Incorrect route path.");
                    }
                    selDir = tempDir;
                }

                List<IListFileItem> results = new List<IListFileItem>();
                FileContinuationToken token = null;
                do
                {
                    FileResultSegment resultSegment = await selDir.ListFilesAndDirectoriesSegmentedAsync(token);
                    results.AddRange(resultSegment.Results);
                    token = resultSegment.ContinuationToken;
                }
                while (token != null);
                if(isDirectory)
                    return results.Where(lfi => lfi is CloudFileDirectory).Select(lfi => ((CloudFileDirectory)lfi).Name);
                else
                    return results.Where(lfi => lfi is CloudFile).Select(lfi => ((CloudFile)lfi).Name);
            }
            else throw new Exception("Share not found.");
        }

        /// <summary>
        /// Escribe un fichero en la ruta especfificada
        /// </summary>
        /// <param name="filePath">Ruta completa al fichero junto al nombre del archivo y su extensión</param>
        /// <param name="content">Contenido del fichero</param>
        public async Task WriteFileAsync(string filePath, byte[] content)
        {
            await AccessToFileAsync(filePath, async cf => {
                using (MemoryStream stream = new MemoryStream())
                {
                    await cf.UploadFromStreamAsync(stream);
                }
                return Task.CompletedTask;
            });
        }

        class AzureFilePath
        {
            /// <summary>
            /// Nombre del contenedor
            /// </summary>
            public string ShareReference { get; set; }

            /// <summary>
            /// Carpetas encontradas
            /// </summary>
            public List<string> Folders { get; set; }

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
                if (!filePath.StartsWith("/")) throw new ArgumentException("First character on filePath must be / (root).", nameof(filePath));

                string[] filePathSplitted = filePath.Split("/");

                if(filePathSplitted.Length < 3) throw new ArgumentException("FilePath correct format is \"/{container}/{Opt.folder/s}/{filename}\".", nameof(filePath));

                List<string> foldersTemp = new List<string>();

                for (int i = 2; i < filePathSplitted.Length - 1; i++)
                    foldersTemp.Add(filePathSplitted[i]);

                return new AzureFilePath() {
                    ShareReference = filePathSplitted[1],
                    Folders = foldersTemp,
                    FileName = filePathSplitted[filePathSplitted.Length - 1]
                };
            }
        }
    }
}
