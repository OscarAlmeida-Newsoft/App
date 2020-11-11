using App.BLL.IT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Notificaciones
{
    public partial class HistorialNotificacionesView : ContentPage
    {
        public HistorialNotificacionesView()
        {
            InitializeComponent();
            Title = "Notificaciones";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            NotificacionBLL notificacionBLL = new NotificacionBLL();

            try
            {
                var notificaciones = await notificacionBLL.ObtenerNotificacionesUsuarioActual();
                if(notificaciones!=null)
                {
                    lvNotificaciones.ItemsSource = notificaciones;
                }
            }
            catch (Exception ex)
            {
                   
            }

            IsBusy = false;
        }
    }
}
