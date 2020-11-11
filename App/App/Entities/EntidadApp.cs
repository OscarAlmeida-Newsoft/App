using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities
{
    public abstract class EntidadApp
    {
        [PrimaryKey, AutoIncrement]
        public int IdApp { get; set; }

        public bool? Sincronizado { get; set; }

        public DateTime? FechaSincronizacion { get; set; }

        public string ErrorSincronizacion { get; set; }

    }
}
