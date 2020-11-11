using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class ReporteExtractoConductor
    {
        public long NumeroTransporte { get; set; }
        public string NombreConductor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Anulado { get; set; }
        public string Placa { get; set; }
        public string Ruta { get; set; }
        public double Anticipo { get; set; }
        public double ReAnticipo { get; set; }
        public double L1Descontado { get; set; }
        public string TransportesAsignacion { get; set; }
        public double TotalporLegalizar { get; set; }
        public double TotalLegalizado { get; set; }
        public double Diferencia { get; set; }
        public string colorDiferencia { get; set; }
    }
}
