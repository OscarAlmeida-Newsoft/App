using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class SubItemCampoEventoLogistico
    {
        public int IdSubItem { get; set; }
        public int IdItem { get; set; }
        public string CampoEventoLogistico { get; set; }
        public string Titulo { get; set; }
        public string TipoCampo { get; set; }
        public int Orden { get; set; }
        public string Propiedades { get; set; }
    }
}
