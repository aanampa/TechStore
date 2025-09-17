using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        public string Documento { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        public virtual ICollection<CarritoItem> CarritoItems { get; set; }
        public virtual ICollection<Orden> Ordenes { get; set; }
    }
}
