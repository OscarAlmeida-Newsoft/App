using App.Entities.Seguridad;
using App.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Reflection;
using System.Text.RegularExpressions;

namespace App.Common
{
    public class Util
    {
        public bool UsuarioTienePermiso(string permiso)
        {
            bool result = false;
            List<PermisoAplicacion> permisosUsuario = null;
            string jsonPermisos = DependencyService.Get<ICredentialsService>().Permisos;
            if(jsonPermisos!=null)
            {
                permisosUsuario = JsonConvert.DeserializeObject<List<PermisoAplicacion>>(jsonPermisos);
            }
            if (permisosUsuario != null)
            {
                var per = (from p in permisosUsuario
                           where p.NombreOpcion.ToLower() == permiso && p.Acceso
                           select p).FirstOrDefault();

                if (per != null)
                    result = true;
            }
            return result;
        }

        /// <summary>
        /// Establece el valor a una propiedad de un objeto indicado
        /// </summary>
        /// <param name="obj">Objeto al cual se le debe establecer el valor.</param>
        /// <param name="propertyName">Propiedad del objeto en la cual se debe establecer el valor</param>
        /// <param name="value">Valor que se desea establecer</param>
        public static void EstablecerValorDinamicamente(object obj, string propertyName, object value)
        {

            var propertyInfo = obj.GetType().GetRuntimeProperty(propertyName);
            if (propertyInfo == null) return;

            

            var convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
            if(convertedValue!=null)
            {
                propertyInfo.SetValue(obj, convertedValue);
            }
            
        }
        /// <summary>
        /// Obtiene el valor a una propiedad de un objeto indicado
        /// </summary>
        /// <param name="obj">Objeto al cual se le debe establecer el valor.</param>
        /// <param name="propertyName">Propiedad del objeto del cual se debe obtener el valor</param>        
        public static string ObtenerValorDinamicamente(object obj, string propertyName)
        {
            string valor = string.Empty;
            if(obj !=null)
            {
                var propertyInfo = obj.GetType().GetRuntimeProperty(propertyName);
                if (propertyInfo == null) return valor;

                if (propertyInfo.GetValue(obj) != null)
                    valor = propertyInfo.GetValue(obj).ToString();                
            }
            return valor;

        }

        public string RemoveAccentsWithRegEx(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]");
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]");
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]");
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]");
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]");
            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_u_Accents.Replace(inputString, "u");
            return inputString;
        }

        /// <summary>
        /// Convierte el texto en titpo titulo (La primera letra de cada palabra mayuscula)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToTitleCase(string text)
        {            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if(!String.IsNullOrEmpty(text))
            {

                try
                {
                    var textArray = text.Split(' ');
                    foreach (string word in textArray)
                    {
                        string newWord = word.ToLower().Replace(" ", "").Trim();
                        if (!String.IsNullOrEmpty(newWord))
                        {
                            newWord = newWord.Substring(0, 1).ToUpper() + newWord.Substring(1);
                            sb.Append(newWord);
                            sb.Append(' ');
                        }


                    }
                }
                catch (Exception ex)
                {

                    
                }
            }
            
            return sb.ToString();
        }

    }
}
