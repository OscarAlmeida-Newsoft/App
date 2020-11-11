using App.Entities.IT.Database;
using App.Entities.Operaciones;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.DAO
{
    public abstract class AppDAOBase
    {
        protected SQLiteConnection _db;
        protected static object locker = new object();

        public AppDAOBase()
        {
            _db = DependencyService.Get<ISQLite>().GetConnection();
            _db.CreateTable<EventoLogistico>();
        }
    }
}
