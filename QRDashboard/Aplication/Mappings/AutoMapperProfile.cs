using QRDashboard.Domain.Entities;
using AutoMapper;
using QRDashboard.Domain.Dtos;

namespace QRDashboard.Aplication.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Roles
            CreateMap<AdminType, DtoRol>().ReverseMap();
            #endregion Roles

            #region Usuario
            CreateMap<Usuario, DtoUsuario>()
                .ForMember(destino =>
                    destino.AdminTypeName,
                    opt => opt.MapFrom(origen => origen.AdminTypeNavigation.Tipo));

            CreateMap<DtoUsuario, Usuario>()
                .ForMember(destino =>
                    destino.AdminTypeNavigation,
                    opt => opt.Ignore());       
            #endregion Usuario

            #region Categoria
            CreateMap<Categorium,DtoCategoria>().ReverseMap();
            #endregion categoria

            #region Proyecto
            CreateMap<ProyectoQr, DtoProyecto>()
                .ForMember(destino =>
                destino.CategoriaName,
                opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Descripcion));

            CreateMap<DtoProyecto, ProyectoQr>()
                .ForMember(destino =>
                destino.IdCategoriaNavigation,
                opt => opt.Ignore());
            #endregion Proyecto

            #region Fotos
            CreateMap<FotosProyecto, DtoFotosProyecto>()
                .ForMember(destino =>
                    destino.NombreProyecto,
                    opt => opt.MapFrom(origen => origen.IdProjNavigation.Titulo));

            CreateMap<DtoFotosProyecto, FotosProyecto>()
                .ForMember(destino =>
                    destino.IdProjNavigation,
                    opt => opt.Ignore());
            #endregion Fotos
            
            #region Login
            CreateMap<Usuario, DtoLogin>().ReverseMap();
            #endregion Login
        }
    }
}