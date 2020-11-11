using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTDM.Entities.Operaciones
{
    public class AdjuntoComentarioManifiesto
    {
        public long Codigo { get; set; }
        public Int64 NumeroManifiesto { get; set; }
        public int CodigoComentario { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string RutaArchivo { get; set; }

        public string Base64String { get; set; }

    }
}
