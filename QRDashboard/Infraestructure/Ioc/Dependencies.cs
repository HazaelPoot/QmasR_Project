using Microsoft.EntityFrameworkCore;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Aplication.Services;
using QRDashboard.Infraestructure.Data;
using QRDashboard.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace QRDashboard.Infraestructure.Ioc
{
    public static class Dependencies
    {
        public static void InjectDependency(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Conexion"));
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("QmasRPolicy", app =>
                {
                    app.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddHttpContextAccessor();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt => {
                opt.Cookie.Name = "Q+R_Cookie";
                opt.LoginPath = "/Login/Index";
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(50);
            });

            services.AddAuthorization(options => {
                options.AddPolicy("Admin", policy => policy.RequireClaim("RolName", "Administrador"));
                options.AddPolicy("Super", policy => policy.RequireClaim("RolName", "Supervisor"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<IProyectoService, ProyectoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IUtilityService, UtilityService>();
            services.AddScoped<IAccesoService, AccesoService>(); 
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFotoService, FotoService>();
            services.AddScoped<IRolService, RolService>();
        }
    }
}