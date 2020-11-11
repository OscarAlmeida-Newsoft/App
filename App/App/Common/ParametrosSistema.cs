using App.Entities.Seguridad;
using App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.Common
{
    public class ParametrosSistema
    {
        //private static string _apiURL = "https://www.tdm.com.co/api/";
        private static string _apiURL = "http://192.168.1.12:4743/";

        private static List<int> eventoTipoOtroconRestriccion = null;
        private static List<int> eventoConCampoEntregas = null;
        private static List<PermisoAplicacion> _permisosUsuario;

        public static string ApiURL
        {
            get
            {
                return _apiURL;
            }
        }
        public static string AppVersion
        {
            get
            {
                return "2.2.6.6";
            }
        }
        public static string MaxDatabaseVersion
        {
            get
            {
                return "1.1";
            }
        }

        public static string PasswordUsuarioActual
        {
            get
            {
                return DependencyService.Get<ICredentialsService>().Password;

            }
        }
        public static string UsuarioActual
        {
            get
            {
                return DependencyService.Get<ICredentialsService>().UserName;
            }
        }
        public static string NumeroIdentificacionUsuarioActual
        {
            get
            {
                return DependencyService.Get<ICredentialsService>().NumeroIdentificacion;
            }
        }
        public static string NombreCompletoUsuarioActual
        {
            get
            {
                return DependencyService.Get<ICredentialsService>().NombreCompleto;
            }
        }
        public static string TokenUsuarioActual
        {
            get
            {
                //return "lFQ_xtgCwdX_a5SrVkBtAMYRCAFcD30S-ixfBnfpiEMPoNpb1IbAYqjtWeOD0zfNjcOWzTeM8lCLo0fc7Otlb1k9O2FLMjW-xCPpReqRi6N8M2tjD9JPv3QEBxPJrj7nhyDFBBFw1HfJybQpMoPqs2GnngdZ3ECaok1KUpscXe3sjBwQiFaMvDskbKs40KRjmMquP3dDp-owH-oBWSXvanrEMrD5ZDB5xf_-6kvcBIuiEA7A5y0tSzwoBAGzUCAOv0DhK_CQYFbZ7coDocbBOwaUTU5SoVUj4rR5dXb4pd8ORDbBD5K-swM7qEmV2-j19usQqq0nMy4H5wJgNddy00wL7bkez8PfltTDqPs8WedBTp5UB0vrd2yuuqn9Os-54YvMOz2AlEZS43nht6icTLAW5JsHwEpAzunnaBQ5vt4zQ9IUoXbi_M9oaUqlWZa4Dt-lqTeFAsr_-JRXdCLwfI9FCdGJXb7R4-AzerAc3JKwkzLqsamC4nUR122LNpdTt4SfXZAXsmym-1kMNaSIVEa4gnPBlSbCWaD24aRGwKL6EgbjIgrrXO6G3XQR1jRY05XUzXqCeukHyk5xv02EImYKzLrWFsoL0Oun7haWSF8mTj8_GWhZsZNqklKzLVz3O7T7ESVMkxa1HBlvhYOPVvtZS1i4doLnWuAFSnfyX2aFb-4uv0KC73_Qg995DT65TPTNSeqxJ0crRM6TohG6WnUMlxV_XQUbJ47mTdC8H3o39xl3W9ILDusRn1zk2N0zeERiFx5_yfM38k-ZH6Q55K7x6PJhH4jFXHHvL017nTxu31118Vrvr_yqwRfOtOVY9ksEjCJm891woi_eyckNHo7wDiskTTZeBylLkcIfVUCUMQuEBQJL4-YcDNuoNiyFmSZPRSn5bqT9DQ9mIBMdoxjKiLtjDa07HmPWoO81iC29wD7Bxrkci5UbiFiyy9YMjEj1LUgwvmiWrIAlb4TnhQ9sKE-1vMJqWa3gnIgkuidNzc73cA0Ys5WVea4rO2aRWPTqmkyfM_CbLI49iywqTy0kHOT07rLJikpbRlvdkJ62NpbWbuo0ysqFY4NRZ8lNqPgEpic2tZS7cvdI9JtwWGvk71eKILq3JE_t9bmmmNt7a4KbBYtvWupZjBZgIR5LDWQRdd-2HDLL8wn8WsFPZUDLRaLxDwDUrchGOOWLeJNBxrR1r7Sb0fs7kn60ziCqr3D4iVilRqw4cWeloVXuSmxssd1NdjAMKbexPL6R8NkHx9a_dpdxq6Rh9Q5lf-LgCbvhBeSwGol5ZNwZZ-3uQmm_fo16CvXpo5pDEpI3Sc4Zxm3wFZbJYCXVr4codQtpC3x6EoqqTtUrgtzELmr5kJlnq_g79NZN0aFNXHRj7xJGejaa0D-WX2yzlBGdasa2R8g1SMz6fqRAlqTJhs_5kbAanyl2QkXLVgMpdxGmpVYxUl8fOBo50LlR5E9LdU-J_Gh3kFH4tJUUY3BQjaNqRCUAVPBVNkb43Ns7H3UpdDx1smQgPk9jgBjrTRpMe1lPFRfY1JmRuQOSU2cSycGwuhWxzeprbbJN7LqTAYgNMpbIrsFbw26T4rZamV3ij0WYKmQoopL6P7U3HBIXR5qZtf-hsTypQNXFs0VG54xsjTP9tSpLQD2kgm0JTxWC9I-oEwAYVv1ue8PElJ9oU6bhRLMWBIA1amAzQiAiWjsitMNFNFdndogeZOKRwYpyahUC7vsjIOwckWaQYrilaN9GFL7cnQtS31T6pnfY7vjxTQu6UtgTQOcRdF_N9qXUbxlIuNAubjkorlh91-n9oQbdbd8U-NYopaxkDFtsTRDU_oN-xlVnpD6YgPhzdD3d2rAeFpuGifmcoVEttdnV-1qawvIMAxpdP7QHJWP1Xt_9nhoQOmmKfGB53p1cObsWF7UWKCy8RsUbA1-jYZUH96JDuY9x4ZNn39JKazYm3YMhVrXmozUMsutBvYdWO2rE84U1IVFaNA";
                return DependencyService.Get<ICredentialsService>().Token;

            }
        }
        public static List<PermisoAplicacion> PermisosUsuarioAlmacenado
        {
            get
            {
                if (_permisosUsuario == null)
                {
                    _permisosUsuario = new List<PermisoAplicacion>();

                    string jsonPermisos = DependencyService.Get<ICredentialsService>().Permisos;
                    if (!string.IsNullOrEmpty(jsonPermisos))
                        _permisosUsuario = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PermisoAplicacion>>(jsonPermisos);
                }

                return _permisosUsuario;
            }
            set
            {
                _permisosUsuario = value;
            }
            
        }

        public static Task<bool> isOnline
        {
            get
            {
                return DependencyService.Get<INetworkService>().isOnline();                
            }
        }

        public static bool IsOnlineSync {

            get {
                return DependencyService.Get<INetworkService>().isOnlineSync();
            }
        }

        public static List<int> EventoTipoOtroconRestriccion
        {
            get
            {
                if (eventoTipoOtroconRestriccion == null)
                {
                    eventoTipoOtroconRestriccion = new List<int>() { 7, 14, 19 };
                }
                return eventoTipoOtroconRestriccion;
            }
            set
            {
                eventoTipoOtroconRestriccion = value;
            }
        }

        public static List<int> EventoConCampoEntregas
        {
            get
            {
                if (eventoConCampoEntregas == null)
                {
                    eventoConCampoEntregas = new List<int>() { 3,4,5,9,31,25,26 };
                }
                return eventoConCampoEntregas;
            }
            set
            {
                eventoConCampoEntregas = value;
            }
        }
    }
}
