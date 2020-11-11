using App.BLL.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.ExtractoCondductores
{
    public partial class ExtractoConductoresView : ContentPage
    {
        public ExtractoConductoresView()
        {
            Title = "Extactos Viajes";
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            dtFechaInicial.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        }

        async void btnConsultarExtractoViajes_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
            ReporteExtractoConductoreBLL extractoBLL = new ReporteExtractoConductoreBLL();
            if (dtFechaFinal.Date != null && dtFechaInicial.Date != null)
            {
                var extractos = await extractoBLL.ObtenerExtractosPorFecha(dtFechaInicial.Date, dtFechaFinal.Date);


                if (extractos != null)
                {
                    lvExtractos.ItemsSource = extractos;
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
