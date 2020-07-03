using System;
using Acciona.Domain.Model.Base;

namespace Acciona.Domain.Model.Security
{
    public class Location : ListableObject
    {
        public long IdLocation { get; set; }
        public string Name { get; set; }
        public string Ciudad { get; set; }
        public string CodPostal { get; set; }
        public string Direccion { get; set; }
        public string Pais { get; set; }
        public string Extra { get; set; }

        public string GetListText()
        {
            return Name;
        }
    }
}
