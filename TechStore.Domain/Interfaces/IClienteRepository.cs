using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Domain.Entities;

namespace TechStore.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(Guid id);
        Task<Cliente?> GetByIdWithCarritoAsync(Guid id);
        Task<Cliente?> GetByIdWithOrdenesAsync(Guid id);
        Task<Cliente?> GetByIdWithAllRelationsAsync(Guid id);
        Task<Cliente?> GetByEmailAsync(string email);
        Task<Cliente?> GetByDocumentoAsync(string documento);
        Task<Cliente> AddAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> DocumentoExistsAsync(string documento);
        Task<bool> CanDeleteClienteAsync(Guid id);
        Task<IEnumerable<Cliente>> GetClientesActivosAsync();
        Task<int> GetTotalClientesAsync();
    }
}
