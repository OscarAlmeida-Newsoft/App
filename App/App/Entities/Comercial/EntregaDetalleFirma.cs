using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Comercial
{
    public class EntregaDetalleFirma
    {
        public int NumeroEntrega { get; set; }
        public string NumeroDocConductor { get; set; }
        public string Observaciones { get; set; }
        public bool PdfCombinado { get; set; }

        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string RutaArchivo { get; set; }

        public string Base64String { get; set; }
    }
}
