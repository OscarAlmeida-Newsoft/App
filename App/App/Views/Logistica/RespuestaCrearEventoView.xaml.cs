using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Logistica
{
    public partial class RespuestaCrearEventoView : ContentPage
    {
        int idEvento = 0;
       
        public RespuestaCrearEventoView()
        {            
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var respuestaView = this.BindingContext as RespuestaProcesoEventoLogistico;
            if (respuestaView != null)
            {
                idEvento = respuestaView.IdTipoEvento;              
                if (respuestaView.ProcesadoCorrectamente)
                    imgIcono.Source = "correcto.png";
                else
                    imgIcono.Source = "incorrecto.png";
            }
        }
                
        public async void OnNavigateButtonClicked(object sender, EventArgs e)
        {
            Navigation.RemovePage(this);

            if(idEvento == 38 || idEvento == 39 || idEvento == 42 || idEvento == 43 || idEvento == 47)
            {
                await Navigation.PushAsync(new HomeView());
            }
            else
            {
                await Navigation.PushAsync(new Logistica.MenuEventosView());
            }
        }
    }
}
