// AppWeb/Controllers/ClientesController.cs
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;

namespace TechStore.AppWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        #region Operaciones CRUD básicas

        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        /// <returns>Lista de todos los clientes</returns>
        /// <response code="200">Lista de clientes obtenida exitosamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClienteDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            try
            {
                var clientes = await _clienteService.GetAllAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los clientes");
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un cliente por su ID
        /// </summary>
        /// <param name="id">ID único del cliente</param>
        /// <returns>Cliente encontrado</returns>
        /// <response code="200">Cliente encontrado</response>
        /// <response code="404">Cliente no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ClienteDto>> GetCliente(Guid id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdAsync(id);

                if (cliente == null)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente con ID {ClienteId}", id);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un cliente con sus items de carrito
        /// </summary>
        /// <param name="id">ID único del cliente</param>
        /// <returns>Cliente con sus items de carrito</returns>
        [HttpGet("{id:guid}/carrito")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ClienteDto>> GetClienteWithCarrito(Guid id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdWithCarritoAsync(id);

                if (cliente == null)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente con carrito, ID {ClienteId}", id);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un cliente con sus órdenes
        /// </summary>
        /// <param name="id">ID único del cliente</param>
        /// <returns>Cliente con sus órdenes</returns>
        [HttpGet("{id:guid}/ordenes")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ClienteDto>> GetClienteWithOrdenes(Guid id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdWithOrdenesAsync(id);

                if (cliente == null)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente con órdenes, ID {ClienteId}", id);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un cliente con todas sus relaciones (carrito y órdenes)
        /// </summary>
        /// <param name="id">ID único del cliente</param>
        /// <returns>Cliente completo con todas sus relaciones</returns>
        [HttpGet("{id:guid}/completo")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ClienteDto>> GetClienteCompleto(Guid id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdWithAllRelationsAsync(id);

                if (cliente == null)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente completo, ID {ClienteId}", id);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        /// <param name="createClienteDto">Datos del cliente a crear</param>
        /// <returns>Cliente creado</returns>
        /// <response code="201">Cliente creado exitosamente</response>
        /// <response code="400">Datos de entrada inválidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ClienteDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ClienteDto>> CreateCliente([FromBody] CreateClienteDto createClienteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var cliente = await _clienteService.CreateAsync(createClienteDto);
                return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente con email {Email}", createClienteDto.Email);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        /// <param name="id">ID del cliente a actualizar</param>
        /// <param name="updateClienteDto">Datos actualizados del cliente</param>
        /// <returns>Cliente actualizado</returns>
        /// <response code="200">Cliente actualizado exitosamente</response>
        /// <response code="400">Datos de entrada inválidos</response>
        /// <response code="404">Cliente no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ClienteDto>> UpdateCliente(Guid id, [FromBody] UpdateClienteDto updateClienteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var cliente = await _clienteService.UpdateAsync(id, updateClienteDto);

                if (cliente == null)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente con ID {ClienteId}", id);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un cliente
        /// </summary>
        /// <param name="id">ID del cliente a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="204">Cliente eliminado exitosamente</response>
        /// <response code="404">Cliente no encontrado</response>
        /// <response code="400">No se puede eliminar el cliente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCliente(Guid id)
        {
            try
            {
                var result = await _clienteService.DeleteAsync(id);

                if (!result)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente con ID {ClienteId}", id);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        #endregion

        #region Consultas por atributos específicos

        /// <summary>
        /// Busca un cliente por email
        /// </summary>
        /// <param name="email">Email del cliente</param>
        /// <returns>Cliente encontrado</returns>
        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ClienteDto>> GetClienteByEmail(string email)
        {
            try
            {
                var cliente = await _clienteService.GetByEmailAsync(email);

                if (cliente == null)
                {
                    return NotFound(new { message = $"Cliente con email {email} no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar cliente por email {Email}", email);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Busca un cliente por documento
        /// </summary>
        /// <param name="documento">Documento del cliente</param>
        /// <returns>Cliente encontrado</returns>
        [HttpGet("documento/{documento}")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ClienteDto>> GetClienteByDocumento(string documento)
        {
            try
            {
                var cliente = await _clienteService.GetByDocumentoAsync(documento);

                if (cliente == null)
                {
                    return NotFound(new { message = $"Cliente con documento {documento} no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar cliente por documento {Documento}", documento);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        #endregion

        #region Validaciones y verificaciones

        /// <summary>
        /// Verifica si un email ya existe en el sistema
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <returns>True si existe, false si no</returns>
        [HttpGet("email-exists/{email}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            try
            {
                var exists = await _clienteService.EmailExistsAsync(email);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar email {Email}", email);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Verifica si un documento ya existe en el sistema
        /// </summary>
        /// <param name="documento">Documento a verificar</param>
        /// <returns>True si existe, false si no</returns>
        [HttpGet("documento-exists/{documento}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> DocumentoExists(string documento)
        {
            try
            {
                var exists = await _clienteService.DocumentoExistsAsync(documento);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar documento {Documento}", documento);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        #endregion

        #region Autenticación y seguridad

        /// <summary>
        /// Autentica un cliente con email y contraseña
        /// </summary>
        /// <param name="loginDto">Credenciales de login</param>
        /// <returns>Datos del cliente autenticado</returns>
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ClienteDto>> Authenticate([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var cliente = await _clienteService.AuthenticateAsync(loginDto.Email, loginDto.Password);

                if (cliente == null)
                {
                    return Unauthorized(new { message = "Email o contraseña incorrectos" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar cliente con email {Email}", loginDto?.Email);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Cambia la contraseña de un cliente
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <param name="changePasswordDto">Datos para cambio de contraseña</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("{id:guid}/change-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _clienteService.ChangePasswordAsync(id, changePasswordDto);

                if (!result)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                return Ok(new { message = "Contraseña cambiada exitosamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar contraseña del cliente {ClienteId}", id);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        #endregion

        #region Búsquedas y filtros

        /// <summary>
        /// Busca clientes por término de búsqueda
        /// </summary>
        /// <param name="search">Término de búsqueda</param>
        /// <returns>Lista de clientes que coinciden con la búsqueda</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ClienteDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> SearchClientes([FromQuery] string search = "")
        {
            try
            {
                var clientes = await _clienteService.SearchClientesAsync(search);
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar clientes con término {SearchTerm}", search);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Busca clientes por nombre
        /// </summary>
        /// <param name="nombre">Nombre a buscar</param>
        /// <returns>Lista de clientes con el nombre especificado</returns>
        [HttpGet("nombre/{nombre}")]
        [ProducesResponseType(typeof(IEnumerable<ClienteDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientesByNombre(string nombre)
        {
            try
            {
                var clientes = await _clienteService.GetClientesByNombreAsync(nombre);
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar clientes por nombre {Nombre}", nombre);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        #endregion

        #region Consultas estadísticas y reportes

        /// <summary>
        /// Obtiene clientes activos
        /// </summary>
        /// <returns>Lista de clientes activos</returns>
        [HttpGet("activos")]
        [ProducesResponseType(typeof(IEnumerable<ClienteDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientesActivos()
        {
            try
            {
                var clientes = await _clienteService.GetClientesActivosAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes activos");
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene resumen de todos los clientes
        /// </summary>
        /// <returns>Lista de resúmenes de clientes</returns>
        [HttpGet("resumen")]
        [ProducesResponseType(typeof(IEnumerable<ClienteResumenDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ClienteResumenDto>>> GetClientesResumen()
        {
            try
            {
                var resumen = await _clienteService.GetClientesResumenAsync();
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener resumen de clientes");
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene resumen de un cliente específico
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Resumen del cliente</returns>
        [HttpGet("{id:guid}/resumen")]
        [ProducesResponseType(typeof(ClienteResumenDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ClienteResumenDto>> GetClienteResumen(Guid id)
        {
            try
            {
                var resumen = await _clienteService.GetClienteResumenAsync(id);

                if (resumen == null)
                {
                    return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
                }

                return Ok(resumen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener resumen del cliente {ClienteId}", id);
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene el total de clientes registrados
        /// </summary>
        /// <returns>Número total de clientes</returns>
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<int>> GetTotalClientes()
        {
            try
            {
                var total = await _clienteService.GetTotalClientesAsync();
                return Ok(new { total });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener total de clientes");
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        #endregion
    }

    // DTO adicional para login
    public class LoginDto
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = string.Empty;
    }
}