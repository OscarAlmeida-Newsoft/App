using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.Entities
{
    class OpcionMenu
    {
        public string NombreOpcion { get; set; }
        public Func<ContentPage> PageFn{ get; set; }

        public OpcionMenu(string nombreOpcion, Func<ContentPage> pageFn)
        {
            NombreOpcion = nombreOpcion;
            PageFn = pageFn;
        }
    }
}
