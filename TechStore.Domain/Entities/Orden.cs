using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Domain.Entities
{
    public class Orden : BaseEntity
    {
        public Guid ClienteId { get; set; }
        public DateTime FechaOrden { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string DireccionEnvio { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<DetalleOrden> DetallesOrden { get; set; }
    }
}
