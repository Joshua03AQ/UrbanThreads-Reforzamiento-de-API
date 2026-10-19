using AutoMapper;
using SharedModels;
using UbranThreads_API.Dto;

namespace UbranThreads_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Producto, ProductoDto>().ForMember(dest => dest.ProductoID, opt => opt.MapFrom(src => src.ProductID));
            CreateMap<Producto, ProductoCreateDto>().ReverseMap();
            CreateMap<Producto, ProductoUpdateDto>().ReverseMap();
            CreateMap<Venta, VentaDto>().ReverseMap();
            CreateMap<Venta, VentaCreateDto>().ReverseMap();
            CreateMap<VentaDetalleCreateDto, VentaDetalle>().ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductoID));
            CreateMap<VentaDetalle, VentaDetalleDto>().ForMember(dest => dest.ProductoID, opt => opt.MapFrom(src => src.ProductID));
            CreateMap<Usuario, RegisterUsuarioDto>().ReverseMap();
        }
    }
}
