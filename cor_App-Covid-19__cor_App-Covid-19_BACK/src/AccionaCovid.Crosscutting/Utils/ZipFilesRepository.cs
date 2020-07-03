using Ionic.Zip;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AccionaCovid.Crosscutting
{

    /// <summary>
    /// Interfaz para la gestión de ficheros zip
    /// </summary>
    public interface IZipFilesRepository
    {
        /// <summary>
        /// Método para generar un fichero zip
        /// </summary>
        /// <returns></returns>
        public MemoryStream CreteZipFileAsync(string zipFilePath = "");

        /// <summary>
        /// Obtiene el contenido de los ficheros como array de bytes si están comprimidos en un zip
        /// </summary>
        /// <param name="fileContent">Contenido del fichero</param>
        /// <returns></returns>
        Dictionary<string, string> GetZippedFilesContentStringAsync(byte[] fileContent);

    }

    public class ZipFilesRepository : IZipFilesRepository
    {
        /// <summary>
        /// Ruta de los ficheros
        /// </summary>
        private string zipFilePath;

        public ZipFilesRepository(string zipFilePath)
        {
            this.zipFilePath = zipFilePath;
        }

        public ZipFilesRepository()
        {
            this.zipFilePath = string.Empty;
        }

        ///// <summary>
        ///// Método que genera un fichero zip
        ///// </summary>
        ///// <returns></returns>
        //public async Task<MemoryStream> CreteZipFileAsync(string zipFilePathPartial = "")
        //{
        //    var zipFile = Path.GetTempPath() + "ZipFiles.zip";
        //    if (File.Exists(zipFile)) File.Delete(zipFile);

        //    ZipFile.CreateFromDirectory($"{zipFilePath}{zipFilePathPartial}" , zipFile);

        //    MemoryStream stream = new MemoryStream();
        //    using (FileStream file = new FileStream(zipFile, FileMode.Open, FileAccess.Read))
        //    {
        //        await file.CopyToAsync(stream).ConfigureAwait(false);
        //    }

        //    File.Delete(zipFile);
        //    return stream;
        //}

        ///// <summary>
        ///// Método que genera un fichero zip
        ///// </summary>
        ///// <returns></returns>
        //public async Task<MemoryStream> CreteZipFileAsync(string zipFilePathPartial = "")
        //{
        //    var zipStream = new MemoryStream();

        //    DirectoryInfo d = new DirectoryInfo($"{zipFilePath}{zipFilePathPartial}");//Assuming Test is your Folder
        //    FileInfo[] Files = d.GetFiles();

        //    using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        //    {
        //        foreach (var file in Files)
        //        {
        //            var entry = zip.CreateEntry(file.Name);
        //            using (var entryStream = entry.Open())
        //            {
        //                await file.OpenRead().CopyToAsync(entryStream).ConfigureAwait(false);
        //            }
        //        }
        //    }
        //    zipStream.Position = 0;
        //    return zipStream;
        //}

        /// <summary>
        /// Método que genera un fichero zip
        /// </summary>
        /// <returns></returns>
        public MemoryStream CreteZipFileAsync(string zipFilePathPartial = "")
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory($"{zipFilePath}{zipFilePathPartial}");

                MemoryStream output = new MemoryStream();
                zip.Save(output);
                return output;
            }
        }

        /// <summary>
        /// Obtiene el contenido de los ficheros como array de bytes si están comprimidos en un zip
        /// </summary>
        /// <param name="fileContent">Contenido del fichero</param>
        /// <returns></returns>
        public Dictionary<string, string> GetZippedFilesContentStringAsync(byte[] fileContent)
        {
            Dictionary<string, string> readFiles = new Dictionary<string, string>();
            using (MemoryStream msFileContent = new MemoryStream(fileContent))
            using (ZipFile zip = ZipFile.Read(msFileContent))
            {
                foreach(var zippedEntry in zip)
                {
                    if(!zippedEntry.IsDirectory)
                    {
                        using (MemoryStream msZippedFileContent = new MemoryStream())
                        {
                            zippedEntry.Extract(msZippedFileContent);
                            msZippedFileContent.Position = 0;
                            using (StreamReader reader = new StreamReader(msZippedFileContent, Encoding.UTF8))
                            {
                                //return reader.ReadToEnd();
                                readFiles.Add(zippedEntry.FileName, reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
            return readFiles;
        }
    }
}
