using App.BLL.Operaciones;
using App.Entities.IT;
using App.Entities.Operaciones;
using App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.IT
{
    public class SincronizacionBLL
    {
        public async Task<bool> SincronizarRegistrosPendientes()
        {
            await SincronizarEventosLogisticos();
            return true;
        }

        public async Task EjecutarSincronizacionAutomatica(System.Threading.CancellationToken token)
        {
            bool ejecutarNuevamente = true;
            TimeSpan frecuenciaEjecucionSincronizacion = TimeSpan.FromMinutes(5);
            EventoLogisticoBLL eventosBLL = new EventoLogisticoBLL();
            var eventosPendientes = eventosBLL.SeleccionarEventosPendientesSincronizar();
            if (eventosPendientes != null && eventosPendientes.Count > 0)
            {
                ejecutarNuevamente = true;
            }
            else
            {
                ejecutarNuevamente = false;
            }
                
            while (ejecutarNuevamente)
            {

                token.ThrowIfCancellationRequested();

                if (await Common.ParametrosSistema.isOnline)
                {
                    eventosPendientes = eventosBLL.SeleccionarEventosPendientesSincronizar();
                    if (eventosPendientes != null && eventosPendientes.Count > 0)
                    {
                        int totalRegistrosSincronizados = 0;
                        try
                        {
                            List<RespuestaProcesoEventoLogistico> respuestas = new List<RespuestaProcesoEventoLogistico>();

                            foreach (EventoLogistico evento in eventosPendientes)
                            {

                                token.ThrowIfCancellationRequested();
                                respuestas.Add(await eventosBLL.GuardarEventoLogistico(evento));
                                totalRegistrosSincronizados++;
                                var message = new TickedMessage
                                {
                                    Message = ((decimal)totalRegistrosSincronizados / (decimal)eventosPendientes.Count).ToString()
                                };

                                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                {
                                    Xamarin.Forms.MessagingCenter.Send<TickedMessage>(message, "TickedMessage");
                                });

                            }

                        }
                        catch (Exception ex)
                        {


                        }
                        finally
                        {
                            if (totalRegistrosSincronizados == eventosPendientes.Count)
                            {
                                ejecutarNuevamente = false;
                            }
                            else
                            {
                                ejecutarNuevamente = false;
                            }
                        }
                        Xamarin.Forms.DependencyService.Get<INotificationService>().ShowLocalNotification("Sincronización automática", "Se han sincronizado " + eventosPendientes.Count + " registros.", DateTime.Now);

                    }
                    else
                    {
                        //Si no existen registros pendientes por sincronizar se detiene la ejecución de esta función
                        ejecutarNuevamente = false;
                    }
                }
                else
                {
                    ejecutarNuevamente = true;
                }
                await Task.Delay(Convert.ToInt32(frecuenciaEjecucionSincronizacion.TotalMilliseconds));
            }

            
        }

        private async Task<bool> SincronizarEventosLogisticos()
        {
            EventoLogisticoBLL eventosBLL = new EventoLogisticoBLL();
            var eventosPendientes = eventosBLL.SeleccionarEventosPendientesSincronizar();
            if(eventosPendientes != null && eventosPendientes.Count >0)
            {
                eventosPendientes = eventosPendientes.OrderBy(e => e.FechaEvento).ToList();
                List<RespuestaProcesoEventoLogistico> respuestas = new List<RespuestaProcesoEventoLogistico>();
                foreach(EventoLogistico evento in eventosPendientes)
                {
                    respuestas.Add(await eventosBLL.GuardarEventoLogistico(evento));
                }
                if(respuestas!=null)
                {

                }                
            }
            return true;
        }

    }
}
