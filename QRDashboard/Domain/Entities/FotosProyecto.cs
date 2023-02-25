using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRDashboard.Domain.Entities
{
    public class FotosProyecto
    {
        public int IdImg { get; set; }
        public string Descripcion { get; set; } = null!;
        public string UrlImage { get; set; } = null!;
        public int? IdProj { get; set; }

        public virtual ProyectoQr? IdProjNavigation { get; set; }
    }
}