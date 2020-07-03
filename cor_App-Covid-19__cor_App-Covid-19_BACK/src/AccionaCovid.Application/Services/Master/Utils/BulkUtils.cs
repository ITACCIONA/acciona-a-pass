using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccionaCovid.Application.Services.Master
{
    public static class BulkUtils
    {
        /// <summary>
        /// Lee un formato CSV
        /// </summary>
        /// <param name="file">Archivo.</param>
        /// <param name="cancellationToken">Token CSV</param>
        /// <returns></returns>
        public static async Task<(string[], string[][])> ReadCsvAsync(IFormFile file, CancellationToken cancellationToken)
        {
            // Lectura del fichero
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
                stream.Position = 0;
                Encoding enc = Encoding.UTF8; //Encoding.GetEncoding(1252);
                using (StreamReader sr = new StreamReader(stream, enc))
                {
                    string headersLine = await sr.ReadLineAsync().ConfigureAwait(false);
                    string[] headers = headersLine.Split(";");

                    List<string[]> data = new List<string[]>();
                    while (!sr.EndOfStream)
                    {
                        string dataLine = await sr.ReadLineAsync().ConfigureAwait(false);
                        string[] dataFields = dataLine.Split(";");
                        data.Add(dataFields);
                    }
                    return (headers, data.ToArray());
                }
            }
        }

        /// <summary>
        /// Lee un formato CSV
        /// </summary>
        /// <param name="fileContent">Contenido del Archivo.</param>
        /// <param name="cancellationToken">Token CSV</param>
        /// <returns></returns>
        public static (string[], string[][]) ReadCsvFromStringAsync(string fileContent)
        {
            string[] lines = fileContent.Split("\n");
            if (lines.Length > 1)
            {
                string headersLine = lines[0].Trim();
                string[] headers = headersLine.Split(";");
                List<string[]> data = new List<string[]>();
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] dataFields = lines[i].Trim().Split(";");
                    if(dataFields.Length == headers.Length)
                        data.Add(dataFields);
                }
                return (headers, data.ToArray());
            }
            else return (new string[0], new string[0][]);
        }
    }
}
