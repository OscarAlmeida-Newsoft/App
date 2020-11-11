using App.Common;
using App.DAO.Comercial;
using App.DAO.Operaciones;
using App.Entities.Operaciones;
using App.Entities.Comercial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.BLL.Comercial;

namespace App.BLL.Operaciones
{
    public class EventoLogisticoBLL : AppBLLBase
    {       

        public async Task<List<TipoEventoLogistico>> SeleccionarSiguienteEventoporManifiesto(Int64 numeroManifiesto, bool consultaLocal= false)
        {
            List<TipoEventoLogistico> eventosSucesores = new List<TipoEventoLogistico>();
            Util util = new Util();
            bool? usuarioTipoTercero = null;
            if (util.UsuarioTienePermiso("eventoslogisticosterceros"))
            {
                usuarioTipoTercero = true;
            }

            if (consultaLocal)
            {
                string numeroIdentificacionConductor = string.Empty;
                List<TipoEventoLogistico> tipoEventos = new List<TipoEventoLogistico>();                
                List<EventoLogistico> eventosRegistrados = new List<EventoLogistico>();
                
                EventoLogisticoDAO dao = new EventoLogisticoDAO();
                List<JerarquiaTipoEventoLogistico> jerarquiaTipoEventos = new List<JerarquiaTipoEventoLogistico>();

                EventoLogistico ultimoEventoRegistrado = null;
                TipoEventoLogistico ultimoTipoEvento = new TipoEventoLogistico();
                tipoEventos = await this.SeleccionarTiposEventoLogistico(consultaLocal: true, aplicaTerceros:usuarioTipoTercero);

                if ((usuarioTipoTercero.HasValue && !usuarioTipoTercero.Value) || !usuarioTipoTercero.HasValue)
                {
                    //Se consulta si tiene jornada laboral activa
                    var eventosInicioJornada = dao.SeleccionarEventosLogisticos(numeroManifiesto: null, estado: null, codigoTipoEvento: (int)TipoEventoLogisticoEnum.InicioJornada);
                    var eventosFinJornada = dao.SeleccionarEventosLogisticos(numeroManifiesto: null, estado: null, codigoTipoEvento: (int)TipoEventoLogisticoEnum.FinJornada);
                    EventoLogistico ultimoInicioJornada = null;
                    EventoLogistico ultimoFinJornada = null;
                    if (eventosInicioJornada != null && eventosInicioJornada.Count > 0)
                    {
                        eventosInicioJornada = eventosInicioJornada.OrderByDescending(e => e.FechaEvento).ToList();
                        ultimoInicioJornada = eventosInicioJornada.DefaultIfEmpty(null).Where(e => String.IsNullOrEmpty(e.Estado) || e.Estado != "E").FirstOrDefault();
                    }
                    if (eventosFinJornada != null && eventosFinJornada.Count > 0)
                    {
                        eventosFinJornada = eventosFinJornada.OrderByDescending(e => e.FechaEvento).ToList();
                        ultimoFinJornada = eventosFinJornada.DefaultIfEmpty(null).Where(e => String.IsNullOrEmpty(e.Estado) || e.Estado != "E").FirstOrDefault();
                    }
                    bool existeJornadaLaboral = false;
                    if (ultimoInicioJornada != null)
                    {
                        if (ultimoFinJornada == null)
                        {
                            existeJornadaLaboral = true;
                        }
                        else
                        {
                            //SI existe algún evento de fin jornada registrado, se debe validar que sea anterior al ultimo inicio de jornada
                            if (ultimoFinJornada.FechaEvento < ultimoInicioJornada.FechaEvento)
                            {
                                existeJornadaLaboral = true;
                            }
                        }
                    }

                    if (existeJornadaLaboral == false)
                    {
                        //Si no tiene jornada laboral activa se le muestra únicamente el evento de inicio jornada
                        eventosSucesores = tipoEventos.Where(t => t.CodigoEvento == (int)TipoEventoLogisticoEnum.InicioJornada ||
                           t.CodigoEvento == (int)TipoEventoLogisticoEnum.Otro).ToList();
                        return eventosSucesores;
                    }
                }
                if (numeroManifiesto == 0)
                {
                    eventosSucesores = dao.SeleccionarEventosSucesores(0);
                    if (eventosSucesores != null)
                    {
                        eventosSucesores = eventosSucesores.Where(e => e.TransporteObligatorio == false).ToList();
                    }
                }
                else
                {
                    eventosRegistrados = SeleccionarEventosLogisticos(numeroManifiesto, consultaLocal:true, estado:null);
                    
                    
                    jerarquiaTipoEventos = await SeleccionarJerarquiaTipoEventosLogisticos(consultaLocal: true);

                    if (eventosRegistrados != null && eventosRegistrados.Count > 0)
                    {
                        //Se excluyen los eventos con error
                        eventosRegistrados = eventosRegistrados.Where(e => String.IsNullOrEmpty(e.Estado) || e.Estado != "E").OrderByDescending(e => e.FechaEvento).ToList();
                        
                        ultimoEventoRegistrado = (from e in eventosRegistrados
                                                  select e).FirstOrDefault();
                    }
                    if (ultimoEventoRegistrado != null)
                    {
                        eventosSucesores = ValidarEventosLogisticosSucesoresPorUltimoEvento(ultimoEventoRegistrado, eventosRegistrados, tipoEventos, numeroManifiesto);

                        if (eventosSucesores != null && eventosSucesores.Count > 0)
                            return eventosSucesores;

                        ultimoTipoEvento = (from t in tipoEventos
                                            where t.CodigoEvento == ultimoEventoRegistrado.IdTipoEvento
                                            select t).FirstOrDefault();

                        if (ultimoTipoEvento.EventoTipoOtros && eventosRegistrados.Count == 1)
                        {
                            //Solo se ha registrado un evento y es de tipo "Otros"
                            eventosSucesores = dao.SeleccionarEventosSucesores(0);
                        }
                        else if (ultimoTipoEvento.EventoTipoOtros && eventosRegistrados.Count > 1)
                        {
                            List<EventoLogistico> eventosRegistradosTipoOtros = new List<EventoLogistico>();
                            //Se valida si el anterior evento registrado es de tipo otros con restriccion 
                            //7,14,19: Parada, Entrada Taller, Inicio jornada respectivamente                    
                            eventosRegistradosTipoOtros = (from e in eventosRegistrados
                                                           join t in tipoEventos on e.IdTipoEvento equals t.CodigoEvento
                                                           where t.EventoTipoOtros && ParametrosSistema.EventoTipoOtroconRestriccion.Contains(t.CodigoEvento)
                                                           orderby e.FechaEvento descending
                                                           select e).ToList();
                            if (eventosRegistradosTipoOtros != null && eventosRegistradosTipoOtros.Count > 0)
                            {
                                var grupoEventosRegistradosTipoOtros = eventosRegistradosTipoOtros.GroupBy(e => e.IdTipoEvento);

                                foreach (IGrouping<int, EventoLogistico> grupo in grupoEventosRegistradosTipoOtros)
                                {
                                    int cantidad = grupo.Count();
                                    var eventoTipoRestriccion = (from j in jerarquiaTipoEventos
                                                                 where j.CodigoEventoPredecesor == grupo.Key
                                                                 select j).FirstOrDefault();
                                    if (eventoTipoRestriccion != null)
                                    {
                                        int cantidadeventoTipoRestriccion = (from e in eventosRegistrados
                                                                             where e.IdTipoEvento == eventoTipoRestriccion.CodigoEvento
                                                                             select e).Count();
                                        if (cantidad > cantidadeventoTipoRestriccion)
                                        {
                                            eventosSucesores = dao.SeleccionarEventosSucesores(grupo.Key);
                                        }
                                    }
                                }

                                if (eventosSucesores.Count() == 0)
                                {
                                    //eventosRegistrados = eventosRegistrados.OrderByDescending(x => x.FechaEvento).OrderByDescending(x => x.ID).ToList();
                                    eventosRegistrados = eventosRegistrados.OrderByDescending(x => x.FechaEvento).ToList();
                                    ultimoEventoRegistrado = null;
                                    ultimoEventoRegistrado = (from e in eventosRegistrados
                                                              join t in tipoEventos on e.IdTipoEvento equals t.CodigoEvento
                                                              where !t.EventoTipoOtros
                                                              select e).FirstOrDefault();
                                    if (ultimoEventoRegistrado != null)
                                    {
                                        eventosSucesores = ValidarEventosLogisticosSucesoresPorUltimoEvento(ultimoEventoRegistrado, eventosRegistrados, tipoEventos, numeroManifiesto);

                                        if (eventosSucesores == null || eventosSucesores.Count == 0)
                                            eventosSucesores = dao.SeleccionarEventosSucesores(ultimoEventoRegistrado.IdTipoEvento);

                                    }
                                    else
                                    {
                                        eventosSucesores = dao.SeleccionarEventosSucesores(0);
                                    }
                                }

                            }
                            else
                            {
                                eventosRegistrados = eventosRegistrados.OrderByDescending(x => x.FechaEvento).OrderByDescending(x => x.ID).ToList();
                                //Se busca el ultimo evento registrado que no sea de tipo otros
                                ultimoEventoRegistrado = (from e in eventosRegistrados
                                                          join t in tipoEventos on e.IdTipoEvento equals t.CodigoEvento
                                                          where !t.EventoTipoOtros
                                                          select e).FirstOrDefault();
                                if (ultimoEventoRegistrado != null)
                                {
                                    eventosSucesores = ValidarEventosLogisticosSucesoresPorUltimoEvento(ultimoEventoRegistrado, eventosRegistrados, tipoEventos, numeroManifiesto);

                                    if (eventosSucesores == null || eventosSucesores.Count == 0)
                                        eventosSucesores = dao.SeleccionarEventosSucesores(ultimoEventoRegistrado.IdTipoEvento);

                                }
                                else
                                {
                                    eventosSucesores = dao.SeleccionarEventosSucesores(0);
                                }
                            }
                        }
                        else
                        {
                            //se consulta los eventos sucesores del ultimo evento registrado
                            eventosSucesores = ValidarEventosLogisticosSucesoresPorUltimoEvento(ultimoEventoRegistrado, eventosRegistrados, tipoEventos, numeroManifiesto);

                            if (eventosSucesores == null || eventosSucesores.Count == 0)
                                eventosSucesores = dao.SeleccionarEventosSucesores(ultimoEventoRegistrado.IdTipoEvento);

                        }
                    }
                    else
                    {
                        //No se ha registrado ningun evento al viaje
                        eventosSucesores = dao.SeleccionarEventosSucesores(0);
                    }
                }

                if (eventosSucesores != null)
                {
                    eventosSucesores = eventosSucesores.OrderBy(t => t.NombreEvento).ToList();
                }
                return eventosSucesores;
            }

            else
            {                
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
                eventosSucesores = await ds.RealizarPeticionApi<List<TipoEventoLogistico>>("Evento/SeleccionarSiguienteEventoporManifiesto?numeromanifiesto=" + numeroManifiesto.ToString()+"&aplicaTerceros="+ usuarioTipoTercero, DataService.TipoPeticionApi.Get);//.ConfigureAwait(false);

            }
            return eventosSucesores;

        }

        private List<TipoEventoLogistico> ValidarEventosLogisticosSucesoresPorUltimoEvento(EventoLogistico ultimoEventoRegistrado, List<EventoLogistico> eventosRegistrados, List<TipoEventoLogistico> tipoEventos, long numeroManifiesto)
        {
            List<TipoEventoLogistico> eventosSucesores = new List<TipoEventoLogistico>();

            //Si el ultimo evento registrado fue activación, valida si es un vacio para mostrar el evento inicio de viaje
            if (ultimoEventoRegistrado.IdTipoEvento == (int)TipoEventoLogisticoEnum.ActivarViaje)
            {
                HistorialActivacionManifiestoDAO historialActivacionDAO = new HistorialActivacionManifiestoDAO();
                HistorialActivacionManifiesto transporte = historialActivacionDAO.SeleccionarHistorialActivacionManifiesto(ultimoEventoRegistrado.NumeroManifiesto);
                if (transporte != null)
                {
                    if (transporte.ClaseTransporte == "ZT17" || transporte.ClaseTransporte == "ZT18")
                    {
                        eventosSucesores = tipoEventos.Where(t => t.CodigoEvento == (int)TipoEventoLogisticoEnum.IniciaViaje ||
                                                    t.CodigoEvento == (int)TipoEventoLogisticoEnum.Otro).ToList();
                        return eventosSucesores;
                    }
                }
            }

            //si tiene inicios de cargue sin fin cargue, muestra solo fin cargue 
            if (ultimoEventoRegistrado.IdTipoEvento == (int)TipoEventoLogisticoEnum.IniciaCargue)
            {
                eventosSucesores = tipoEventos.Where(t => t.CodigoEvento == (int)TipoEventoLogisticoEnum.FinCargue ||
                                                t.CodigoEvento == (int)TipoEventoLogisticoEnum.Otro).ToList();
                return eventosSucesores;
            }
            
            //si valida tiene mas entregas pendientes por registrarle datos de cargue
            EntregaDAO entregaDAO = new EntregaDAO();
            List<Entrega> entregasManifiesto = entregaDAO.SeleccionarEntregasPorTransporte(numeroManifiesto);
            
            if (ultimoEventoRegistrado.IdTipoEvento == (int)TipoEventoLogisticoEnum.FinCargue)
            {
                

                int entregasCargadas = 0;
                foreach (Entrega entrega in entregasManifiesto)
                {
                    var llegadaCargue = (from e in eventosRegistrados
                                         where e.IdTipoEvento == (int)TipoEventoLogisticoEnum.LlegadaCargue
                                         where e.campo1 == entrega.NumeroEntrega.ToString()
                                         select e).ToList();
                    var finCargue = (from e in eventosRegistrados
                                     where e.IdTipoEvento == (int)TipoEventoLogisticoEnum.FinCargue
                                     where e.campo1 == entrega.NumeroEntrega.ToString()
                                     select e).ToList();
                    if (llegadaCargue != null && finCargue != null)
                        entregasCargadas += 1;
                }
                if (entregasCargadas < entregasManifiesto.Count)
                {
                    eventosSucesores = tipoEventos.Where(t => t.CodigoEvento == (int)TipoEventoLogisticoEnum.LlegadaCargue ||
                                       t.CodigoEvento == (int)TipoEventoLogisticoEnum.Otro).ToList();
                    return eventosSucesores;
                }
            }
            //si tiene inicios de descargue sin fin descargue, muestra solo fin descargue 
            if (ultimoEventoRegistrado.IdTipoEvento == (int)TipoEventoLogisticoEnum.IniciaDescargue)
            {
                eventosSucesores = tipoEventos.Where(t => t.CodigoEvento == (int)TipoEventoLogisticoEnum.FinDescargue ||
                                                 t.CodigoEvento == (int)TipoEventoLogisticoEnum.Otro).ToList();
                return eventosSucesores;
            }
            //si valida tiene mas entregas pendientes por registrarle datos de descargue
            if (ultimoEventoRegistrado.IdTipoEvento == (int)TipoEventoLogisticoEnum.FinDescargue)
            {
                bool existenEntregasSinDescargar = false;
                

                int entregasDescargadasoDesegachadas = 0;

                foreach (Entrega entrega in entregasManifiesto)
                {
                    var llegadaDescargue = (from e in eventosRegistrados
                                            where e.IdTipoEvento == (int)TipoEventoLogisticoEnum.LlegadaDestino
                                            where e.campo1 == entrega.NumeroEntrega.ToString()
                                            select e).FirstOrDefault();
                    var finDescargue = (from e in eventosRegistrados
                                        where e.IdTipoEvento == (int)TipoEventoLogisticoEnum.FinDescargue
                                        where e.campo1 == entrega.NumeroEntrega.ToString()
                                        select e).FirstOrDefault();

                    if (llegadaDescargue != null && finDescargue != null)
                        entregasDescargadasoDesegachadas = entregasDescargadasoDesegachadas + 1;
                    else
                    {
                        var desenganche = (from e in eventosRegistrados
                                           where e.IdTipoEvento == (int)TipoEventoLogisticoEnum.Desenganche
                                           select e).FirstOrDefault();

                        if (desenganche != null)
                        {
                            int remesesasDesengachadas = 0;
                            if (int.TryParse(desenganche.campo1, out remesesasDesengachadas))
                                entregasDescargadasoDesegachadas = entregasDescargadasoDesegachadas + remesesasDesengachadas;
                        }
                    }
                }
                if (entregasDescargadasoDesegachadas < entregasManifiesto.Count())
                {
                    existenEntregasSinDescargar = true;
                }

                if (existenEntregasSinDescargar)
                {
                    eventosSucesores = tipoEventos.Where(t => t.CodigoEvento == (int)TipoEventoLogisticoEnum.LlegadaDestino ||
                                       t.CodigoEvento == (int)TipoEventoLogisticoEnum.Otro).ToList();
                    return eventosSucesores;
                }
            }
            //si tiene inicios de paradas sin continuacion viaje, muestra solo eventos que puede registrar en esta parado
            if (ultimoEventoRegistrado.IdTipoEvento == (int)TipoEventoLogisticoEnum.ParadaCamino)
            {
                eventosSucesores = tipoEventos.Where(t => t.CodigoEvento == (int)TipoEventoLogisticoEnum.ContinuacionViaje ||
                                        t.CodigoEvento == (int)TipoEventoLogisticoEnum.Otro).ToList();
                return eventosSucesores;
            }

            return eventosSucesores;
        }


        public Task<List<Transporte>> SeleccionarTransporteHabilitadoRegistroEventos(bool consultaLocal = true, bool? tercero = null)
        {
            Task<List<Transporte>> transportesTask = null;
            if (consultaLocal)
            {
                List<Transporte> transportes = new List<Transporte>();
                EventoLogisticoDAO dao = new EventoLogisticoDAO();
               
                var eventosActivacionViaje = dao.SeleccionarEventosLogisticos(null, estado: null, codigoTipoEvento: (int)TipoEventoLogisticoEnum.ActivarViaje);
                var eventosInicioViaje = dao.SeleccionarEventosLogisticos(null, estado: null, codigoTipoEvento: (int)TipoEventoLogisticoEnum.IniciaViaje);
                var eventosFinViaje = dao.SeleccionarEventosLogisticos(null, estado: null, codigoTipoEvento: (int)TipoEventoLogisticoEnum.FinViaje);

                if (tercero.HasValue && tercero.Value)
                {
                    if (eventosInicioViaje != null && eventosInicioViaje.Count > 0)
                    {
                        //Si existen eventos de inicio viaje
                        eventosInicioViaje = eventosInicioViaje.OrderByDescending(e => e.FechaEvento).ToList();
                        if (eventosFinViaje == null || eventosFinViaje.Count <= 0)
                        {
                            //Si no existen eventos de fin viaje
                            Transporte transporte = new Transporte() { NumeroTransporte = eventosInicioViaje.First().NumeroManifiesto, NumeroDocConductor= Convert.ToInt32(eventosInicioViaje.First().NumeroDocumentoConductor), Placa= eventosInicioViaje.First().Placa };
                            transportes.Add(transporte);

                        }
                        else
                        {
                            //SI existen eventos de fin viaje
                            eventosFinViaje = eventosFinViaje.OrderByDescending(e => e.FechaEvento).ToList();
                            if (eventosInicioViaje.First().FechaEvento > eventosFinViaje.First().FechaEvento)
                            {

                                Transporte transporte = new Transporte() { NumeroTransporte = eventosInicioViaje.First().NumeroManifiesto, NumeroDocConductor = Convert.ToInt32(eventosInicioViaje.First().NumeroDocumentoConductor), Placa = eventosInicioViaje.First().Placa };
                                transportes.Add(transporte);

                            }
                        }
                    }
                }
                else
                {
                    if (eventosActivacionViaje != null && eventosActivacionViaje.Count > 0)
                    {
                        //Si existen eventos de activación viaje
                        eventosActivacionViaje = eventosActivacionViaje.OrderByDescending(e => e.FechaEvento).ToList();
                        if (eventosFinViaje == null || eventosFinViaje.Count <= 0)
                        {
                            //Si no existen eventos de fin viaje
                            Transporte transporte = new Transporte() { NumeroTransporte = eventosActivacionViaje.First().NumeroManifiesto };
                            transportes.Add(transporte);

                        }
                        else
                        {
                            //SI existen eventos de fin viaje
                            eventosFinViaje = eventosFinViaje.OrderByDescending(e => e.FechaEvento).ToList();
                            if (eventosActivacionViaje.First().FechaEvento > eventosFinViaje.First().FechaEvento)
                            {

                                Transporte transporte = new Transporte() { NumeroTransporte = eventosActivacionViaje.First().NumeroManifiesto , Placa = eventosActivacionViaje.First().Placa};
                                transportes.Add(transporte);

                            }
                        }
                    }
                }

                transportesTask =Task.FromResult(transportes);
                
            }
            else
            {
                
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
                if(tercero.HasValue && tercero.Value)
                    transportesTask = ds.RealizarPeticionApi<List<Transporte>>("Evento/SeleccionarTransportesHabilitadosRegistroEventos?tercero="+tercero.Value, DataService.TipoPeticionApi.Get);
                else
                    transportesTask = ds.RealizarPeticionApi<List<Transporte>>("Evento/SeleccionarTransportesHabilitadosRegistroEventos", DataService.TipoPeticionApi.Get);
            }
            return transportesTask;


        }

        public Task<List<CampoEventoLogistico>> SeleccionarCamposPorEvento(int? codigoEvento, bool consultaLocal = true)
        {
            Task<List<CampoEventoLogistico>> campos;
            if(consultaLocal)
            {
                EventoLogisticoDAO dao = new EventoLogisticoDAO();
                campos = Task.FromResult(dao.SeleccionarCamposPorEvento(codigoEvento));
            }
            else
            {
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
                string url = "Evento/SeleccionarCamposporEvento";
                if (codigoEvento.HasValue)
                    url += "?TipoEvento=" + codigoEvento.Value.ToString();
                campos = ds.RealizarPeticionApi<List<CampoEventoLogistico>>(url, DataService.TipoPeticionApi.Get);//.ConfigureAwait(false); 
                
            }
            return campos;

        }

        public async Task<RespuestaProcesoEventoLogistico> GuardarEventoLogistico(EventoLogistico evento)
        {
            RespuestaProcesoEventoLogistico respuesta = new RespuestaProcesoEventoLogistico();

            


            if (await ParametrosSistema.isOnline)
            {
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
                respuesta = await ds.RealizarPeticionApi<RespuestaProcesoEventoLogistico>("Evento/GuardarEventoLogistico", DataService.TipoPeticionApi.Post, evento);
                evento.ID = respuesta.IdEvento;
                evento.Sincronizado = true;
                evento.FechaSincronizacion = DateTime.Now;
                if (respuesta.ProcesadoCorrectamente == false)
                {
                    evento.Estado = "E";
                    evento.ErrorSincronizacion = respuesta.Respuesta;
                }
                else
                {
                    evento.Estado = "S";
                }
            }
            else
            {
                evento.Sincronizado = false;
                respuesta.ProcesadoCorrectamente = true;
                respuesta.Respuesta = "Se ha guardado el evento en la memoria interna del teléfono.";
            }

            //Se determina si se debe guardar el evento en la base de datos local            
            EventoLogisticoDAO eventoDAO = new EventoLogisticoDAO();
            eventoDAO.GuardarEventoLogistico(evento);

            //Si es un evento de activación se debe registrar el manifiesto en el historial de activaciones
            if (evento.IdTipoEvento == (int)TipoEventoLogisticoEnum.ActivarViaje)
            {
                if (respuesta.ProcesadoCorrectamente == true)
                {
                    Transporte transporte = new Transporte();
                    HistorialActivacionManifiesto historial = new HistorialActivacionManifiesto();

                    historial.Sincronizado = true;
                    historial.FechaSincronizacion = DateTime.Now;
                    historial.Activo = true;
                    historial.FechaActivacion = evento.FechaEvento;
                    historial.UsuarioActivacion = ParametrosSistema.UsuarioActual;
                    TransporteBLL transporteBLL = new TransporteBLL();
                    transporte = await transporteBLL.SeleccionarTransporte(evento.NumeroManifiesto);
                    //if (await ParametrosSistema.isOnline)
                    //{
                    //    historial.Sincronizado = true;
                    //    historial.FechaSincronizacion = DateTime.Now;

                    //    TransporteBLL transporteBLL = new TransporteBLL();
                    //    transporte = await transporteBLL.SeleccionarTransporte(evento.NumeroManifiesto);

                    //}
                    //else
                    //{
                    //    historial.Sincronizado = false;
                    //    transporte.NumeroDocConductor = Convert.ToInt32(ParametrosSistema.NumeroIdentificacionUsuarioActual);
                    //    transporte.NumeroTransporte = evento.NumeroManifiesto;
                    //}


                    //se guarda un registro en historial activación viaje
                    if (transporte != null)
                    {
                        historial.Placa = transporte.Placa;
                        historial.NumeroDocConductor = transporte.NumeroDocConductor.ToString();
                        historial.NombreRuta = transporte.NombreRuta;
                        historial.NumeroManifiesto = transporte.NumeroTransporte;
                        historial.ClaseTransporte = transporte.ClaseTransporte;
                    }
                    else
                    {
                        historial.NumeroDocConductor = historial.UsuarioActivacion;
                    }
                    HistorialActivacionManifiestoDAO historialActivacionDAO = new HistorialActivacionManifiestoDAO();

                    //si en la bd local el conductor aun tiene un viaje activo, se elimina
                    HistorialActivacionManifiesto historialAnterior = historialActivacionDAO.SeleccionarHistorialManifiestoActivoPorConductor(evento.NumeroDocumentoConductor);
                    if (historialAnterior != null)
                    {
                        historialActivacionDAO.EliminarHistorialActivacionManifiesto(historialAnterior.NumeroManifiesto);
                    }

                    //se fuarda el registro de activacion viaje
                    historialActivacionDAO.GuardarHistorialActivacionManifiesto(historial);

                    //Se guardan las entregas del transporte
                    if (transporte != null && transporte.Entregas != null && transporte.Entregas.Count > 0)
                    {
                        EntregaBLL entregaBLL = new EntregaBLL();
                        foreach (Entrega entrega in transporte.Entregas)
                        {
                            entregaBLL.GuardarEntrega(entrega);
                        }
                    }

                }
                else if (evento.Sincronizado == true && evento.Estado == "E")
                {
                    //Si el evento se sincronizó pero devolvió error, se elimina el manifiesto del historial de activacion                    
                    HistorialActivacionManifiestoDAO historialActivacionDAO = new HistorialActivacionManifiestoDAO();
                    historialActivacionDAO.EliminarHistorialActivacionManifiesto(evento.NumeroManifiesto);
                }
            }
            else if (evento.IdTipoEvento == (int)TipoEventoLogisticoEnum.SuspenderViaje)
            {
                HistorialActivacionManifiestoBLL historialBLL = new HistorialActivacionManifiestoBLL();
                historialBLL.EliminarHistorialActivacionManifiesto(evento.NumeroManifiesto);
            }
            else if (evento.IdTipoEvento == (int)TipoEventoLogisticoEnum.ReanudarViaje)
            {
                Transporte transporte = new Transporte();
                HistorialActivacionManifiesto historial = new HistorialActivacionManifiesto();

                historial.Sincronizado = true;
                historial.FechaSincronizacion = DateTime.Now;
                historial.Activo = true;
                historial.FechaActivacion = evento.FechaEvento;
                historial.UsuarioActivacion = ParametrosSistema.UsuarioActual;
                TransporteBLL transporteBLL = new TransporteBLL();
                transporte = await transporteBLL.SeleccionarTransporte(evento.NumeroManifiesto);
               
                if (transporte != null)
                {
                    historial.Placa = transporte.Placa;
                    historial.NumeroDocConductor = transporte.NumeroDocConductor.ToString();
                    historial.NombreRuta = transporte.NombreRuta;
                    historial.NumeroManifiesto = transporte.NumeroTransporte;
                    historial.ClaseTransporte = transporte.ClaseTransporte;
                }
                else
                {
                    historial.NumeroDocConductor = historial.UsuarioActivacion;
                }
                HistorialActivacionManifiestoDAO historialActivacionDAO = new HistorialActivacionManifiestoDAO();

                //si en la bd local el conductor aun tiene un viaje activo, se elimina
                HistorialActivacionManifiesto historialAnterior = historialActivacionDAO.SeleccionarHistorialManifiestoActivoPorConductor(evento.NumeroDocumentoConductor);
                if (historialAnterior != null)
                {
                    historialActivacionDAO.EliminarHistorialActivacionManifiesto(historialAnterior.NumeroManifiesto);
                }

                //se guarda el registro de reanudación viaje
                historialActivacionDAO.GuardarHistorialActivacionManifiesto(historial);
            }
            else if (evento.IdTipoEvento == (int)TipoEventoLogisticoEnum.FinViaje)
            {
                HistorialActivacionManifiestoDAO historialActivacionDAO = new HistorialActivacionManifiestoDAO();
                var historialActivacion = historialActivacionDAO.SeleccionarHistorialActivacionManifiesto(evento.NumeroManifiesto);
                if (historialActivacion != null)
                {
                    if (evento.Sincronizado == false || respuesta.ProcesadoCorrectamente == true)
                    {
                        //Se debe marcar como finalizado el viaje
                        historialActivacion.Activo = false;
                        historialActivacion.FechaInactivacion = evento.FechaEvento;
                        historialActivacion.UsuarioInactivacion = evento.NumeroDocumentoConductor;
                        historialActivacionDAO.GuardarHistorialActivacionManifiesto(historialActivacion);

                    }
                    else if (evento.Sincronizado == true && evento.Estado == "E")
                    {
                        //Si el evento se sincronizó pero devolvió error, se elimina el manifiesto del historial de activacion
                        historialActivacion.Activo = true;
                        historialActivacion.FechaInactivacion = null;
                        historialActivacion.UsuarioInactivacion = null;
                        historialActivacionDAO.GuardarHistorialActivacionManifiesto(historialActivacion);

                    }
                }

            }
            return respuesta;
        }

        public List<EventoLogistico> SeleccionarEventosLogisticos(long? numeroManifiesto, string estado = "S", int? codigoTipoEvento = null, bool consultaLocal = true)
        {
            EventoLogisticoDAO dao = new EventoLogisticoDAO();
            return dao.SeleccionarEventosLogisticos(numeroManifiesto,estado,codigoTipoEvento).
                Where(d => d.NombreTipoEvento != null).ToList();
        }

        public List<EventoLogistico> SeleccionarEventosPendientesSincronizar()
        {
            EventoLogisticoDAO dao = new EventoLogisticoDAO();
            return dao.SeleccionarEventosPendientesSincronizar();
        }

        public int EliminarEventoLogisticoLocal(int idApp)
        {
            EventoLogisticoDAO dao = new EventoLogisticoDAO();
            return dao.EliminarEventoLogistico(idApp);
        }

        public Task<List<TipoEventoLogistico>> SeleccionarTiposEventoLogistico(bool consultaLocal = true, bool? aplicaTerceros = null)
        {
            Task<List<TipoEventoLogistico>> tiposEvento = null;
            if (consultaLocal)
            {
                EventoLogisticoDAO dao = new EventoLogisticoDAO();
                tiposEvento = Task.FromResult(dao.SeleccionarTiposEventoLogistico(aplicaTerceros));
            }
            else
            {                
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
                if(aplicaTerceros.HasValue && aplicaTerceros.Value)
                    tiposEvento = ds.RealizarPeticionApi<List<TipoEventoLogistico>>("Evento/SeleccionarTiposEventosLogisticos?aplicaTerceros="+aplicaTerceros, DataService.TipoPeticionApi.Get);
                else
                    tiposEvento = ds.RealizarPeticionApi<List<TipoEventoLogistico>>("Evento/SeleccionarTiposEventosLogisticos", DataService.TipoPeticionApi.Get);                
                return tiposEvento;
                
            }
            return tiposEvento;
        }

        public Task<List<ItemCampoEventoLogistico>> SeleccionarItemsPorCamposEvento(int? codigoEvento, string campoEvento, bool consultaLocal = true)
        {
            Task<List<ItemCampoEventoLogistico>> itemsCampo;
            if (consultaLocal)
            {
                EventoLogisticoDAO dao = new EventoLogisticoDAO();
                itemsCampo = Task.FromResult(dao.SeleccionarItemsPorCamposEvento(codigoEvento, campoEvento));
            }
            else
            {               
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
                string url = "Evento/SeleccionarItemsPorCamposEvento";
                if (codigoEvento.HasValue && !String.IsNullOrEmpty(campoEvento))
                {
                    url += "?tipoEvento=" + codigoEvento.Value.ToString()+ "&campoEvento=" + campoEvento;
                }
                else
                {
                    url += "?tipoEvento=&campoEvento=";
                }
                itemsCampo = ds.RealizarPeticionApi<List<ItemCampoEventoLogistico>>(url, DataService.TipoPeticionApi.Get);                
            }
            return itemsCampo;
        }
        public Task<List<SubItemCampoEventoLogistico>> SeleccionarSubItemsPorCamposEvento(int? idItem, bool consultaLocal = true)
        {
            Task<List<SubItemCampoEventoLogistico>> subItemsCampo;
            if (consultaLocal)
            {
                EventoLogisticoDAO dao = new EventoLogisticoDAO();
                subItemsCampo = Task.FromResult(dao.SeleccionarSubItemsPorCamposEvento(idItem));
            }
            else
            {
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
                string url = "Evento/SeleccionarSubItemsPorItem";
                if (idItem.HasValue)
                    url += "?idItem=" + idItem.Value.ToString();
                else
                    url += "?idItem=";
                subItemsCampo = ds.RealizarPeticionApi<List<SubItemCampoEventoLogistico>>(url, DataService.TipoPeticionApi.Get);
            }
            return subItemsCampo;
        }
        public Task<List<JerarquiaTipoEventoLogistico>> SeleccionarJerarquiaTipoEventosLogisticos(bool consultaLocal = true,bool? aplicaTerceros=null)
        {
            Task<List<JerarquiaTipoEventoLogistico>> jerarquia;
            if(consultaLocal)
            {
                EventoLogisticoDAO dao = new EventoLogisticoDAO();
                jerarquia = Task.FromResult(dao.SeleccionarJerarquiaTipoEventosLogisticos());
            }
            else
            {
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);

                string url = "Evento/SeleccionarJerarquiaTipoEventosLogisticos";
                if (aplicaTerceros.HasValue && aplicaTerceros.Value)
                    url = "Evento/SeleccionarJerarquiaTipoEventosLogisticos?aplicaTerceros="+aplicaTerceros.Value;

                jerarquia = ds.RealizarPeticionApi<List<JerarquiaTipoEventoLogistico>>(url, DataService.TipoPeticionApi.Get);
            }
            return jerarquia;
        }

        public Task<List<TipoEventoLogistico>> SeleccionarEventosTipoOtros(Int64 numeromanifiesto, bool consultaLocal=true,bool? aplicaTerceros=null)
        {
            Task<List<TipoEventoLogistico>> tipoeventos;
            if (consultaLocal)
            {
                EventoLogisticoDAO dao = new EventoLogisticoDAO();
                tipoeventos = Task.FromResult(dao.SeleccionarEventosTipoOtros(numeromanifiesto, aplicaTerceros));
            }
            else
            {
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);

                string url = "Evento/SeleccionarEventoTipoOtros?numeroTransporte=" + numeromanifiesto.ToString();

                if (aplicaTerceros.HasValue && aplicaTerceros.Value)
                    url = url + "&aplicaTerceros=" + aplicaTerceros.Value;

                tipoeventos = ds.RealizarPeticionApi<List<TipoEventoLogistico>>(url, DataService.TipoPeticionApi.Get);
            }
            return tipoeventos;
        }

        public Task<List<EventoLogistico>> SeleccionarEventosLogisticosUsuarioActual()
        {
            Task<List<EventoLogistico>> eventos;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Evento/SeleccionarEventosLogisticosAppMovil/";
            eventos = ds.RealizarPeticionApi<List<EventoLogistico>>(url, DataService.TipoPeticionApi.Get);
            return eventos;
        }
        
        public EventoLogistico SeleccionarEventosLogisticoporID(Int64 idEvento)
        {
            EventoLogisticoDAO eventoDAO = new EventoLogisticoDAO();
            List<EventoLogistico> eventos = eventoDAO.SeleccionarEventosLogisticoporID(idEvento);
            EventoLogistico evento = new EventoLogistico();

            if(eventos!=null && eventos.Count>0)
                evento = eventos[0];

            return evento;                
        }

        public Task<bool> EliminarEventosLogisticosPorManifiesto(long numeroManifiesto)
        {
            Task<bool> eliminado;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Evento/EliminarEventosLogisticosPorManifiesto/"+numeroManifiesto;
            eliminado = ds.RealizarPeticionApi<bool>(url, DataService.TipoPeticionApi.Delete);            
            return eliminado;
        }

        public void EliminarEventoLogisticoLocalPorManifiesto(long numeroManifiesto)
        {
            EventoLogisticoDAO dao = new EventoLogisticoDAO();
            dao.EliminarEventoLogisticoLocalPorManifiesto(numeroManifiesto);
        }

        public async Task<List<TipoEventoLogistico>> SeleccionarEventosLogisticosBodega()
        {
            List<TipoEventoLogistico> eventosBodega = new List<TipoEventoLogistico>();

            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            eventosBodega = await ds.RealizarPeticionApi<List<TipoEventoLogistico>>("Evento/SeleccionarEventosBodega", DataService.TipoPeticionApi.Get);//.ConfigureAwait(false);

            eventosBodega = eventosBodega.Where(x => x.NombreEvento.Equals("Recepcion Entrega") || x.NombreEvento.Equals("Despacho Entrega")
                || x.NombreEvento.Equals("Recepcion Viaje") || x.NombreEvento.Equals("Despacho Viaje") || x.NombreEvento.Equals("Otra Operacion Bodega")).ToList();

            return eventosBodega;
        }

        public async Task<List<RemesasPorNumeroEntrega>> SeleccionarRemesaPorNumeroEntrega(Int32 numeroEntrega)
        {
            List<RemesasPorNumeroEntrega> remesasPorNumeroEntrega = new List<RemesasPorNumeroEntrega>();

            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            remesasPorNumeroEntrega = await ds.RealizarPeticionApi<List<RemesasPorNumeroEntrega>>("Evento/SeleccionarRemesaPorNumeroEntrega?numeroEntrega=" + numeroEntrega.ToString(), DataService.TipoPeticionApi.Get);

            return remesasPorNumeroEntrega;
        }

        public async Task<List<RemesasPorNumeroTransporte>> SeleccionarRemesasPorNumeroViaje(Int32 numeroViaje)
        {
            List<RemesasPorNumeroTransporte> remesasPorNumeroTransporte = new List<RemesasPorNumeroTransporte>();

            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            remesasPorNumeroTransporte = await ds.RealizarPeticionApi<List<RemesasPorNumeroTransporte>>("Evento/SeleccionarRemesasPorNumeroTransporte?numeroTransporte=" + numeroViaje.ToString(), DataService.TipoPeticionApi.Get);

            return remesasPorNumeroTransporte;
        }

        public async void GuardarPosicionVehiculoGPS(EventoLogistico evento)
        {
            RespuestaProcesoEventoLogistico respuesta = new RespuestaProcesoEventoLogistico();

            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            await ds.RealizarPeticionApi<RespuestaProcesoEventoLogistico>("Evento/GuardarPosicionVehiculoGPS", DataService.TipoPeticionApi.Post, evento);
        }
    }
}
