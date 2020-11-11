using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Turnos
{
    public class Vehiculo
    {      
        public string Placa { get; set; }
        public List<CaracteristicaVehiculo> Caracteristicas { get; set; }
      
        public DateTime? FechaPosicion { get; set; }
        public int? MinutosTolerancia { get; set; }
    }
}
