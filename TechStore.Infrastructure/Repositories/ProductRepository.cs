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

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllProductsAsync()
        {
            return await _context.Productos
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetActiveProductsAsync()
        {
            return await _context.Productos
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<Producto> GetProductByIdAsync(Guid id)
        {
            return await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Producto>> GetProductsByCategoryAsync(string category)
        {
            return await _context.Productos
                .Where(p => p.Categoria == category && p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> SearchProductsAsync(string searchTerm)
        {
            return await _context.Productos
                .Where(p => p.Activo &&
                           (p.Nombre.Contains(searchTerm) ||
                            p.Descripcion.Contains(searchTerm) ||
                            p.Categoria.Contains(searchTerm)))
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<Producto> CreateProductAsync(Producto product)
        {
            product.FechaCreacion = DateTime.UtcNow;
            _context.Productos.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Producto> UpdateProductAsync(Producto product)
        {
            var existingProduct = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
                return null;

            existingProduct.Nombre = product.Nombre;
            existingProduct.Descripcion = product.Descripcion;
            existingProduct.Precio = product.Precio;
            existingProduct.Categoria = product.Categoria;
            existingProduct.ImagenUrl = product.ImagenUrl;
            existingProduct.Stock = product.Stock;
            existingProduct.Activo = product.Activo;

            _context.Productos.Update(existingProduct);
            await _context.SaveChangesAsync();

            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return false;

            _context.Productos.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateProductAsync(Guid id)
        {
            var product = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return false;

            product.Activo = false;
            _context.Productos.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReduceStockAsync(Guid productId, int quantity)
        {
            var product = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == productId && p.Activo);

            if (product == null || product.Stock < quantity)
                return false;

            product.Stock -= quantity;
            _context.Productos.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IncreaseStockAsync(Guid productId, int quantity)
        {
            var product = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == productId && p.Activo);

            if (product == null)
                return false;

            product.Stock += quantity;
            _context.Productos.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
