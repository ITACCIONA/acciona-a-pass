using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccionaCovid.Crosscutting
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Comprueba si un fichero existe en la ruta seleccionada
        /// </summary>
        /// <param name="filePath">ruta completa al fichero</param>
        /// <returns></returns>
        Task<bool> CheckFileExistsAsync(string filePath);

        /// <summary>
        /// Obtiene el contenido de un fichero como un array de bytes
        /// </summary>
        /// <param name="filePath">Ruta completa al fichero</param>
        /// <returns></returns>
        Task<byte[]> GetFileContentAsync(string filePath);

        /// <summary>
        /// Obtiene el contenido de un fichero como un array de bytes
        /// </summary>
        /// <param name="filePath">Ruta completa al fichero</param>
        /// <returns></returns>
        Task<string> GetFileContentStringAsync(string filePath);

        /// <summary>
        /// Enumera las carpetas existentes dentro de otra
        /// </summary>
        /// <param name="folderPath">ruta a la carpeta</param>
        /// <returns></returns>
        Task<IEnumerable<string>> ListFoldersAsync(string folderPath);

        /// <summary>
        /// Lista los ficheros contenidos en una carpeta
        /// </summary>
        /// <param name="folderPath">ruta a la carpeta contenedora</param>
        /// <returns></returns>
        Task<IEnumerable<string>> ListFilesAsync(string folderPath);

        /// <summary>
        /// Escribe un fichero en la ruta especfificada
        /// </summary>
        /// <param name="filePath">Ruta completa al fichero junto al nombre del archivo y su extensión</param>
        /// <param name="content">Contenido del fichero</param>
        Task WriteFileAsync(string filePath, byte[] content);
    }
}
