using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRDashboard.Domain.Entities
{
    public class ProyectoQr
    {
        public ProyectoQr()
        {
            FotosProyectos = new HashSet<FotosProyecto>();
        }

        public int IdProj { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime FinaliDatre { get; set; }
        public string Ubicacion { get; set; } = null!;
        public decimal? Presupuesto { get; set; }

        public virtual ICollection<FotosProyecto> FotosProyectos { get; set; }
    }
}