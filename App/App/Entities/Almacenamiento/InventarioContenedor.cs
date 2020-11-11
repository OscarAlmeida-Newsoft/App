using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Almacenamiento
{
    public class InventarioContenedor
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Puesto { get; set; }
        public string UsuarioCreacion { get; set; }

        public List<DetalleInventarioContenedor> Detalles { get; set; }

    }
}
