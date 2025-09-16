using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Domain.Entities
{
    public class CarritoItem : BaseEntity
    {
        public Guid ClienteId { get; set; }
        public Guid ProductoId { get; set; }
        public int Cantidad { get; set; } = 1;

        public virtual Cliente Cliente { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
