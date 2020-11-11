using System.Linq;
using Xamarin.Auth;
using Xamarin.Forms;
using App.Interfaces;
using App.Droid;
using System.Collections.Generic;

[assembly: Dependency(typeof(CredentialsService))]
namespace App.Droid
{
    public class CredentialsService : ICredentialsService
    {
        public string UserName
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }

        public string Password
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
            }
        }
        public string NumeroIdentificacion
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["NumeroIdentificacion"] : null;
            }
        }
        public string NombreCompleto
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).FirstOrDefault();
                if(account!=null && account.Properties !=null && account.Properties.ContainsKey("Nombre"))
                {
                    return account.Properties["Nombre"];
                }
                else
                {
                    return null;
                }
                
            }
        }
        public string Token
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["Token"] : null;
            }
        }

        public string Permisos
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).FirstOrDefault();
                if(account!=null && account.Properties != null && account.Properties.ContainsKey("Permisos"))
                {
                    return (account != null) ? account.Properties["Permisos"] : null;
                }
                else
                {
                    return null;
                }
                
            }
        }

        public void SaveCredentials(string userName, string password, string numeroIdentificacion, string nombre, string token)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("Password", password);
                account.Properties.Add("Token", token);
                if(!string.IsNullOrEmpty(numeroIdentificacion))
                    account.Properties.Add("NumeroIdentificacion", numeroIdentificacion);
                if (!string.IsNullOrEmpty(nombre))
                    account.Properties.Add("Nombre", nombre);
                AccountStore.Create(Forms.Context).Save(account, App.AppName);
            }
        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create(Forms.Context).Delete(account, App.AppName);
            }
        }


        public bool DoCredentialsExist()
        {
            return AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).Any() ? true : false;
        }

        public void SavePermissions(string permisos)
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                if(account.Properties.ContainsKey("Permisos"))
                {
                    account.Properties["Permisos"] = permisos;

                }
                else
                {
                    account.Properties.Add("Permisos", permisos);
                }
                
                AccountStore.Create(Forms.Context).Save(account, App.AppName);
            }
        }
    }
}