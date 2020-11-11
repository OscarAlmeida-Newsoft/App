using App.BLL.Comercial;
using App.BLL.Operaciones;
using App.Common;
using App.DAO.IT;
using App.DAO.Operaciones;
using App.Entities.Comercial;
using App.Entities.IT;
using App.Entities.Operaciones;
using App.Entities.Turnos;
using App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.BLL.IT
{
    class DatabaseBLL
    {
        public async Task<RespuestaProceso> CrearBaseDeDatos()
        {
            RespuestaProceso respuesta = new RespuestaProceso();
            respuesta.ProcesadoCorrectamente = true;

            List<Type> tablasRequeridas = new List<Type>();

            //Se verifica si el usuario debe tener base de datos local
            tablasRequeridas.Add(typeof(Entities.Operaciones.Agencia));

            if (ParametrosSistema.PermisosUsuarioAlmacenado.Count(p=>p.NombreOpcion.ToLower() == "registrar_evento_logistico" || p.NombreOpcion.ToLower() == "eventoslogisticosterceros") > 0)
            {
                
                tablasRequeridas.Add(typeof(Entities.Operaciones.TipoEventoLogistico));
                tablasRequeridas.Add(typeof(Entities.Operaciones.EventoLogistico));
                tablasRequeridas.Add(typeof(Entities.Operaciones.CampoEventoLogistico));
                tablasRequeridas.Add(typeof(Entities.Operaciones.ItemCampoEventoLogistico));
                tablasRequeridas.Add(typeof(Entities.Operaciones.SubItemCampoEventoLogistico));
                tablasRequeridas.Add(typeof(Entities.Operaciones.JerarquiaTipoEventoLogistico));
                //tablasRequeridas.Add(typeof(Entities.Operaciones.RemesasPorNumeroEntrega));

                tablasRequeridas.Add(typeof(Entities.Operaciones.HistorialActivacionManifiesto));
                tablasRequeridas.Add(typeof(Entities.Comercial.Entrega));

                tablasRequeridas.Add(typeof(Entities.IT.ConfiguracionApp));                               
            }
            if (tablasRequeridas.Count > 0)
            {
                DatabaseDAO dao = new DatabaseDAO();
                bool esNueva = false;
                if (!DependencyService.Get<ISQLite>().ExisteBaseDeDatos() || true)
                {
                    dao.CrearBaseDeDatos(tablasRequeridas);
                    esNueva = true;
                }
                if (esNueva)
                {
                    if (await Common.ParametrosSistema.isOnline)
                    {
                        try
                        {
                            //Se llenan las tablas maestras
                            EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();

                            bool? aplicaTerceros = null;
                            if (ParametrosSistema.PermisosUsuarioAlmacenado.Count(p => p.NombreOpcion.ToLower() == "eventoslogisticosterceros") > 0)
                            {
                                aplicaTerceros = true;
                            }
                            foreach (Type tabla in tablasRequeridas)
                            {
                                switch(tabla.Name)
                                {
                                    case "TipoEventoLogistico":                                       
                                        var tiposEventos = await eventoBLL.SeleccionarTiposEventoLogistico(consultaLocal: false,aplicaTerceros:aplicaTerceros);
                                        dao.GuardarRegistros(tiposEventos);
                                        break;
                                    case "EventoLogistico":
                                        if(!String.IsNullOrEmpty( ParametrosSistema.UsuarioActual))
                                        {
                                            //Se consultan los ultimos eventos del usuario actual
                                            var eventos = await eventoBLL.SeleccionarEventosLogisticosUsuarioActual();
                                            if(eventos!=null)
                                            {
                                                dao.GuardarRegistros(eventos);
                                            }
                                        }
                                        break;
                                    case "CampoEventoLogistico":
                                        var camposTiposEventos = await eventoBLL.SeleccionarCamposPorEvento(null, consultaLocal: false);
                                        dao.GuardarRegistros(camposTiposEventos);
                                        break;
                                    case "ItemCampoEventoLogistico":
                                        var itemsCampoEventoLogistico = await eventoBLL.SeleccionarItemsPorCamposEvento(null, null, consultaLocal: false);
                                        dao.GuardarRegistros(itemsCampoEventoLogistico);
                                        break;
                                    case "SubItemCampoEventoLogistico":
                                        var subItemsCampoEventoLogistico = await eventoBLL.SeleccionarSubItemsPorCamposEvento(null,consultaLocal: false);
                                        dao.GuardarRegistros(subItemsCampoEventoLogistico);
                                        break;
                                    case "JerarquiaTipoEventoLogistico":                                       
                                        var jerarquiaTiposEventosLogisticos = await eventoBLL.SeleccionarJerarquiaTipoEventosLogisticos(consultaLocal: false, aplicaTerceros: aplicaTerceros);
                                        dao.GuardarRegistros(jerarquiaTiposEventosLogisticos);
                                        break;
                                    case "HistorialActivacionManifiesto":
                                        var transportes = await eventoBLL.SeleccionarTransporteHabilitadoRegistroEventos(consultaLocal: false,tercero:aplicaTerceros);
                                        if(transportes != null && transportes.Count > 0)
                                        {
                                            Transporte transporte = transportes[0];

                                            HistorialActivacionManifiesto historial = new HistorialActivacionManifiesto();
                                            historial.Activo = true;
                                            historial.FechaActivacion = DateTime.Now;
                                            historial.NumeroManifiesto = transporte.NumeroTransporte;
                                            historial.Placa = transporte.Placa;
                                            historial.NumeroDocConductor = transporte.NumeroDocConductor.ToString();
                                            historial.NombreRuta = transporte.NombreRuta;
                                            historial.UsuarioActivacion = ParametrosSistema.UsuarioActual;

                                            //Se busca localmente el evento de activación del transporte activo
                                            var eventosActivacionTransporte = eventoBLL.SeleccionarEventosLogisticos(transporte.NumeroTransporte, codigoTipoEvento: (int)TipoEventoLogisticoEnum.ActivarViaje, consultaLocal: true);
                                            if(eventosActivacionTransporte!=null & eventosActivacionTransporte.Count>0)
                                            {
                                                historial.FechaActivacion = eventosActivacionTransporte[0].FechaEvento;
                                            }

                                            HistorialActivacionManifiestoDAO historialActivacionDAO = new HistorialActivacionManifiestoDAO();
                                            historialActivacionDAO.GuardarHistorialActivacionManifiesto(historial);

                                            //Se guardan las entregas del transporte
                                            if (transporte.Entregas != null && transporte.Entregas.Count > 0)
                                            {
                                                EntregaBLL entregaBLL = new EntregaBLL();
                                                foreach (Entrega entrega in transporte.Entregas)
                                                {
                                                    entregaBLL.GuardarEntrega(entrega);
                                                }
                                            }
                                        }
                                        break;
                                    case "Agencia":
                                        List<Agencia> agencias = new List<Agencia>();
                                        agencias = CargarAgencias();
                                        dao.GuardarRegistros(agencias);
                                        break;
                                }
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            respuesta.ProcesadoCorrectamente = false;
                            respuesta.Respuesta = "Ocurrió un error configurando la aplicación.";
                        }

                    }
                    else
                    {
                        respuesta.ProcesadoCorrectamente = false;
                        respuesta.Respuesta = "Se necesita una conexión a Internet para que la aplicación pueda configurarse correctamente. Esto sólo se necesita la primera vez.";
                    }


                }

            }




            
            return respuesta;
        }

        public List<Agencia> CargarAgencias()
        {
            Agencia agencia6 = new Agencia();
            List<Agencia> agencias = new List<Agencia>();
            agencia6.CodigoAgencia = "TD06";
            agencia6.NombreAgencia = "Barranquilla";
            agencias.Add(agencia6);
            Agencia agencia3 = new Agencia();
            agencia3.CodigoAgencia = "TD03";
            agencia3.NombreAgencia = "Bogotá";
            agencias.Add(agencia3);
            Agencia agencia2 = new Agencia();
            agencia2.CodigoAgencia = "TD02";
            agencia2.NombreAgencia = "Buenaventura";
            agencias.Add(agencia2);
            Agencia agencia5 = new Agencia();
            agencia5.CodigoAgencia = "TD05";
            agencia5.NombreAgencia = "Cali";
            agencias.Add(agencia5);
            Agencia agencia4 = new Agencia();
            agencia4.CodigoAgencia = "TD04";
            agencia4.NombreAgencia = "Cartagena";
            agencias.Add(agencia4);
            Agencia agencia1 = new Agencia();
            agencia1.CodigoAgencia = "TD01";
            agencia1.NombreAgencia = "Girardota";
            agencias.Add(agencia1);
            Agencia agencia10 = new Agencia();
            agencia10.CodigoAgencia = "TD10";
            agencia10.NombreAgencia = "Haceb";
            agencias.Add(agencia10);

            return agencias;            
        }
       
    }
}
