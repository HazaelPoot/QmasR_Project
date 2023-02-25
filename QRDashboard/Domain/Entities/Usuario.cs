using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRDashboard.Domain.Entities
{
    public class Usuario
    {
        public int IdUser { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public int? AdminType { get; set; }
        public string UrlImagen { get; set; } = null!;

        public virtual AdminType? AdminTypeNavigation { get; set; }
    }
}