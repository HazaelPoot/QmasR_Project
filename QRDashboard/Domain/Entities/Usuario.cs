using System;
using System.Collections.Generic;

namespace QRDashboard.Domain.Entities
{
    public partial class Usuario
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
