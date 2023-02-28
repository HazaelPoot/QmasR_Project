using System;
using System.Collections.Generic;

namespace QRDashboard.Domain.Entities
{
    public partial class FotosProyecto
    {
        public int IdImg { get; set; }
        public string Descripcion { get; set; } = null!;
        public string UrlImage { get; set; } = null!;
        public int? IdProj { get; set; }

        public virtual ProyectoQr? IdProjNavigation { get; set; }
    }
}
