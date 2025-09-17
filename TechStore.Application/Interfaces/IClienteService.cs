using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces
{
    public interface IClienteService
    {
        // Operaciones básicas CRUD
        Task<IEnumerable<ClienteDto>> GetAllAsync();
        Task<ClienteDto?> GetByIdAsync(Guid id);
        Task<ClienteDto?> GetByIdWithCarritoAsync(Guid id);
        Task<ClienteDto?> GetByIdWithOrdenesAsync(Guid id);
        Task<ClienteDto?> GetByIdWithAllRelationsAsync(Guid id);
        Task<ClienteDto?> GetByEmailAsync(string email);
        Task<ClienteDto?> GetByDocumentoAsync(string documento);
        Task<ClienteDto> CreateAsync(CreateClienteDto createClienteDto);
        Task<ClienteDto?> UpdateAsync(Guid id, UpdateClienteDto updateClienteDto);
        Task<bool> DeleteAsync(Guid id);

        // Operaciones de validación
        Task<bool> ExistsAsync(Guid id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> DocumentoExistsAsync(string documento);
        Task<bool> CanDeleteClienteAsync(Guid id);

        // Operaciones de autenticación
        Task<ClienteDto?> AuthenticateAsync(string email, string password);
        Task<bool> ChangePasswordAsync(Guid id, ChangePasswordDto changePasswordDto);
        Task<bool> ResetPasswordAsync(string email, string newPassword);

        // Consultas especiales
        Task<IEnumerable<ClienteDto>> GetClientesActivosAsync();
        Task<IEnumerable<ClienteResumenDto>> GetClientesResumenAsync();
        Task<ClienteResumenDto?> GetClienteResumenAsync(Guid id);
        Task<int> GetTotalClientesAsync();

        // Búsquedas
        Task<IEnumerable<ClienteDto>> SearchClientesAsync(string searchTerm);
        Task<IEnumerable<ClienteDto>> GetClientesByNombreAsync(string nombre);
    }
}