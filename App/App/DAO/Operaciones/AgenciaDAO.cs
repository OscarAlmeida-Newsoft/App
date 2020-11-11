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
    class AgenciaDAO :AppDAOBase
    {
        public List<Agencia> SeleccionarAgencias()
        {
            List<Agencia> agencias = new List<Agencia>();
            using (var db = DependencyService.Get<ISQLite>().GetConnection())
            {
                lock (locker)
                {
                    agencias = db.Query<Agencia>("SELECT * FROM [Agencia]");
                }
                return agencias;
            }
        }
    }
}
