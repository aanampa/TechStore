using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Domain.Entities
{

    public class Producto : BaseEntity
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
        public string ImagenUrl { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; } = true;

        public virtual ICollection<CarritoItem> CarritoItems { get; set; }
        public virtual ICollection<DetalleOrden> DetallesOrden { get; set; }
    }
}
