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
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            // Mapeo de Entity a DTO principal
            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.CarritoItems, opt => opt.MapFrom(src => src.CarritoItems))
                .ForMember(dest => dest.Ordenes, opt => opt.MapFrom(src => src.Ordenes));

            // Mapeo de CreateDto a Entity
            CreateMap<CreateClienteDto, Cliente>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CarritoItems, opt => opt.Ignore())
                .ForMember(dest => dest.Ordenes, opt => opt.Ignore());

            // Mapeo de UpdateDto a Entity (solo campos permitidos)
            CreateMap<UpdateClienteDto, Cliente>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Documento, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CarritoItems, opt => opt.Ignore())
                .ForMember(dest => dest.Ordenes, opt => opt.Ignore());

            // Mapeo para ClienteResumenDto
            CreateMap<Cliente, ClienteResumenDto>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"))
                .ForMember(dest => dest.TotalOrdenes, opt => opt.MapFrom(src => src.Ordenes.Count))
                .ForMember(dest => dest.TotalCompras, opt => opt.MapFrom(src => src.Ordenes.Sum(o => o.Total)))
                .ForMember(dest => dest.UltimaOrden, opt => opt.MapFrom(src =>
                    src.Ordenes.Any() ? src.Ordenes.OrderByDescending(o => o.FechaOrden).First().FechaOrden : (DateTime?)null));

            // Mapeo para CarritoItemDto
            CreateMap<CarritoItem, CarritoItemDto>()
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto != null ? src.Producto.Nombre : ""));

            // Mapeo para OrdenDto
            CreateMap<Orden, OrdenDto>()
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.DetallesOrden.Sum(d => d.Cantidad)));
        }
    }
}
