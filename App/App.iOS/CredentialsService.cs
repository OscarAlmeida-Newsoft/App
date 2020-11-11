using System.Linq;
using App.iOS;
using Xamarin.Auth;
using Xamarin.Forms;
using App.Interfaces;

[assembly: Dependency(typeof(CredentialsService))]
namespace App.iOS
{
    public class CredentialsService : ICredentialsService
    {
        public string UserName
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }

        public string Password
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
            }
        }
        public string Token
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["Token"] : null;
            }
        }
        public string Permisos
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["Permisos"] : null;
            }
        }

        public string NumeroIdentificacion
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["NumeroIdentificacion"] : null;
            }
        }
        public string NombreCompleto
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["Nombre"] : null;
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
                if (!string.IsNullOrEmpty(numeroIdentificacion))
                    account.Properties.Add("NumeroIdentificacion", numeroIdentificacion);
                if (!string.IsNullOrEmpty(nombre))
                    account.Properties.Add("Nombre", nombre);
                AccountStore.Create().Save(account, App.AppName);
            }

        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, App.AppName);
            }
        }

        public bool DoCredentialsExist()
        {
            return AccountStore.Create().FindAccountsForService(App.AppName).Any() ? true : false;
        }

        public void SavePermissions(string permisos)
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                account.Properties.Add("Permisos", permisos);
                AccountStore.Create().Save(account, App.AppName);
            }
        }
    }
}
