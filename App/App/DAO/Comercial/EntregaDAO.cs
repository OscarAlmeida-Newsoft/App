using App.Interfaces;
using App.Entities.Comercial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.DAO.Comercial
{
    public class EntregaDAO : AppDAOBase
    {
        public List<Entrega> SeleccionarEntregasPorTransporte(long numeroManifiesto)
        {
            List<Entrega> entregas = new List<Entrega>();
            using (var db = DependencyService.Get<ISQLite>().GetConnection())
            {
                lock (locker)
                {
                    entregas = db.Query<Entrega>("SELECT * FROM [Entrega] WHERE [NumeroTransporte] = ?", numeroManifiesto);
                }
                return entregas;
            }
        }

        public Entrega SeleccionarEntrega(int numeroEntrega)
        {
            Entrega entrega = null;
            using (var db = DependencyService.Get<ISQLite>().GetConnection())
            {
                lock (locker)
                {
                    var entregas = db.Query<Entrega>("SELECT * FROM [Entrega] WHERE [NumeroEntrega] = ?", numeroEntrega);
                    if(entregas!=null && entregas.Count>0)
                    {
                        entrega = entregas[0];
                    }
                }
            }
            return entrega;
        }

        public Entrega GuardarEntrega(Entrega entrega)
        {
            using (var db = DependencyService.Get<ISQLite>().GetConnection())
            {
                lock (locker)
                {
                    Entrega entregaActual = null;
                    entregaActual = SeleccionarEntrega(entrega.NumeroEntrega);
                    if (entregaActual == null)
                    {
                        db.Insert(entrega);
                    }
                    else
                    {
                        db.Update(entrega);
                    }
                    
                }
                return entrega;
            }
        }
    }
}
