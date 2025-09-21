using AutoMapper;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Domain.Entities;
using TechStore.Domain.Interfaces;

namespace TechStore.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();

            var productDtos =  _mapper.Map<IEnumerable<ProductDto>>(products);
            //return products.Select(MapToDto).ToList();

            return productDtos.ToList();

        }

        public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(category);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _productRepository.SearchProductsAsync(searchTerm);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Producto>(productDto);
            var createdProduct = await _productRepository.CreateProductAsync(product);
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public async Task<ProductDto> UpdateProductAsync(Guid id, ProductDto productDto)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);

            if (existingProduct == null)
                return null;

            var product = _mapper.Map<Producto>(productDto);
            product.Id = id; // Asegurar que el ID no cambie

            var updatedProduct = await _productRepository.UpdateProductAsync(product);
            return _mapper.Map<ProductDto>(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            return await _productRepository.DeleteProductAsync(id);
        }

        public async Task<bool> DeactivateProductAsync(Guid id)
        {
            return await _productRepository.DeactivateProductAsync(id);
        }

        public async Task<bool> ReduceStockAsync(Guid productId, int quantity)
        {
            return await _productRepository.ReduceStockAsync(productId, quantity);
        }

        public async Task<bool> IncreaseStockAsync(Guid productId, int quantity)
        {
            return await _productRepository.IncreaseStockAsync(productId, quantity);
        }

        #region Mappings

        private ProductDto MapToDto(Producto producto)
        {
            if (producto == null) return null;

            return new ProductDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Categoria = producto.Categoria,
                ImagenUrl = producto.ImagenUrl,
                Stock = producto.Stock,
                Activo = producto.Activo
            };
        }

        private Producto MapToEntity(ProductDto productDto)
        {
            if (productDto == null) return null;

            return new Producto
            {
                Id = productDto.Id,
                Nombre = productDto.Nombre,
                Descripcion = productDto.Descripcion,
                Precio = productDto.Precio,
                Categoria = productDto.Categoria,
                ImagenUrl = productDto.ImagenUrl,
                Stock = productDto.Stock,
                Activo = productDto.Activo
            };
        }

        private void UpdateEntityFromDto(Producto entity, ProductDto dto)
        {
            entity.Nombre = dto.Nombre;
            entity.Descripcion = dto.Descripcion;
            entity.Precio = dto.Precio;
            entity.Categoria = dto.Categoria;
            entity.ImagenUrl = dto.ImagenUrl;
            entity.Stock = dto.Stock;
            entity.Activo = dto.Activo;
        }


        #endregion


    }
}