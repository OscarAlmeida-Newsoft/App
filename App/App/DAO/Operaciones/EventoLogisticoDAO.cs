using App.Entities.Operaciones;
using App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.DAO.Operaciones
{
    public class EventoLogisticoDAO : AppDAOBase
    {
        public List<EventoLogistico> SeleccionarEventosLogisticos(long? numeroManifiesto, string estado = "S", int? codigoTipoEvento = null)
        {
            using (var db = DependencyService.Get<ISQLite>().GetConnection())
            {
                List<EventoLogistico> eventos = new List<EventoLogistico>();
                if(numeroManifiesto.HasValue || codigoTipoEvento.HasValue || !String.IsNullOrEmpty(estado))
                {
                    string query = "SELECT * FROM [EventoLogistico] WHERE ";
                    bool esPrimerParametro = true;
                    //List<object> valoresParametros = new List<object>();
                    Dictionary<string, object> parametros = new Dictionary<string, object>();
                    if(numeroManifiesto.HasValue)
                    {
                        //query += (esPrimerParametro?"":" AND ") + "[NumeroManifiesto] = ?";
                        parametros.Add("[NumeroManifiesto]", numeroManifiesto.Value);
                        //valoresParametros.Add(numeroManifiesto.Value);
                    }
                    if(codigoTipoEvento.HasValue)
                    {
                        //query += (esPrimerParametro ? "" : " AND ") + "[IdTipoEvento] = ?";
                        parametros.Add("[IdTipoEvento]", codigoTipoEvento.Value);
                        //valoresParametros.Add(codigoTipoEvento.Value);
                    }
                    if (!String.IsNullOrEmpty(estado))
                    {
                        //query += (esPrimerParametro ? "" : " AND ") + "[Estado] = ?";
                        parametros.Add("[Estado]", estado);
                        //valoresParametros.Add(estado);
                    }
                    for(int i = 0; i<parametros.Count; i++)
                    {
                        if(i != 0)
                        {
                            query += " AND ";
                        }

                        query += parametros.ElementAt(i).Key + " = ?";
                    }
                    lock (locker)
                    {                        
                        eventos = db.Query<EventoLogistico>( query,parametros.Select(p=>p.Value).ToArray<object>());
                    }
                }
                else
                {
                    lock (locker)
                    {
                        eventos = db.Table<EventoLogistico>().ToList();
                    }
                }

                
                return eventos;
            }
                
        }

        public List<EventoLogistico> SeleccionarEventosPendientesSincronizar()
        {
            List<EventoLogistico> eventos = new List<EventoLogistico>();
            lock (locker)
            {
                using (var _db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    eventos = _db.Query<EventoLogistico>("SELECT * FROM [EventoLogistico] WHERE [Sincronizado] = 0");
                }
                    
            }
            return eventos;
        }

        public EventoLogistico GuardarEventoLogistico(EventoLogistico evento)
        {
            lock (locker)
            {
                if (evento.IdApp != 0)
                {
                    using (var _db = DependencyService.Get<ISQLite>().GetConnection())
                    {
                        _db.Update(evento);
                    }
                    
                    return evento;
                }
                else {
                    using (var _db = DependencyService.Get<ISQLite>().GetConnection())
                    {
                        evento.IdApp = _db.Insert(evento);
                    }
                    return evento;
                }
            }
            
        }

        public int EliminarEventoLogistico(int idApp)
        {
            using (var _db = DependencyService.Get<ISQLite>().GetConnection())
            {
                lock (locker)
                {
                    return _db.Delete<EventoLogistico>(idApp);
                }
            }
            
        }

        public List<TipoEventoLogistico> SeleccionarTiposEventoLogistico(bool? aplicaTerceros = null)
        {
            List<TipoEventoLogistico> tipos;
            lock (locker)
            {
                using (var _db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    if(aplicaTerceros.HasValue && aplicaTerceros.Value)
                        tipos = _db.Query<TipoEventoLogistico>("SELECT * FROM [TipoEventoLogistico] WHERE [AplicaTerceros] = 1");
                    else
                        tipos = _db.Table<TipoEventoLogistico>().ToList();
                }
            }
            return tipos;
        }

        public List<CampoEventoLogistico> SeleccionarCamposPorEvento(int? codigoEvento)
        {
            List<CampoEventoLogistico> tipos;
            lock (locker)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    if(codigoEvento.HasValue)
                    {
                        tipos = db.Query<CampoEventoLogistico>("SELECT * FROM [CampoEventoLogistico] WHERE [CodigoEvento] = ?",codigoEvento);
                    }
                    else
                    {
                        tipos = db.Table<CampoEventoLogistico>().ToList();
                    }
                    
                }
            }
            return tipos;
        }

        public List<ItemCampoEventoLogistico> SeleccionarItemsPorCamposEvento(int? codigoEvento, string campoEvento)
        {
            List<ItemCampoEventoLogistico> tipos;
            lock (locker)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    if (codigoEvento.HasValue && !String.IsNullOrEmpty(campoEvento))
                    {
                        tipos = db.Query<ItemCampoEventoLogistico>("SELECT * FROM [ItemCampoEventoLogistico] WHERE [IdEvento] = ? AND [CampoEventoLogistico] = ?", codigoEvento, campoEvento);
                    }
                    else
                    {
                        tipos = db.Table<ItemCampoEventoLogistico>().ToList();
                    }

                }
            }
            return tipos;
        }
        public List<SubItemCampoEventoLogistico> SeleccionarSubItemsPorCamposEvento(int? idItem)
        {
            List<SubItemCampoEventoLogistico> subitems;
            lock (locker)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    if (idItem.HasValue)
                        subitems = db.Query<SubItemCampoEventoLogistico>("SELECT * FROM [SubItemCampoEventoLogistico] WHERE [IdItem] =?", idItem);
                    else
                        subitems = db.Table<SubItemCampoEventoLogistico>().ToList();
                }
            }
            return subitems;
        }
        public List<JerarquiaTipoEventoLogistico> SeleccionarJerarquiaTipoEventosLogisticos()
        {
            List<JerarquiaTipoEventoLogistico> tipos;
            lock (locker)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    tipos = db.Table<JerarquiaTipoEventoLogistico>().ToList();
                }
            }
            return tipos;
        }

        public List<TipoEventoLogistico> SeleccionarEventosTipoOtros(Int64 numeromanifiesto, bool? aplicaTerceros = null)
        {
            List<TipoEventoLogistico> tipoEventos;
            lock (locker)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    if (!aplicaTerceros.HasValue || (aplicaTerceros.HasValue && !aplicaTerceros.Value))
                    {
                        if (numeromanifiesto != 0)
                        {
                            tipoEventos = db.Query<TipoEventoLogistico>("SELECT * FROM [TipoEventoLogistico] WHERE [EventoTipoOtros] = 1");
                        }
                        else
                        {
                            tipoEventos = db.Query<TipoEventoLogistico>("SELECT * FROM [TipoEventoLogistico] WHERE [TransporteObligatorio] = 0 AND [EventoTipoOtros] = 1");
                        }
                    }
                    else
                    {
                        tipoEventos = db.Query<TipoEventoLogistico>("SELECT * FROM [TipoEventoLogistico] WHERE [EventoTipoOtros] = 1");
                    }

                }
            }
            return tipoEventos;
        }

        public List<TipoEventoLogistico> SeleccionarEventosSucesores(int codigoEvento)
        {
            List<TipoEventoLogistico> eventosSucesores = new List<TipoEventoLogistico>();
            lock (locker)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    eventosSucesores = db.Query<TipoEventoLogistico>("SELECT E.* FROM  [TipoEventoLogistico] E " +
                        "INNER JOIN [JerarquiaTipoEventoLogistico] J ON J.CodigoEvento = E.CodigoEvento " +
                        
                        "WHERE J.CodigoEventoPredecesor = ? and E.EventoTipoOtros = 0 AND E.Activo = 1", codigoEvento);


                }
            }
            return eventosSucesores;
        }
        public List<EventoLogistico> SeleccionarEventosLogisticoporID(Int64 idEvento)
        {
            List<EventoLogistico> eventos;
            lock (locker)
            {
                //;
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    eventos = db.Query<EventoLogistico>("SELECT * FROM [EventoLogistico] WHERE [ID] = ?", idEvento);
                }
            }
            return eventos;
        }

        public void EliminarEventoLogisticoLocalPorManifiesto(long numeroManfiesto)
        {
            using (var _db = DependencyService.Get<ISQLite>().GetConnection())
            {
                lock (locker)
                {
                    List<EventoLogistico> eventos = new List<EventoLogistico>();
                    eventos = this.SeleccionarEventosLogisticos(numeroManfiesto);
                    if (eventos != null && eventos.Count > 0)
                    {
                        foreach (EventoLogistico e in eventos)
                        {
                            _db.Delete<EventoLogistico>(e.ID);
                        }
                    }                    
                }
            }

        }
    }
}
