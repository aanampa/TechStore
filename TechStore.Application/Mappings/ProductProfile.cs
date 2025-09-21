using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.DTOs;
using TechStore.Domain.Entities;

namespace TechStore.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            //CreateMap<Producto, ProductDto>().ReverseMap();

            // Mapeo de Producto a ProductDto

            CreateMap<Producto, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Precio))
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria))
                .ForMember(dest => dest.ImagenUrl, opt => opt.MapFrom(src => src.ImagenUrl))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo));


            // Mapeo de ProductDto a Producto (para crear/actualizar)

            CreateMap<ProductDto, Producto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.CarritoItems, opt => opt.Ignore())
                .ForMember(dest => dest.DetallesOrden, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Precio))
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria))
                .ForMember(dest => dest.ImagenUrl, opt => opt.MapFrom(src => src.ImagenUrl))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock));

        }
    }
}
