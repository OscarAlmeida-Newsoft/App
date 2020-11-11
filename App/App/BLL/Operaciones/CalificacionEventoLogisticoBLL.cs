using App.Common;
using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Operaciones
{
    public class CalificacionEventoLogisticoBLL
    {
        public Task<List<CalificacionEventoLogisticoManifiesto>> ObtenerCalificacionesEventosLogisticos(DateTime fechaInicial, DateTime fechaFinal,string numeroDocConductor)
        {
            Task<List<CalificacionEventoLogisticoManifiesto>> calificaciones;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);

            string url = "Evento/ObtenerCalificacionesEventosLogisticos?fechaInicial="+ fechaInicial + "&fechaFinal="+ fechaFinal + "&numeroDocConductor="+ numeroDocConductor;

            calificaciones = ds.RealizarPeticionApi<List<CalificacionEventoLogisticoManifiesto>>(url, DataService.TipoPeticionApi.Get);
            return calificaciones;
        }
    }
}
