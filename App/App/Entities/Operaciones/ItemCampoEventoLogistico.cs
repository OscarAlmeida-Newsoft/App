using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Entities.Operaciones
{
    public class ItemCampoEventoLogistico: EntidadApp
    {
        public int Id { get; set; }
        public int IdEvento { get; set; }
        public string CampoEventoLogistico { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public bool Activo { get; set; }
        public bool TieneSubItems { get; set; }
    }
}
