using AccionaCovid.Domain.Core;
using System;
using System.Collections.Generic;

namespace AccionaCovid.Domain.Model
{
    public partial class Audits : Entity<Audits>
    {
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public int? KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string RequestId { get; set; }
    }
}
