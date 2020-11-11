
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Comercial
{
    public class Entrega
    {

        private int _NumeroEntrega;
        public int NumeroEntrega
        {
            get { return _NumeroEntrega; }
            set
            {
                _NumeroEntrega = value;

            }
        }
        

        private String _NombreCliente;
        public String NombreCliente
        {
            get { return _NombreCliente; }
            set
            {
                _NombreCliente = value;

            }
        }
        public long NumeroTransporte { get; set; }

        [Ignore]
        public List<DetalleEntrega> Posiciones { get; set; }

        public bool EstaFirmada { get; set; }
    }
}
