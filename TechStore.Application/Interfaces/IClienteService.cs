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
        Task<IEnumerable<ClienteDto>> GetAllAsync();
        Task<ClienteDto?> GetByIdAsync(Guid id);
        Task<ClienteDto?> GetByEmailAsync(string email);
        Task<ClienteDto?> GetByDocumentoAsync(string documento);
        Task<ClienteDto> CreateAsync(CreateClienteDto createClienteDto);
        Task<ClienteDto?> UpdateAsync(Guid id, UpdateClienteDto updateClienteDto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> DocumentoExistsAsync(string documento);
    }
}