using App.Common;
using App.Entities.Seguridad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App.DataService
{
    public class AppDataService
    {
        private string _token;

        public AppDataService(string token)
        {
            _token = token;
            
            //Token para probar (Expirado)
            //"lFQ_xtgCwdX_a5SrVkBtAMYRCAFcD30S-ixfBnfpiEMPoNpb1IbAYqjtWeOD0zfNjcOWzTeM8lCLo0fc7Otlb1k9O2FLMjW-xCPpReqRi6N8M2tjD9JPv3QEBxPJrj7nhyDFBBFw1HfJybQpMoPqs2GnngdZ3ECaok1KUpscXe3sjBwQiFaMvDskbKs40KRjmMquP3dDp-owH-oBWSXvanrEMrD5ZDB5xf_-6kvcBIuiEA7A5y0tSzwoBAGzUCAOv0DhK_CQYFbZ7coDocbBOwaUTU5SoVUj4rR5dXb4pd8ORDbBD5K-swM7qEmV2-j19usQqq0nMy4H5wJgNddy00wL7bkez8PfltTDqPs8WedBTp5UB0vrd2yuuqn9Os-54YvMOz2AlEZS43nht6icTLAW5JsHwEpAzunnaBQ5vt4zQ9IUoXbi_M9oaUqlWZa4Dt-lqTeFAsr_-JRXdCLwfI9FCdGJXb7R4-AzerAc3JKwkzLqsamC4nUR122LNpdTt4SfXZAXsmym-1kMNaSIVEa4gnPBlSbCWaD24aRGwKL6EgbjIgrrXO6G3XQR1jRY05XUzXqCeukHyk5xv02EImYKzLrWFsoL0Oun7haWSF8mTj8_GWhZsZNqklKzLVz3O7T7ESVMkxa1HBlvhYOPVvtZS1i4doLnWuAFSnfyX2aFb-4uv0KC73_Qg995DT65TPTNSeqxJ0crRM6TohG6WnUMlxV_XQUbJ47mTdC8H3o39xl3W9ILDusRn1zk2N0zeERiFx5_yfM38k-ZH6Q55K7x6PJhH4jFXHHvL017nTxu31118Vrvr_yqwRfOtOVY9ksEjCJm891woi_eyckNHo7wDiskTTZeBylLkcIfVUCUMQuEBQJL4-YcDNuoNiyFmSZPRSn5bqT9DQ9mIBMdoxjKiLtjDa07HmPWoO81iC29wD7Bxrkci5UbiFiyy9YMjEj1LUgwvmiWrIAlb4TnhQ9sKE-1vMJqWa3gnIgkuidNzc73cA0Ys5WVea4rO2aRWPTqmkyfM_CbLI49iywqTy0kHOT07rLJikpbRlvdkJ62NpbWbuo0ysqFY4NRZ8lNqPgEpic2tZS7cvdI9JtwWGvk71eKILq3JE_t9bmmmNt7a4KbBYtvWupZjBZgIR5LDWQRdd-2HDLL8wn8WsFPZUDLRaLxDwDUrchGOOWLeJNBxrR1r7Sb0fs7kn60ziCqr3D4iVilRqw4cWeloVXuSmxssd1NdjAMKbexPL6R8NkHx9a_dpdxq6Rh9Q5lf-LgCbvhBeSwGol5ZNwZZ-3uQmm_fo16CvXpo5pDEpI3Sc4Zxm3wFZbJYCXVr4codQtpC3x6EoqqTtUrgtzELmr5kJlnq_g79NZN0aFNXHRj7xJGejaa0D-WX2yzlBGdasa2R8g1SMz6fqRAlqTJhs_5kbAanyl2QkXLVgMpdxGmpVYxUl8fOBo50LlR5E9LdU-J_Gh3kFH4tJUUY3BQjaNqRCUAVPBVNkb43Ns7H3UpdDx1smQgPk9jgBjrTRpMe1lPFRfY1JmRuQOSU2cSycGwuhWxzeprbbJN7LqTAYgNMpbIrsFbw26T4rZamV3ij0WYKmQoopL6P7U3HBIXR5qZtf-hsTypQNXFs0VG54xsjTP9tSpLQD2kgm0JTxWC9I-oEwAYVv1ue8PElJ9oU6bhRLMWBIA1amAzQiAiWjsitMNFNFdndogeZOKRwYpyahUC7vsjIOwckWaQYrilaN9GFL7cnQtS31T6pnfY7vjxTQu6UtgTQOcRdF_N9qXUbxlIuNAubjkorlh91-n9oQbdbd8U-NYopaxkDFtsTRDU_oN-xlVnpD6YgPhzdD3d2rAeFpuGifmcoVEttdnV-1qawvIMAxpdP7QHJWP1Xt_9nhoQOmmKfGB53p1cObsWF7UWKCy8RsUbA1-jYZUH96JDuY9x4ZNn39JKazYm3YMhVrXmozUMsutBvYdWO2rE84U1IVFaNA"
        }
        private async Task<HttpResponseMessage> Ejecutar(string url, TipoPeticionApi tipoPeticion, object data = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                //var _url = new Uri(ParametrosSistema.ApiURL);
                //string miUrl = _url.AbsolutePath;
                client.BaseAddress = new Uri(ParametrosSistema.ApiURL);
                //client.BaseAddress = _url;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!String.IsNullOrEmpty(_token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this._token);
                }

                string requestUri = url;


                string json = String.Empty;
                StringContent stringContent = null;

                if (data != null)
                {
                    json = JsonConvert.SerializeObject(data);
                    stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                }

                
                switch (tipoPeticion)
                {
                    case TipoPeticionApi.Post:

                        response = await client.PostAsync(requestUri, stringContent);//.ConfigureAwait(false);
                        break;
                    case TipoPeticionApi.Get:
                        response = await client.GetAsync(requestUri);//.ConfigureAwait(false);
                        break;
                    case TipoPeticionApi.Delete:
                        response = await client.DeleteAsync(requestUri);//.ConfigureAwait(false); 
                        break;
                    case TipoPeticionApi.Put:
                        response = await client.PutAsync(requestUri, stringContent);//.ConfigureAwait(false); 
                        break;
                    default:
                        throw new Exception("No se reconoce el tipo de petición realizada (TipoPeticionApi)");
                }

            }
            return response;
        }



        public async Task<T> RealizarPeticionApi<T>(string url, TipoPeticionApi tipoPeticion, object data = null)
        {
            T resultado = default(T);

            HttpResponseMessage response = await Ejecutar(url, tipoPeticion, data);//.ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();//.ConfigureAwait(false); 
                if(!string.IsNullOrEmpty(content))
                {
                    resultado = JsonConvert.DeserializeObject<T>(content);
                }
                
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !url.Contains("recuperarContrasenia") && !url.Contains("Registrar"))
                {
                    try
                    {
                        BLL.Seguridad.SeguridadBLL seguridadBLL = new BLL.Seguridad.SeguridadBLL();
                        var token = await seguridadBLL.Autenticar(ParametrosSistema.UsuarioActual, ParametrosSistema.PasswordUsuarioActual);//.ConfigureAwait(false);
                        if (token != null)
                        {
                            this._token = token.access_token;
                          
                            //seguridadBLL.GuardarCredencialesUsuario(token.userName, ParametrosSistema.PasswordAlmacenado, token.access_token);
                            response = await this.Ejecutar(url, tipoPeticion, data);//.ConfigureAwait(false); ;
                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();//.ConfigureAwait(false); ;
                                resultado = JsonConvert.DeserializeObject<T>(content);
                            }
                            else
                            {
                                var result = response.ReasonPhrase + " " + response.RequestMessage + " " + response.StatusCode;
                            }

                        }
                    }
                    catch (InvalidOperationException)
                    {
                        // user cancelled auth, so lets return the original response
                        //return response;
                        throw new Exception();
                    }
                }
                else
                {
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        string a = content;
                        if (content != null && !string.IsNullOrEmpty(content) && content != a)
                        {
                            resultado = JsonConvert.DeserializeObject<T>(content);
                        }
                        else
                        {
                            resultado = default(T);
                        }

                    }
                    catch(Exception ex)
                    {
                        throw new Exception(response.ReasonPhrase + " " + response.RequestMessage + " " + response.StatusCode);
                    }
                    
                    //var result = response.ReasonPhrase + " " + response.RequestMessage + " " + response.StatusCode;
                }
                
            }
            return resultado;
        }
    }
    public enum TipoPeticionApi
    {
        Get, Post, Put, Delete
    }
}
