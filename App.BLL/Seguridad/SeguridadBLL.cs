using App.BLL.DataService;
using App.Common;

using App.Entities.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Seguridad
{
    public class SeguridadBLL:AppBLLBase
    {
        public async Task<TokenSeguridad> Autenticar(string usuario, string password)
        {
            var data = new
            {
                NombreUsuario = usuario,
                Password = password,
                ClaimResourceName = "app_movil"
            };
            var token = await _ds.RealizarPeticionApi<TokenSeguridad>("Autenticar", TipoPeticionApi.Post, data);
            
            return token;

        }
    }
}
