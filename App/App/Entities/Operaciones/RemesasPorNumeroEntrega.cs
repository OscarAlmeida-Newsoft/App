using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class RemesasPorNumeroEntrega: EntidadApp
    {
        public int NumeroManifiesto { get; set; }
        public int NumeroEntrega { get; set; }
        public string NombreCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public string PlacaVehiculo { get; set; }
    }
}
