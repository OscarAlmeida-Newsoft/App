using App.Common;
using App.Entities.Operaciones;
using App.Entities.Turnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Operaciones
{
    public class SatelitalBLL
    {
        public Task<PosicionSatelitalVehiculo> ObtenerUltimoKilometrajePorPlaca(Vehiculo vehiculo)
        {
            Task<PosicionSatelitalVehiculo> posicion;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Satelital/ObtenerPosicionSatelitalVehiculo";
            posicion = ds.RealizarPeticionApi<PosicionSatelitalVehiculo>(url, DataService.TipoPeticionApi.Post,vehiculo);
                    
            return posicion;
        }
    }
}
