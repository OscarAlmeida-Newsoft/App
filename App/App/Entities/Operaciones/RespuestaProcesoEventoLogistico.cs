using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class RespuestaProcesoEventoLogistico
    {
        public Int64 IdRespuesta { get; set; }
        public string Placa { get; set; }
        public string Respuesta { get; set; }
        public Int64 IdEvento { get; set; }
        public bool ProcesadoCorrectamente { get; set; }
        public int IdTipoEvento { get; set; }
    }
}
