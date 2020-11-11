using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class HistorialActivacionManifiesto: EntidadApp
    {
        public Int64 NumeroManifiesto { get; set; }
        public string Placa { get; set; }
        public string NumeroDocConductor { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaActivacion { get; set; }
        public string UsuarioActivacion { get; set; }
        public DateTime? FechaInactivacion { get; set; }
        public string UsuarioInactivacion { get; set; }
        public string NombreRuta { get; set; }
        public string ClaseTransporte { get; set; }
        
    }
}
