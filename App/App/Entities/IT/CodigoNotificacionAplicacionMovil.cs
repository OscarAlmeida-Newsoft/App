using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.IT
{
    public class CodigoNotificacionAplicacionMovil
    {
        public string Usuario { get; set; }
        public string OneSignalId { get; set; }
        public string PushToken { get; set; }
        public string Plataforma { get; set; }
    }
}
