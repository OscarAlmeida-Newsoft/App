using App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Documentos
{
    public class DocumentosBBL
    {
        public async Task<string> DescargaEntrega(string numeroEntrega)
        {
            string ruta = "";
            if (await ParametrosSistema.isOnline)
            {
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
                ruta = await ds.RealizarPeticionApi<string>("Docuware/DescargarEntrega/" + numeroEntrega, DataService.TipoPeticionApi.Get);

            }
            return ruta;
        }
    }
}
