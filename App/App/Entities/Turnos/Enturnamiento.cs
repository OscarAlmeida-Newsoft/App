using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Turnos
{
    public class Enturnamiento
    {
        public long IdTurno { get; set; }

        public string PuestoExpedicion { get; set; }
        public string DescripcionPuestoExpedicion { get; set; }

        public string PlacaTrailer { get; set; }
        public string PlacaCabezote { get; set; }
        public string NumeroDocConductor { get; set; }
        public string Usuario { get; set; }
        public string NumeroTransporte { get; set; }
        public string ClaseTrailer { get; set; }
        public string TipoTrailer1 { get; set; }
        public string TipoTrailer2 { get; set; }
        public string Estado { get; set; }
        public string DescripcionEstado { get; set; }
        public string Destino { get; set; }
        public int? Turno { get; set; }

        public DateTime? FechaDisponible { get; set; }
        public DateTime FechaCreacion { get; set; }




        public string CodigoTipoTrailer { get; set; }
        public string TipoTrailer { get; set; }
        public string NombreConductor { get; set; }

        public string TipoVehiculo { get; set; }
        public string PropiedadVehiculo { get; set; }

        public string ConfiguracionVehicular { get; set; }

        public string CategoriaVehiculo { get; set; }
    }
}
