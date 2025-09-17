using AutoMapper;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Domain.Entities;
using TechStore.Domain.Interfaces;

namespace TechStore.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        #region Operaciones CRUD básicas

        public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
        }

        public async Task<ClienteDto?> GetByIdAsync(Guid id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto?> GetByIdWithCarritoAsync(Guid id)
        {
            var cliente = await _clienteRepository.GetByIdWithCarritoAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto?> GetByIdWithOrdenesAsync(Guid id)
        {
            var cliente = await _clienteRepository.GetByIdWithOrdenesAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto?> GetByIdWithAllRelationsAsync(Guid id)
        {
            var cliente = await _clienteRepository.GetByIdWithAllRelationsAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var cliente = await _clienteRepository.GetByEmailAsync(email);
            return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto?> GetByDocumentoAsync(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
                return null;

            var cliente = await _clienteRepository.GetByDocumentoAsync(documento);
            return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ClienteDto> CreateAsync(CreateClienteDto createClienteDto)
        {
            // Validaciones de negocio
            if (await _clienteRepository.EmailExistsAsync(createClienteDto.Email))
            {
                throw new InvalidOperationException("El email ya está registrado");
            }

            if (await _clienteRepository.DocumentoExistsAsync(createClienteDto.Documento))
            {
                throw new InvalidOperationException("El documento ya está registrado");
            }

            // Mapear y crear la entidad
            var cliente = _mapper.Map<Cliente>(createClienteDto);
            cliente.PasswordHash = HashPassword(createClienteDto.Password);

            var createdCliente = await _clienteRepository.AddAsync(cliente);
            return _mapper.Map<ClienteDto>(createdCliente);
        }

        public async Task<ClienteDto?> UpdateAsync(Guid id, UpdateClienteDto updateClienteDto)
        {
            var existingCliente = await _clienteRepository.GetByIdAsync(id);
            if (existingCliente == null)
            {
                return null;
            }

            // Mapear solo los campos permitidos para actualización
            _mapper.Map(updateClienteDto, existingCliente);
            await _clienteRepository.UpdateAsync(existingCliente);

            return _mapper.Map<ClienteDto>(existingCliente);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (!await _clienteRepository.ExistsAsync(id))
            {
                return false;
            }

            // Verificar si se puede eliminar (no tiene órdenes)
            if (!await _clienteRepository.CanDeleteClienteAsync(id))
            {
                throw new InvalidOperationException("No se puede eliminar el cliente porque tiene órdenes asociadas");
            }

            await _clienteRepository.DeleteAsync(id);
            return true;
        }

        #endregion

        #region Operaciones de validación

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _clienteRepository.ExistsAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return await _clienteRepository.EmailExistsAsync(email);
        }

        public async Task<bool> DocumentoExistsAsync(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
                return false;

            return await _clienteRepository.DocumentoExistsAsync(documento);
        }

        public async Task<bool> CanDeleteClienteAsync(Guid id)
        {
            return await _clienteRepository.CanDeleteClienteAsync(id);
        }

        #endregion

        #region Operaciones de autenticación

        public async Task<ClienteDto?> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            var cliente = await _clienteRepository.GetByEmailAsync(email);
            if (cliente == null || !VerifyPassword(password, cliente.PasswordHash))
            {
                return null;
            }

            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<bool> ChangePasswordAsync(Guid id, ChangePasswordDto changePasswordDto)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
            {
                return false;
            }

            // Verificar contraseña actual
            if (!VerifyPassword(changePasswordDto.CurrentPassword, cliente.PasswordHash))
            {
                throw new InvalidOperationException("La contraseña actual no es correcta");
            }

            // Actualizar contraseña
            cliente.PasswordHash = HashPassword(changePasswordDto.NewPassword);
            await _clienteRepository.UpdateAsync(cliente);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var cliente = await _clienteRepository.GetByEmailAsync(email);
            if (cliente == null)
            {
                return false;
            }

            cliente.PasswordHash = HashPassword(newPassword);
            await _clienteRepository.UpdateAsync(cliente);

            return true;
        }

        #endregion

        #region Consultas especiales

        public async Task<IEnumerable<ClienteDto>> GetClientesActivosAsync()
        {
            var clientes = await _clienteRepository.GetClientesActivosAsync();
            return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
        }

        public async Task<IEnumerable<ClienteResumenDto>> GetClientesResumenAsync()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClienteResumenDto>>(clientes);
        }

        public async Task<ClienteResumenDto?> GetClienteResumenAsync(Guid id)
        {
            var cliente = await _clienteRepository.GetByIdWithAllRelationsAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteResumenDto>(cliente);
        }

        public async Task<int> GetTotalClientesAsync()
        {
            return await _clienteRepository.GetTotalClientesAsync();
        }

        #endregion

        #region Búsquedas

        public async Task<IEnumerable<ClienteDto>> SearchClientesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            var clientes = await _clienteRepository.GetAllAsync();
            var filteredClientes = clientes.Where(c =>
                c.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Apellido.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Documento.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            );

            return _mapper.Map<IEnumerable<ClienteDto>>(filteredClientes);
        }

        public async Task<IEnumerable<ClienteDto>> GetClientesByNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<ClienteDto>();

            var clientes = await _clienteRepository.GetAllAsync();
            var filteredClientes = clientes.Where(c =>
                c.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase) ||
                c.Apellido.Contains(nombre, StringComparison.OrdinalIgnoreCase)
            );

            return _mapper.Map<IEnumerable<ClienteDto>>(filteredClientes);
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}