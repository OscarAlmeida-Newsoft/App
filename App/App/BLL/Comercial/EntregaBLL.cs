using App.Common;
using App.DAO.Comercial;
using App.DAO.Operaciones;
using App.Entities.Comercial;
using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Comercial
{
    public class EntregaBLL
    {
        public List<Entrega> SeleccionarEntregasPorTransporteSinEventoLogistico(long numeroManifiesto, int idTipoEvento)
        {
            

            List<Entrega> entregas = new List<Entrega>();
            entregas = this.SeleccionarEntregasPorTransporte(numeroManifiesto);

            if(entregas!=null)
            {
                EventoLogisticoDAO eventoDAO = new EventoLogisticoDAO();
                //Se consultan los eventos registrados 
                List<EventoLogistico> eventos = new List<EventoLogistico>();
                eventos = eventoDAO.SeleccionarEventosLogisticos(numeroManifiesto);

                //se tiene en cuenta únicamente los eventos del tipo indicado
                var eventosPreviosRegistrados = (from e in eventos
                                                 where e.IdTipoEvento == idTipoEvento
                                                 && e.Estado == "S"
                                                 select e).DefaultIfEmpty().ToList();

                if (eventosPreviosRegistrados != null && eventosPreviosRegistrados[0] != null)
                {
                    var codigosEntregas = (from e in eventosPreviosRegistrados
                                           select e.campo1).ToList();

                    //se consulta las entregas a las que les la falta registrar el tipo de evento 
                    entregas = (from e in entregas
                                where !codigosEntregas.Contains(e.NumeroEntrega.ToString())
                                select e).ToList();
                }
            }
            

            return entregas;
        }

        public List<Entrega> SeleccionarEntregasPorTransporte(long numeroManifiesto)
        {
            EntregaDAO entregaDAO = new EntregaDAO();
            

            List<Entrega> entregas = new List<Entrega>();
            entregas = entregaDAO.SeleccionarEntregasPorTransporte(numeroManifiesto);
            return entregas;
        }

        public Entrega GuardarEntrega(Entrega entrega)
        {
            EntregaDAO entregaDAO = new EntregaDAO();
            return entregaDAO.GuardarEntrega(entrega);
        }


        public async Task<List<Entrega>> ObtenerEntregasDeContenedoresEnPatio(string puesto)
        {
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            var entregas = await ds.RealizarPeticionApi<List<Entrega>>("Entrega/ObtenerEntregasDeContenedoresEnPatio?puesto=" + puesto, DataService.TipoPeticionApi.Get);
            return entregas;
        }

        public async Task<string> GuardarImagenFirmaEntrega(EntregaDetalleFirma adjunto)
        {
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            await ds.RealizarPeticionApi<string>("Entrega/GuardarFirmaEntrega" , DataService.TipoPeticionApi.Post, adjunto);

            return "Exitoso";
        }

        public async Task<Entrega> ObtenerEntrega(int numeroEntrega) {

            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);

            List<int> listEntregas = new List<int>();
            listEntregas.Add(numeroEntrega);

            Entrega miEntrega = (await ds.RealizarPeticionApi<List<Entrega>>("Entrega/ObtenerEntregas", DataService.TipoPeticionApi.Post, listEntregas)).SingleOrDefault();

            return miEntrega;
        }
    }
}
