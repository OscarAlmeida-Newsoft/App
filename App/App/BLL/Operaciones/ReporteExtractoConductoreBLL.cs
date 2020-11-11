using App.Common;
using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BLL.Operaciones
{
    public class ReporteExtractoConductoreBLL
    {
        public async Task<List<ReporteExtractoConductor>> ObtenerExtractosPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            List<ReporteExtractoConductor> extractos = new List<ReporteExtractoConductor>();
            if (await ParametrosSistema.isOnline)
            {              
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
                extractos = await ds.RealizarPeticionApi<List<ReporteExtractoConductor>>("Conductor/ObtenerExtractosPorConductoryFecha?FechaInicial="+fechaInicial+ "&FechaFinal="+fechaFinal, DataService.TipoPeticionApi.Get);
            }
            //if (extractos != null)
            //{
            //    foreach (ReporteExtractoConductor extracto in extractos)
            //    {
            //        if (extracto.Diferencia > 0)
            //            extracto.colorDiferencia = "Green";
            //        else
            //            extracto.colorDiferencia = "Red";
            //    }
            //}
            return extractos;
        }
    }
}
