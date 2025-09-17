using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Domain.Entities;

namespace TechStore.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductDto>> GetActiveProductsAsync();
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
        Task<ProductDto> CreateProductAsync(ProductDto productDto);
        Task<ProductDto> UpdateProductAsync(Guid id, ProductDto productDto);
        Task<bool> DeleteProductAsync(Guid id);
        Task<bool> DeactivateProductAsync(Guid id);
        Task<bool> ReduceStockAsync(Guid productId, int quantity);
        Task<bool> IncreaseStockAsync(Guid productId, int quantity);

    }
}
