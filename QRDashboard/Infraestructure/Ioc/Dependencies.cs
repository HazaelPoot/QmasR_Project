
using Microsoft.EntityFrameworkCore;
using QRDashboard.Aplication.Services;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Infraestructure.Data;
using QRDashboard.Infraestructure.Repositories;

namespace QRDashboard.Infraestructure.Ioc
{
    public static class Dependencies
    {
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Conexion"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
        }
    }
}