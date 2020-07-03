using System;
using System.Collections.Generic;
using System.Text;

namespace AccionaCovid.Domain.Core
{

    public class EntityLogicalRemove<T> : Entity<T> where T : Entity<T>
    {
        public bool Deleted { get; set; }

        public string LastAction { get; set; }

        public DateTime LastActionDate { get; set; }
    }
}
