
using App.BLL.IT;
using App.Common;
using App.DataService;
using App.Entities.Seguridad;
using App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
                ClaimResourceName = "app_movil",
                ParametrosAdicionales = "Version:" + ParametrosSistema.AppVersion
            };
            var token = await _ds.RealizarPeticionApi<TokenSeguridad>("Autenticar", TipoPeticionApi.Post, data);
            if(token != null && !String.IsNullOrEmpty(token.access_token))
            {
                _ds = new AppDataService(token.access_token);
                //se guardan las credenciales de forma segura
                this.GuardarCredencialesUsuario(token.userName, password, null, null, token.access_token);

                //Se obtiene información del usuario
                Usuario infoUsuario = await this.SeleccionarUsuario(token.userName);
                if(infoUsuario!=null)
                {
                    this.GuardarCredencialesUsuario(token.userName, password, infoUsuario.NumeroIdentificacion.ToString(), infoUsuario.NombreUsuario, token.access_token);
                    //se consultan y guardan los permisos del usuario
                    List<PermisoAplicacion> permisos = new List<PermisoAplicacion>();
                    permisos = await this.ObtenerPermisosUsuario();
                    string jsonPermisos = Newtonsoft.Json.JsonConvert.SerializeObject(permisos);
                    this.GuardarPermisosUsuario(jsonPermisos);

                    //Se resetea la variable estatica de permisos
                    ParametrosSistema.PermisosUsuarioAlmacenado = null;
                }
                else
                {
                    token = null;
                }
            }
            return token;
        }

        public async Task<bool> RecuperarClave(string usuario)
        {
            bool resultado = false;
            if(String.IsNullOrEmpty(usuario))
            {
                throw new ArgumentNullException("El parámetro usuario es obligatorio para recuperar la contraseña.");
            }
            var response = await _ds.RealizarPeticionApi<System.Net.Http.HttpResponseMessage>("recuperarContrasenia?usuario="+usuario, TipoPeticionApi.Get);
            if(response == null || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultado = true;
            }
            else
            {
                resultado = false;

            }
            return resultado;
        }

        public async Task<string> Registrar(Usuario usuario)
        {
            string usuarioTask = null;
         
            if (usuario!=null)
            {
               usuarioTask = await _ds.RealizarPeticionApi<string>("Registrar", TipoPeticionApi.Post,usuario);          
            }
            return usuarioTask;
        }
        public Task<Usuario> SeleccionarUsuario(string nombreUsuario)
        {
            Task<Usuario> usuarioTask;
                List<PermisoAplicacion> permisos = new List<PermisoAplicacion>();
            usuarioTask = _ds.RealizarPeticionApi<Usuario>("ObtenerUsuario?nombreUsuario="+nombreUsuario, TipoPeticionApi.Get);
            return usuarioTask;
        }

        private void GuardarCredencialesUsuario(string usuario, string password, string numeroIdentificacion, string nombre, string token)
        {
            DependencyService.Get<ICredentialsService>().SaveCredentials(usuario, password,numeroIdentificacion,nombre, token);
        }

        public async Task<List<PermisoAplicacion>> ObtenerPermisosUsuario()
        {
            List<PermisoAplicacion> permisos = new List<PermisoAplicacion>();
            permisos = await _ds.RealizarPeticionApi<List<PermisoAplicacion>>("ObtenerPermisosUsuario?claimResourceName=app_movil", TipoPeticionApi.Get);
            return permisos;
        }

        public void EliminarCredencialesUsuario()
        {
            DependencyService.Get<ICredentialsService>().DeleteCredentials();
        }

        private void GuardarPermisosUsuario(string permisos)
        {
            DependencyService.Get<ICredentialsService>().SavePermissions(permisos);
        }

    }
}
