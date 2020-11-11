using App.BLL.Comercial;
using App.BLL.Operaciones;
using App.Common;
using App.Entities.Comercial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Almacenamiento
{
    public partial class CrearInventarioContenedores : ContentPage
    {
        private ObservableCollection<Entities.Almacenamiento.DetalleInventarioContenedor> detallesInventario;
        private List<Entrega> entregasContenedoresEnPatio = new List<Entrega>();
        
        public CrearInventarioContenedores()
        {
            detallesInventario = new ObservableCollection<Entities.Almacenamiento.DetalleInventarioContenedor>();
            InitializeComponent();
            lvInventario.ItemsSource = detallesInventario;
            ConfigurarControlesNuevoInventario();
            
        }

        private void ConfigurarControlesNuevoInventario()
        {
            detallesInventario.Clear();
            ddlPuesto.SelectedIndex = -1;            
            lblFechaInventario.Text = DateTime.Now.ToString("dd-MM-yyyy");
            
            
        }

        async void ddlPuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (ddlPuesto.SelectedIndex >= 0)
            {
                if (await DisplayAlert("Alerta", "Seguro desea generar un inventario para el centro " + ddlPuesto.Items[ddlPuesto.SelectedIndex] + "?.", "SI", "NO"))
                {
                    ddlPuesto.IsEnabled = false;
                    IsBusy = true;
                    if (await ParametrosSistema.isOnline)
                    {
                        EntregaBLL entregaBLL = new EntregaBLL();
                        entregasContenedoresEnPatio = await entregaBLL.ObtenerEntregasDeContenedoresEnPatio(ObtenerCodigoPuestoSeleccionado());
                    }
                    IsBusy = false;
                    btnAgregarItem.IsEnabled = true;
                }
            }
            else
            {
                ddlPuesto.IsEnabled = true;
                btnAgregarItem.IsEnabled = false;
            }
        }

        

        async void btnAgregarItem_Clicked(Object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new InventarioContenedor(detallesInventario, entregasContenedoresEnPatio));
        }

        async void btnNuevoInventario_Clicked(Object sender, EventArgs e)
        {
            if(detallesInventario!=null && detallesInventario.Count>0)
            {
                if (! await DisplayAlert("Atención","Se perderá el inventario actual porque no lo ha enviado al servidor. Desea continuar?","Aceptar","Cancelar"))
                {
                    return;
                }
            }
            ConfigurarControlesNuevoInventario();
        }

        async void btnEnviarInventario_Clicked(Object sender, EventArgs e)
        {
            if(detallesInventario == null || detallesInventario.Count <= 0)
            {
                await DisplayAlert("Error","No hay items en el inventario","Aceptar");
            }
            else
            {
                Entities.Almacenamiento.InventarioContenedor inventario = new Entities.Almacenamiento.InventarioContenedor();
                inventario.Fecha = DateTime.Now;
                inventario.Puesto = ObtenerCodigoPuestoSeleccionado(); 
                
                inventario.Detalles = new List<Entities.Almacenamiento.DetalleInventarioContenedor>(detallesInventario);
                AlmacenamientoBLL almacenamientoBLL = new AlmacenamientoBLL();
                IsBusy = true;
                try
                {
                    inventario = await almacenamientoBLL.GuardarInventarioContenedor(inventario);
                    if (inventario.Id != default(long))
                    {
                        await DisplayAlert("Infomración", "Se ha guardado correctamente el inventario.", "Aceptar");
                        ConfigurarControlesNuevoInventario();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Ha ocurrido un error guardando el inventario", "Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Ha ocurrido un error guardando el inventario", "Aceptar");
                }
                finally
                {
                    IsBusy = false;
                }



            }


        }

        private string ObtenerCodigoPuestoSeleccionado()
        {
            string nombrePuesto = ddlPuesto.Items[ddlPuesto.SelectedIndex];
            string codigoPuesto = String.Empty;
            if(ddlPuesto.SelectedIndex>=0)
            {
                switch (nombrePuesto.ToLower())
                {
                    case "girardota":
                        codigoPuesto = "TD01";
                        break;
                    case "buenaventura":
                        codigoPuesto = "TD02";
                        break;
                    case "bogotá":
                        codigoPuesto = "TD03";
                        break;
                    case "cartagena":
                        codigoPuesto = "TD04";
                        break;
                    case "cali":
                        codigoPuesto = "TD05";
                        break;
                    case "barranquilla":
                        codigoPuesto = "TD06";
                        break;
                }
            }
            
            return codigoPuesto;
        }


    }
}
