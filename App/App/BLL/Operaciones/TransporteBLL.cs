using App.Common;
using App.DAO.Operaciones;
using App.Entities.Operaciones;
using CoreTDM.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Operaciones
{
    public class TransporteBLL
    {
        public Task<Transporte> SeleccionarTransporte(long numeroTransporte)
        {
            Task<Transporte> transporteTask;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            transporteTask = ds.RealizarPeticionApi<Transporte>("Transporte/ObtenerTransporte/" + numeroTransporte.ToString(), DataService.TipoPeticionApi.Get);
            return transporteTask;

        }
        public HistorialActivacionManifiesto SeleccionarHistorialManifiestoActivoPorConductor(string numeroIdentificacion)
        {
            HistorialActivacionManifiestoDAO dao = new HistorialActivacionManifiestoDAO();
            HistorialActivacionManifiesto ham = dao.SeleccionarHistorialManifiestoActivoPorConductor(numeroIdentificacion);

            return ham;
        }

        public Task<List<Transporte>> SeleccionarTransportesPendientesActivacionPorConductor()
        {
            Task<List<Transporte>> transportes;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);

            string url = "Transporte/ObtenerTransportesPendientesActivacionPorConductor";

            transportes = ds.RealizarPeticionApi<List<Transporte>>(url, DataService.TipoPeticionApi.Get);

            return transportes;
        }

        public Task<List<Transporte>> SeleccionarTransportesSuspendidosPorConductor()
        {
            Task<List<Transporte>> transportes;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);

            string url = "Transporte/ObtenerTransportesSuspendidosPorConductor";

            transportes = ds.RealizarPeticionApi<List<Transporte>>(url, DataService.TipoPeticionApi.Get);

            return transportes;
        }


        public Task<List<PuestoControlManifiesto>> ObtenerPuestosControlPendientesPorManifiesto(long numeroTransporte)
        {
            Task<List<PuestoControlManifiesto>> puestosControl;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            puestosControl = ds.RealizarPeticionApi<List<PuestoControlManifiesto>>("Transporte/ObtenerPuestosControlPendientesPorManifiesto/" + numeroTransporte.ToString(), DataService.TipoPeticionApi.Get);
            return puestosControl;

        }
    }
}
