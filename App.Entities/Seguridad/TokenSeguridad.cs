using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Seguridad
{
    public class TokenSeguridad
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string userName { get; set; }
    }
}
