using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Domain.Entities
{
    public class DetalleOrden : BaseEntity
    {
        public Guid OrdenId { get; set; }
        public Guid ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public virtual Orden Orden { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
