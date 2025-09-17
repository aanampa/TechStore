using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Domain.Entities;
using TechStore.Domain.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Consultas básicas

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .OrderBy(c => c.Apellido)
                .ThenBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(Guid id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente?> GetByIdWithCarritoAsync(Guid id)
        {
            return await _context.Clientes
                .Include(c => c.CarritoItems)
                    .ThenInclude(ci => ci.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente?> GetByIdWithOrdenesAsync(Guid id)
        {
            return await _context.Clientes
                .Include(c => c.Ordenes)
                    .ThenInclude(o => o.DetallesOrden)
                        .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente?> GetByIdWithAllRelationsAsync(Guid id)
        {
            return await _context.Clientes
                .Include(c => c.CarritoItems)
                    .ThenInclude(ci => ci.Producto)
                .Include(c => c.Ordenes)
                    .ThenInclude(o => o.DetallesOrden)
                        .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente?> GetByEmailAsync(string email)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<Cliente?> GetByDocumentoAsync(string documento)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Documento == documento);
        }

        #endregion

        #region Operaciones CRUD

        public async Task<Cliente> AddAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var cliente = await GetByIdAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }

        #endregion

        #region Validaciones

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Clientes
                .AnyAsync(c => c.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Clientes
                .AnyAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> DocumentoExistsAsync(string documento)
        {
            return await _context.Clientes
                .AnyAsync(c => c.Documento == documento);
        }

        public async Task<bool> CanDeleteClienteAsync(Guid id)
        {
            // Verificar si el cliente tiene órdenes
            var hasOrders = await _context.Ordenes
                .AnyAsync(o => o.ClienteId == id);

            return !hasOrders;
        }

        #endregion

        #region Consultas especiales

        public async Task<IEnumerable<Cliente>> GetClientesActivosAsync()
        {
            // Por ahora retornamos todos los clientes
            // Se podría agregar una propiedad "Activo" a la entidad Cliente
            return await GetAllAsync();
        }

        public async Task<int> GetTotalClientesAsync()
        {
            return await _context.Clientes.CountAsync();
        }

        #endregion

        #region Métodos adicionales para búsquedas

        public async Task<IEnumerable<Cliente>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync();
            }

            return await _context.Clientes
                .Where(c =>
                    c.Nombre.Contains(searchTerm) ||
                    c.Apellido.Contains(searchTerm) ||
                    c.Email.Contains(searchTerm) ||
                    c.Documento.Contains(searchTerm))
                .OrderBy(c => c.Apellido)
                .ThenBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> GetClientesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Clientes
                .Where(c => c.FechaCreacion >= startDate && c.FechaCreacion <= endDate)
                .OrderBy(c => c.FechaCreacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> GetClientesWithRecentOrdersAsync(int days = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);

            return await _context.Clientes
                .Where(c => c.Ordenes.Any(o => o.FechaOrden >= cutoffDate))
                .Include(c => c.Ordenes)
                .OrderByDescending(c => c.Ordenes.Max(o => o.FechaOrden))
                .ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> GetTopClientesByOrderCountAsync(int topCount = 10)
        {
            return await _context.Clientes
                .Include(c => c.Ordenes)
                .Where(c => c.Ordenes.Any())
                .OrderByDescending(c => c.Ordenes.Count)
                .Take(topCount)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> GetTopClientesByTotalSpentAsync(int topCount = 10)
        {
            return await _context.Clientes
                .Include(c => c.Ordenes)
                .Where(c => c.Ordenes.Any())
                .OrderByDescending(c => c.Ordenes.Sum(o => o.Total))
                .Take(topCount)
                .ToListAsync();
        }

        #endregion
    }
}
