using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAO.Operaciones
{
    public class EventoLogisticoDAO : AppDAOBase
    {
        public List<EventoLogistico> ObtenerEventos()
        {
            List<EventoLogistico> eventos = new List<EventoLogistico>();
            eventos=_db.Table<EventoLogistico>().ToList();
            return eventos;
        }

        //public EventoLogistico GuardarEventoLogistico(EventoLogistico evento)
        //{
        //    lock (locker)
        //    {
        //        if (evento.ID != 0)
        //        {
        //            _db.Update(item);
        //            return item.ID;
        //        }
        //        else {
        //            return database.Insert(item);
        //        }
        //    }
        //    _db.Table<EventoLogistico>().Intersect();
        //}
    }
}
