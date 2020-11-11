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
    public class HistorialActivacionManifiestoDAO : AppDAOBase
    {
        public HistorialActivacionManifiesto GuardarHistorialActivacionManifiesto(HistorialActivacionManifiesto historial)
        {
            lock (locker)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    HistorialActivacionManifiesto historialActual = null;
                    var historiales = db.Query<HistorialActivacionManifiesto>("SELECT * FROM [HistorialActivacionManifiesto] WHERE [NumeroManifiesto] = ?", historial.NumeroManifiesto);
                    if(historiales!=null && historiales.Count > 0)
                    {
                        historialActual = historiales[0];
                    }

                    if(historialActual==null)
                    {
                        historial.IdApp = db.Insert(historial);
                    }
                    else
                    {
                        db.Update(historial);

                    }
                }
            }
            return historial;
        }

        public HistorialActivacionManifiesto SeleccionarHistorialManifiestoActivoPorConductor(string numeroIdentificacion)
        {
            HistorialActivacionManifiesto historial = null;
            lock (locker)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    
                    var historiales = db.Query<HistorialActivacionManifiesto>("SELECT * FROM [HistorialActivacionManifiesto] WHERE [NumeroDocConductor] = ? AND [Activo] = 1", numeroIdentificacion);
                    if (historiales != null && historiales.Count > 0)
                    {
                        historial = historiales[0];
                    }
                    
                }
            }
            return historial;
        }
        public HistorialActivacionManifiesto SeleccionarHistorialActivacionManifiesto(long numeroManifiesto)
        {
            HistorialActivacionManifiesto historial = null;
            lock (locker)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {

                    var historiales = db.Query<HistorialActivacionManifiesto>("SELECT * FROM [HistorialActivacionManifiesto] WHERE [NumeroManifiesto] = ?", numeroManifiesto);
                    if (historiales != null && historiales.Count > 0)
                    {
                        historial = historiales[0];
                    }

                }
            }
            return historial;
        }


        public bool EliminarHistorialActivacionManifiesto(long numeroManifiesto)
        {
            bool respuesta = false;
            lock (locker)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    
                    HistorialActivacionManifiesto historialActual = null;
                    var historiales = db.Query<HistorialActivacionManifiesto>("SELECT * FROM [HistorialActivacionManifiesto] WHERE [NumeroManifiesto] = ?", numeroManifiesto);
                    if (historiales != null && historiales.Count > 0)
                    {
                        historialActual = historiales[0];
                    }

                    if (historialActual != null)
                    {
                        db.Delete<HistorialActivacionManifiesto>(historialActual.IdApp);
                        respuesta = true;
                        
                    }
                    else
                    {
                        respuesta = false;
                    }
                    
                }
            }
            return respuesta;
            
        }
    }
}
