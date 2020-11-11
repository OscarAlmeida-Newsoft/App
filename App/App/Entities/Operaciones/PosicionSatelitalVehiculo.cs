using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class PosicionSatelitalVehiculo
    {
        public long Codigo { get; set; }
        public string Placa { get; set; }
        public DateTime FechaPosicion { get; set; }
        public long? NumeroManifiesto { get; set; }
        public float Latitud { get; set; }
        public float Longitud { get; set; }
        public string DescripcionPosicion { get; set; }
        public float? Distancia { get; set; }
        public int? Orientacion { get; set; }
        public float? NivelMar { get; set; }
        public float? Velocidad { get; set; }
        public string NombreGeocerca { get; set; }
        public string TipoGeocerca { get; set; }
        public string ProveedorGPS { get; set; }
        public string NombreEvento { get; set; }
        public string ValorEvento { get; set; }

        public string Ciudad { get; set; }
        public string Departamento { get; set; }
        public string Pais { get; set; }

        public int? Odometro { get; set; }
        public int? RPM { get; set; }
        public float? NivelCombustible { get; set; }

        public bool Procesado { get; set; }
        public DateTime? FechaProcesado { get; set; }
        public string ObservacionesProcesado { get; set; }
    }
}
