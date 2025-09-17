using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Domain.Entities;

namespace TechStore.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Producto>> GetAllProductsAsync();
        Task<Producto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Producto>> GetProductsByCategoryAsync(string category);

        Task<IEnumerable<Producto>> GetActiveProductsAsync();
        Task<IEnumerable<Producto>> SearchProductsAsync(string searchTerm);
        Task<Producto> CreateProductAsync(Producto product);
        Task<Producto> UpdateProductAsync(Producto product);
        Task<bool> DeleteProductAsync(Guid id);
        Task<bool> DeactivateProductAsync(Guid id);
        Task<bool> ReduceStockAsync(Guid productId, int quantity);
        Task<bool> IncreaseStockAsync(Guid productId, int quantity);

    }
}
