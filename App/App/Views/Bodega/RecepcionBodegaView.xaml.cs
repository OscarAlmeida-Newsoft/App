using App.BLL.Operaciones;
using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Bodega
{
    public partial class RecepcionBodegaView : ContentPage
    {
        public RecepcionBodegaView()
        {
            try
            {
                InitializeComponent();
                Title = "Recepción Bodega";
            }
            catch (Exception e)
            {

                DisplayAlert("error", e.Message, "Aceptar");
            }
            
        }

        async void btnConsultarPorNumeroEntrega_Clicked(object sender, EventArgs e)
        {
            
            if(txtNumeroEntrega.Text == null || txtNumeroEntrega.Text.IndexOf(".") != -1)
            {
                DisplayAlert("Alerta", "Ingrese Número Entrega válido", "Aceptar");
            }
            else
            {
                try
                {
                    EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();
                    var remesasPorEntrega = new List<RemesasPorNumeroEntrega>();

                    remesasPorEntrega = await eventoBLL.SeleccionarRemesaPorNumeroEntrega(Int32.Parse(txtNumeroEntrega.Text));

                    //var eventosBodega = remesasPorEntrega;

                    lvEventosBodega.ItemsSource = remesasPorEntrega;
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.StackTrace, "Aceptar");
                }
                
            }
        }
    }
}
