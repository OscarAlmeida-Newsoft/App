using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreTDM.Entities.Operaciones
{
    public class PuestoControlManifiesto
    {
        public Int64 CodigoManifiesto { get; set; }

        public string CodigoPuestoControl { get; set; }

        public int Orden { get; set; }

        public string NombrePuestoControl { get; set; }

        public int Horas { get; set; }

        public int Minutos { get; set; }

        public DateTime FechaEstimada { get; set; }

        public DateTime? FechaReal { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string Usuario { get; set; }

        public bool? NoReportado { get; set; }




        public PuestoControlManifiesto()
        {
        }
    }
}
