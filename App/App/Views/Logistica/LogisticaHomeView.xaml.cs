using App.BLL.Turnos;
using App.Common;
using App.Entities.Turnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Logistica
{
    public partial class LogisticaHomeView : ContentPage
    {
        public LogisticaHomeView()
        {
            InitializeComponent();
            Title = "Logística";
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            
            StackLayout layout = new StackLayout();
            layout.Padding = new Thickness(10);

            Label lblPlaca = new Label();
            lblPlaca.Text = "Placa: ";
            layout.Children.Add(lblPlaca);

            Util util = new Util();
            if (util.UsuarioTienePermiso("logistica_buscar_placa"))
            {
                Entry txtPlaca = new Entry();                
                layout.Children.Add(txtPlaca);
            }
            else
            {
                EnturnamientoBLL enturnamientoBLL = new EnturnamientoBLL();
                List<Vehiculo> vehiculos = new List<Vehiculo>();
                vehiculos = await enturnamientoBLL.ObtenerCabezotesTurnosPorUsuarioActual();
                Picker pickerPlaca = new Picker();
                if (vehiculos != null && vehiculos.Count > 0)
                {                   
                    foreach (Vehiculo v in vehiculos)
                    {
                        pickerPlaca.Items.Add(v.Placa);
                    }
                }
                layout.Children.Add(pickerPlaca);
            }

            StackLayout layoutBotones = new StackLayout();
            layoutBotones.Orientation = StackOrientation.Horizontal;
            Button btn = new Button();
            btn.Text = "Aceptar";
            //btn.Clicked += Btn_Clicked;
            layoutBotones.Children.Add(btn);

            layout.Children.Add(layoutBotones);

            Content = layout;

            IsBusy = false;
        }
    }
}
