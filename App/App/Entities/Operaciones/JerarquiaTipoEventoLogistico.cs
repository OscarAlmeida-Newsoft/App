using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class JerarquiaTipoEventoLogistico
    {
        public int CodigoEvento { get; set; }
        public int CodigoEventoPredecesor { get; set; }

    }
}
