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
            CreateMap<VentaDetalle, VentaDetalleDto>().ReverseMap();
            CreateMap<VentaDetalle, VentaDetalleCreateDto>().ReverseMap();
            CreateMap<Usuario, RegisterUsuarioDto>().ReverseMap();
        }
    }
}
