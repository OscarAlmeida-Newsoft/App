using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class AdjuntoEventoLogistico
    {
        public long IDAdjunto { get; set; }
        public Int64 NumeroManifiesto { get; set; }
        public long IDEvento { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string RutaArchivo { get; set; }

        public string Base64String { get; set; }
    }
}
