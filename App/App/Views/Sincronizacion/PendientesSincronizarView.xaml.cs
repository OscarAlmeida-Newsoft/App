
using App.BLL.IT;
using App.BLL.Operaciones;
using App.Common;
using App.CustomCells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Sincronizacion
{
    public partial class PendientesSincronizarView : ContentPage
    {
        public PendientesSincronizarView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            
            EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();
            
            var eventos = eventoBLL.SeleccionarEventosPendientesSincronizar();
            var dataTemplate = new DataTemplate(typeof(TextCell));
            dataTemplate.SetBinding(TextCell.TextProperty, "NombreEvento");
            dataTemplate.SetBinding(TextCell.DetailProperty, "Detalles");


            //var dataTemplate = new DataTemplate(typeof(PendienteSincronizarCustomCell));
            //dataTemplate.SetBinding(PendienteSincronizarCustomCell.NombreTipoEventoProperty, "NombreTipoEvento");
            //dataTemplate.SetBinding(PendienteSincronizarCustomCell.NumeroManifiestoProperty, "NumeroManifiesto");
            //dataTemplate.SetBinding(PendienteSincronizarCustomCell.FechaEventoProperty, "FechaEvento");

            var eventosTexto = eventos.Select(e => new
            {
                NombreEvento = e.NombreTipoEvento + " IdApp: " + e.IdApp,
                Detalles = (e.FechaEvento.HasValue ? e.FechaEvento.Value.ToString("dd.MM.yyyy HH:mm a") : "--") + " , NumeroManifiesto: " + e.NumeroManifiesto.ToString(),
            }).ToList();


            lvPendientesSincronizar.ItemsSource = eventosTexto;
            lvPendientesSincronizar.ItemTemplate = dataTemplate;
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }

        async void btnSincronizar_Clicked(object sender, EventArgs e)
        {
            if(await ParametrosSistema.isOnline)
            {
                activityIndicator.IsRunning = true;
                activityIndicator.IsVisible = true;
                SincronizacionBLL bll = new SincronizacionBLL();
                await bll.SincronizarRegistrosPendientes();
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
            else
            {
                await DisplayAlert("Atención", "No es posible conectar con el servidor remoto. Verifique su conexión a Internet.", "Aceptar");
            }
            
        }
    }
}
