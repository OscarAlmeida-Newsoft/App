using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class CalificacionEventoLogisticoManifiesto
    {
        public int Id { get; set; }
        public long NumeroManifiesto { get; set; }
        public float CalificacionAutomatica { get; set; }
        public float CalificacionManual { get; set; }
       
        public DateTime FechaCalificacion { get; set; }      
        public string ObservacionCalificacionAutomatica { get; set; }
        public string ObservacionCalificacionManual { get; set; }
    
        public string NumeroDocConductor { get; set; }
        public string NombreConductor { get; set; }
        public string Placa { get; set; }

     
    }
}
