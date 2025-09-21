using Microsoft.AspNetCore.Mvc;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Application.Services;
using TechStore.Controllers;
using TechStore.Web.Models;

namespace TechStore.AppWeb.Controllers
{

    public class ClientesController : Controller
    {
        private readonly ILogger<ClientesController> _logger;
        private readonly IClienteService _clienteService;

        public ClientesController(
            ILogger<ClientesController> logger, 
            IClienteService clienteService)
        {
            _logger = logger;
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            //ProductoViewModel model = new ProductoViewModel();

            var lista = await _clienteService.GetAllAsync();

            return View();
        }


        /// <summary>
        /// Obtiene todos los clientes
        /// </summary>
        /// <returns>Lista de clientes</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            try
            {
                var clientes = await _clienteService.GetAllAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un cliente por su ID
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Cliente encontrado</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClienteDto>> GetCliente(Guid id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdAsync(id);

                if (cliente == null)
                {
                    return NotFound($"Cliente con ID {id} no encontrado");
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un cliente por su email
        /// </summary>
        /// <param name="email">Email del cliente</param>
        /// <returns>Cliente encontrado</returns>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<ClienteDto>> GetClienteByEmail(string email)
        {
            try
            {
                var cliente = await _clienteService.GetByEmailAsync(email);

                if (cliente == null)
                {
                    return NotFound($"Cliente con email {email} no encontrado");
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un cliente por su documento
        /// </summary>
        /// <param name="documento">Documento del cliente</param>
        /// <returns>Cliente encontrado</returns>
        [HttpGet("documento/{documento}")]
        public async Task<ActionResult<ClienteDto>> GetClienteByDocumento(string documento)
        {
            try
            {
                var cliente = await _clienteService.GetByDocumentoAsync(documento);

                if (cliente == null)
                {
                    return NotFound($"Cliente con documento {documento} no encontrado");
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        /// <param name="createClienteDto">Datos del cliente a crear</param>
        /// <returns>Cliente creado</returns>
        [HttpPost]
        public async Task<ActionResult<ClienteDto>> CreateCliente(CreateClienteDto createClienteDto)
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <param name="updateClienteDto">Datos actualizados del cliente</param>
        /// <returns>Cliente actualizado</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ClienteDto>> UpdateCliente(Guid id, UpdateClienteDto updateClienteDto)
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
                    return NotFound($"Cliente con ID {id} no encontrado");
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina un cliente
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCliente(Guid id)
        {
            try
            {
                var result = await _clienteService.DeleteAsync(id);

                if (!result)
                {
                    return NotFound($"Cliente con ID {id} no encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica si un email ya existe
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <returns>True si existe, false si no</returns>
        [HttpGet("email-exists/{email}")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            try
            {
                var exists = await _clienteService.EmailExistsAsync(email);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica si un documento ya existe
        /// </summary>
        /// <param name="documento">Documento a verificar</param>
        /// <returns>True si existe, false si no</returns>
        [HttpGet("documento-exists/{documento}")]
        public async Task<ActionResult<bool>> DocumentoExists(string documento)
        {
            try
            {
                var exists = await _clienteService.DocumentoExistsAsync(documento);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}