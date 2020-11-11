using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class RemesasPorNumeroTransporte
    {
        public int NumeroRemesa { get; set; }
        public int NumeroManifiesto { get; set; }
        public string NombreCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public DateTime? FechaProgramadaCargue { get; set; }
        public DateTime? FechaProgramadaDescargue { get; set; }
    }
}
