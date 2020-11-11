using App.BLL.Comercial;
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
    public partial class InventarioContenedor : ContentPage
    {
        private ObservableCollection<Entities.Almacenamiento.DetalleInventarioContenedor> detallesInventario;
        private ObservableCollection<string> contenedoresEnPatioFiltrados = new ObservableCollection<string>();
        private List<Entities.Comercial.Entrega> contenedoresEnPatio;
        public InventarioContenedor(ObservableCollection<Entities.Almacenamiento.DetalleInventarioContenedor> inventario, List<Entrega> contenedoresEnPatio)
        {
            this.detallesInventario = inventario;
            this.contenedoresEnPatio = contenedoresEnPatio;
            InitializeComponent();

            autocompleteContenedor.PropertyChanged += (sender, e) => {
                if (e.PropertyName == "Text")
                {
                    autocompleteContenedor.Suggestions = new System.Collections.ObjectModel.ObservableCollection<object>();

                    var textEntry = autocompleteContenedor.Text;
                    if (textEntry != null && textEntry.Length > 0)
                    {
                        var temp = (from c in contenedoresEnPatio
                                    from d in c.Posiciones
                                    where d.NumeroContenedor.StartsWith(textEntry.ToUpper())
                                    select d.NumeroContenedor).ToList();

                        if (temp != null)
                        {
                            //contenedoresEnPatioFiltrados.Clear();
                            //contenedoresEnPatioFiltrados = new ObservableCollection<string>(temp);
                            //var s = autocompleteContenedor.AvailableSuggestions;
                            autocompleteContenedor.Suggestions = temp;
                            //OnPropertyChanged("contenedoresEnPatioFiltrados");
                        }
                    }
                }
            };

            autocompleteContenedor.TextChanged += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.NewTextValue) &&  e.NewTextValue.Length ==10)
                {
                    string numeroContenedor = e.NewTextValue;
                    txtLetrasContenedor.Text = numeroContenedor.Substring(0,3);
                    txtNumeroContenedor.Text = numeroContenedor.Substring(3, 6);
                    txtDigitoContenedor.Text = numeroContenedor.Substring(9, 1);
                }
            };
        }
        //void autocompleteContenedor_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    var temp = (from c in contenedoresEnPatio
        //                from d in c.Posiciones
        //                where d.NumeroContenedor.StartsWith(e.NewTextValue.ToUpper())
        //                select d.NumeroContenedor).ToList();
        //    if(temp!=null)
        //    {
        //        //contenedoresEnPatioFiltrados.Clear();
        //        //contenedoresEnPatioFiltrados = new ObservableCollection<string>(temp);
        //        //var s = autocompleteContenedor.AvailableSuggestions;
        //        autocompleteContenedor.Suggestions = temp;
        //        //OnPropertyChanged("contenedoresEnPatioFiltrados");
        //    }
        //}

        

        async void btnAgregar_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("mensaje", autocompleteContenedor.Text,"cancelar");
            if( (String.IsNullOrEmpty(autocompleteContenedor.Text) || txtLetrasContenedor.Text.Length < 3)
                || (String.IsNullOrEmpty(txtNumeroContenedor.Text) || txtNumeroContenedor.Text.Length < 6) 
                || (String.IsNullOrEmpty(txtDigitoContenedor.Text) || txtDigitoContenedor.Text.Length<1))
            {
                await DisplayAlert("Error", "Número de contenedor incompleto.", "Aceptar");
                return;
            }
            if(ddlEstadoContenedor.SelectedIndex <0)
            {
                await DisplayAlert("Error", "Debe seleccionar el estado del contenedor.", "Aceptar");
                return;
            }
            Entities.Almacenamiento.DetalleInventarioContenedor contenedor = new Entities.Almacenamiento.DetalleInventarioContenedor();
            contenedor.NumeroContenedor = txtLetrasContenedor.Text.ToUpper()+ txtNumeroContenedor.Text + txtDigitoContenedor.Text;
            contenedor.EstadoContenedor = ddlEstadoContenedor.Items[ddlEstadoContenedor.SelectedIndex];
            switch (contenedor.EstadoContenedor.ToLower())
            {
                case "vacío":
                    contenedor.EstadoContenedor = "02";
                    break;
                case "lleno":
                    contenedor.EstadoContenedor = "01";
                    break;
            }
            if(ddlTamanoContenedor.SelectedIndex >= 0)
            {
                var tamanoSeleccionado = ddlTamanoContenedor.Items[ddlTamanoContenedor.SelectedIndex];
                switch (tamanoSeleccionado.ToLower())
                {
                    case "20 pies":
                        contenedor.TamanoContenedor = 20;
                        break;
                    case "40 pies":
                        contenedor.TamanoContenedor = 40;
                        break;
                }
            }

            var contenedorAgregado = (from d in detallesInventario
                                      where d.NumeroContenedor == contenedor.NumeroContenedor
                                      select d).FirstOrDefault();
            if (contenedorAgregado != null)
            {
                if (await DisplayAlert("Alerta", "El Contenedor que intenta agregar ya ha sido agregado. Desea sobreescribir la información ingresada previamente ? ", "SI", "NO"))
                {
                    detallesInventario.Remove(contenedorAgregado);
                    detallesInventario.Add(contenedor);
                    await Navigation.PopModalAsync();
                }
                else
                {
                    return;
                }               
            }
            else
            {
                detallesInventario.Add(contenedor);
                await Navigation.PopModalAsync();
            }
        }
    }
}
