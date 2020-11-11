using App.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.DataService
{
    public class AppDataService
    {
        public async Task<T> RealizarPeticionApi<T>(string url, TipoPeticionApi tipoPeticion, object data = null)
        {
            T resultado = default(T);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ParametrosSistema.ApiURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestUri = url;

               
                var json = JsonConvert.SerializeObject(data);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                // New code:
                HttpResponseMessage response = await  client.PostAsync(requestUri, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var content =  await response.Content.ReadAsStringAsync();
                    resultado = JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    var result = response.ReasonPhrase + " " + response.RequestMessage + " " + response.StatusCode;
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
