using System.Security.Claims;
using QRDashboard.Domain.Dtos;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace QRDashboard.Aplication.Services
{
    public class AccesoService : IAccesoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericRepository<Usuario> _repository;
        private readonly IUtilityService _utilityService;
        public AccesoService(IGenericRepository<Usuario> repository, IUtilityService utilityService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _utilityService = utilityService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Usuario> Authentication(string username, string password)
        {
            Usuario user_encontrado = await _repository.Obtain(u => u.Username.Equals(username));

            if(user_encontrado is null)
                throw new TaskCanceledException($"No existe el Usuario {username}");

            var userPassword = _utilityService.DesencryptMD5(user_encontrado.Passw);
            if(userPassword != password)
                throw new TaskCanceledException("La contraseña es incorrecta");

            return user_encontrado;
        }

        public async Task<Usuario> GetUserAuth(int IdUsuario)
        {
            IQueryable<Usuario> query = await _repository.Consult(u => u.IdUser == IdUsuario);
            Usuario result = query.Include(r => r.AdminTypeNavigation).FirstOrDefault();

            return result;
        }

        public async Task<bool> GenerateClaims(DtoUsuario user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Role, user.AdminType.ToString()),
                new Claim("RolName", user.AdminTypeName),
                new Claim("Profile", user.UrlImagen),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(50)
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            return true;
        }
    }
}