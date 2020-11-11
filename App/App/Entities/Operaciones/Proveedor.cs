using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class Proveedor
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string Grupo { get; set; }
        public string NumIdentificacionFiscal { get; set; }
        public string TipoIdentificacionFiscal { get; set; }
        public string FuncionBloqueo { get; set; }
    }
}
