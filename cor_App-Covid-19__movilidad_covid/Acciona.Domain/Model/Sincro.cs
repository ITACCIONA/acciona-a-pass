using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acciona.Domain.Model
{
    public class Sincro
    {
        public const int TYPE_PASPORT = 0;
        public const int TYPE_TEMPERATURE = 1;        

        [PrimaryKey, AutoIncrement]
        public long id { get; set; }
        public string User { get; set; }
        public long Time { get; set; }
        public int Type { get; set; }
        public string Serialized { get; set; }  //o base64        
    }
}
