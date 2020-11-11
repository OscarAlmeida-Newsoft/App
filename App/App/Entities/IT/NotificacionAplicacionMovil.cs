using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.IT
{
    public class NotificacionAplicacionMovil
    {
        public long Codigo { get; set; }
        public DateTime Fecha { get; set; }
        public char Estado { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public string CodigoAplicacion { get; set; }
        public int CodigoTipoNotificacion { get; set; }

        public string Referencia1 { get; set; }

        public Dictionary<string, string> DataAdicional { get; set; }
        public List<BotonNotificacionAplicacionMovil> Botones { get; set; }
        
    }
}
