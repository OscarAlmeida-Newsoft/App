using App.Common;
using App.Entities.IT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.IT
{
    public class NotificacionBLL
    {
        public Task<bool> RegistrarDispositivo(CodigoNotificacionAplicacionMovil codigo)
        {
            Task<bool> eventos;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "NotificacionApp/RegistrarDispositivo";
            eventos = ds.RealizarPeticionApi<bool>(url, DataService.TipoPeticionApi.Post,codigo);
            return eventos;

        }

        public Task<List<NotificacionAplicacionMovil>> ObtenerNotificacionesUsuarioActual()
        {
            Task<List<NotificacionAplicacionMovil>> notificaciones;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "NotificacionApp/ObtenerNotificacionesPorUsuario";
            notificaciones = ds.RealizarPeticionApi<List<NotificacionAplicacionMovil>>(url, DataService.TipoPeticionApi.Get);
            return notificaciones;
        }
    }
}
