using AccionaCovid.Domain.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccionaCovid.Data.Core
{
    /// <summary>
    /// Clase de entrada a la auditoria
    /// </summary>
    public class AuditEntry
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entry"></param>
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        /// <summary>
        /// Entidad a tratar
        /// </summary>
        public EntityEntry Entry { get; }

        /// <summary>
        /// Nombre de la tabla a auditar
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Identificador del registro a auditar
        /// </summary>
        public int KeyValue { get; set; }

        /// <summary>
        /// Lista de los viajos valores
        /// </summary>
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Lista de los nuevos valores
        /// </summary>
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Lista de propiedades temporales
        /// </summary>
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        /// <summary>
        /// Propiedad que indica si el registro tiene un borrado logico
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Ultima accion realizada sobre el registro
        /// </summary>
        public string LastAction { get; set; }

        /// <summary>
        /// Fecha de la ultima accion
        /// </summary>
        public DateTime LastActionDate { get; set; }

        /// <summary>
        /// Identificador del usuario que ha realizado la ultima accion
        /// </summary>
        public int IdUser { get; set; }

        /// <summary>
        /// Nombre del usuario que ha realizado la ultima accion
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// RequestId
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Propiedades Temporales
        /// </summary>
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        /// <summary>
        /// Auditar
        /// </summary>
        /// <returns></returns>
        public Audits ToAudit()
        {
            var audit = new Audits();
            audit.TableName = TableName;
            audit.DateTime = DateTime.UtcNow;
            audit.KeyValues = KeyValue;
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.Deleted = Deleted;
            audit.LastAction = LastAction;
            audit.LastActionDate = DateTime.UtcNow;
            audit.IdUser = IdUser;
            audit.UserName = UserName;
            audit.RequestId = RequestId;
            return audit;
        }
    }
}
