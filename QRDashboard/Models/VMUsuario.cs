using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRDashboard.Models
{
    public class VMUsuario
    {
        public int IdUser { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public int? AdminType { get; set; }
        public string AdminTypeName { get; set; } = null!;
        public string UrlImagen { get; set; } = null!;
    }
}