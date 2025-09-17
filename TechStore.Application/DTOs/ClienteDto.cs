using System.ComponentModel.DataAnnotations;

namespace TechStore.Application.DTOs
{
    // DTO para mostrar información del cliente (sin contraseña)
    public class ClienteDto
    {
        public Guid Id { get; set; }
        public string Documento { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }

        // Propiedades de navegación (solo para consultas con relaciones)
        public ICollection<CarritoItemDto>? CarritoItems { get; set; }
        public ICollection<OrdenDto>? Ordenes { get; set; }

        // Propiedades calculadas
        public int TotalOrdenes => Ordenes?.Count ?? 0;
        public int TotalItemsCarrito => CarritoItems?.Sum(c => c.Cantidad) ?? 0;
    }

    // DTO para crear un nuevo cliente
    public class CreateClienteDto
    {
        [Required(ErrorMessage = "El documento es obligatorio")]
        [StringLength(20, ErrorMessage = "El documento no puede exceder 20 caracteres")]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = "El documento debe contener solo números y tener entre 8 y 15 dígitos")]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "La contraseña debe contener al menos: 1 minúscula, 1 mayúscula, 1 número y 1 carácter especial")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de contraseña es obligatoria")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public string Direccion { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no tiene un formato válido")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 20 caracteres")]
        [RegularExpression(@"^[\+]?[0-9\-\(\)\s]+$", ErrorMessage = "El teléfono solo puede contener números, espacios, paréntesis, guiones y el signo +")]
        public string Telefono { get; set; } = string.Empty;
    }

    // DTO para actualizar un cliente existente (sin documento, email ni contraseña)
    public class UpdateClienteDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios")]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public string Direccion { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no tiene un formato válido")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 20 caracteres")]
        [RegularExpression(@"^[\+]?[0-9\-\(\)\s]+$", ErrorMessage = "El teléfono solo puede contener números, espacios, paréntesis, guiones y el signo +")]
        public string Telefono { get; set; } = string.Empty;
    }

    // DTO para cambiar contraseña
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "La contraseña actual es obligatoria")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La nueva contraseña debe tener entre 8 y 100 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "La nueva contraseña debe contener al menos: 1 minúscula, 1 mayúscula, 1 número y 1 carácter especial")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de contraseña es obligatoria")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    // DTOs para las entidades relacionadas (versiones simplificadas)
    public class CarritoItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
        public DateTime FechaCreacion { get; set; }
    }

    public class OrdenDto
    {
        public Guid Id { get; set; }
        public string NumeroOrden { get; set; } = string.Empty;
        public DateTime FechaOrden { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string DireccionEnvio { get; set; } = string.Empty;
        public int TotalItems { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    // DTO para resumen del cliente
    public class ClienteResumenDto
    {
        public Guid Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int TotalOrdenes { get; set; }
        public decimal TotalCompras { get; set; }
        public DateTime? UltimaOrden { get; set; }
    }
}