
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Interfaces
{
    public interface ICredentialsService
    {
        string UserName { get; }

        string Password { get; }

        string Token { get; }
        string NumeroIdentificacion { get; }
        string NombreCompleto { get; }

        string Permisos { get; }            

        void SaveCredentials(string userName, string password, string numeroIdentificacion, string nombre, string token);

        void DeleteCredentials();

        bool DoCredentialsExist();

        void SavePermissions(string permisos);
    }
}
