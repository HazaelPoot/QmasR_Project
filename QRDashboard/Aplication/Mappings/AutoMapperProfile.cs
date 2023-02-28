using QRDashboard.Models;
using QRDashboard.Domain.Entities;
using AutoMapper;

namespace QRDashboard.Aplication.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Roles
            CreateMap<AdminType, VMRol>().ReverseMap();
            #endregion Roles

            #region Usuario
            CreateMap<Usuario, VMUsuario>()
                .ForMember(destino =>
                    destino.AdminTypeName,
                    opt => opt.MapFrom(origen => origen.AdminTypeNavigation.Tipo));

            CreateMap<VMUsuario, Usuario>()
                .ForMember(destino =>
                    destino.AdminTypeNavigation,
                    opt => opt.Ignore());       
            #endregion Usuario

            #region Categoria
            CreateMap<Categorium, VMCategoria>().ReverseMap();
            #endregion categoria

            #region Proyecto
            CreateMap<ProyectoQr, VMProyecto>()
                .ForMember(destino =>
                destino.CategoriaName,
                opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Descripcion));

            CreateMap<VMProyecto, ProyectoQr>()
                .ForMember(destino =>
                destino.IdCategoriaNavigation,
                opt => opt.Ignore());
            #endregion Proyecto

            
        }
    }
}