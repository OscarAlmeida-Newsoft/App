using App.BLL.Operaciones;
using App.Common;
using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Bodega
{
    public partial class BodegaView : ContentPage
    {
        public BodegaView()
        {
            InitializeComponent();

            Title = "Eventos Bodega";
            //lblVersion.Text = "Versión " + ParametrosSistema.AppVersion;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;

            EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();
            var tiposEventos = new List<TipoEventoLogistico>();

            tiposEventos = await eventoBLL.SeleccionarEventosLogisticosBodega();

            lvMenuEventos.ItemsSource = tiposEventos;

            IsBusy = false;
        }

        async void OnSelectionEventoBodega(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            
            var opcion = (TipoEventoLogistico)e.SelectedItem;

            if(opcion.CodigoEvento == 38 || opcion.CodigoEvento == 39 || opcion.CodigoEvento == 42 || opcion.CodigoEvento == 43 || opcion.CodigoEvento == 47)
            {
                //Navigation.PushAsync(new RecepcionBodegaView());

                if (await ParametrosSistema.isOnline == false)
                {
                    //Se valida si el evento seleccionado se permite registras offline
                    if (opcion.RegistrarOffLine == false)
                    {
                        await DisplayAlert("Atención", "No es posible registrar el evento " + opcion.NombreEvento + " sin conexión a Internet.", "Aceptar");
                        ((ListView)sender).SelectedItem = null;
                        return;
                    }
                }
                await Navigation.PushAsync(new Logistica.CrearEventoView(opcion.CodigoEvento, opcion.NombreEvento));
                ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
            }

            

        }

        //public async void btnRecepBodegaClicked(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new RecepcionBodegaView());
        //}
    }
}
