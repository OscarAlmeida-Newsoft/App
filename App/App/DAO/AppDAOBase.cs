
using App.Entities.Operaciones;
using App.Interfaces;
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
        
        protected static object locker = new object();

        public AppDAOBase()
        {   
            
        }
    }
}
