using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Almacenamiento
{
    public class DetalleInventarioContenedor
    {
        public long Id { get; set; }
        public long CodigoInventarioContenedor { get; set; }
        public string NumeroContenedor { get; set; }
        public string EstadoContenedor { get; set; }
        public int TamanoContenedor { get; set; }
    }
}
