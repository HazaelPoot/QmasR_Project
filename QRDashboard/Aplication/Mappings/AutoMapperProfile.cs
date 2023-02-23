using QRDashboard.Models;
using QRDashboard.Domain.Entities;
using System.Globalization;
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
        }
    }
}