using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common
{
    public class ParametrosSistema
    {
        private static string _apiURL = "https://www.tdm.com.co/api/";

        public static string ApiURL
        {
            get
            {
                return _apiURL;
            }
        }
    }
}
