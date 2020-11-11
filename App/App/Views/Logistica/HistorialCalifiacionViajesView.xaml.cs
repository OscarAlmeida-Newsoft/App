using App.BLL.Operaciones;
using App.Common;
using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Logistica
{
    public partial class HistorialCalifiacionViajesView : ContentPage
    {
        public HistorialCalifiacionViajesView()
        {
            Title = "Calificación Viajes";
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            dtFechaInicial.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            
        }

        async void btnConsultarHistorialCalificacionViajes_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
            CalificacionEventoLogisticoBLL califiacionBLL = new CalificacionEventoLogisticoBLL();
            if (dtFechaFinal.Date != null && dtFechaInicial.Date != null)
            {
                var calificaciones = await califiacionBLL.ObtenerCalificacionesEventosLogisticos(dtFechaInicial.Date, dtFechaFinal.Date, ParametrosSistema.NumeroIdentificacionUsuarioActual);

                if (calificaciones != null)
                {
                    foreach (var calificacion in calificaciones)
                    {
                        if (calificacion.CalificacionManual != 0)
                        {
                            calificacion.CalificacionAutomatica = calificacion.CalificacionManual;
                            calificacion.ObservacionCalificacionAutomatica = calificacion.ObservacionCalificacionManual;
                        }
                    }
                    lvCalificaciones.ItemsSource = calificaciones;
                }
            }
            else
            {
                DisplayAlert("Fecha", "Debe seleccionar una fecha inicial y una fecha final", "Aceptar");
            }            
            IsBusy = false;

        }

    }
}
