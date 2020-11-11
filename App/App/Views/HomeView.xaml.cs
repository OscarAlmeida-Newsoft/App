
using App.BLL.IT;
using App.BLL.Operaciones;
using App.BLL.Seguridad;
using App.Common;
using App.Entities;
using App.Entities.Seguridad;
using App.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views
{
    public partial class HomeView : ContentPage
    {
        public ListView Menu;
        
        public HomeView()
        {
            InitializeComponent();

            lblNombreCompletoUsuario.Text = Util.ToTitleCase("Hola " +ParametrosSistema.NombreCompletoUsuarioActual);
            
            //Se carga el menú en base a los permisos que tenga el usuario logueado
            Title = "Menú Principal";
            lblVersion.Text = "Versión " + ParametrosSistema.AppVersion;
            //Icon = "menu.png";

            Padding = new Thickness(10, 20);


        }

        protected async override void OnAppearing()
        {
            var opciones = new List<OpcionMenu>();

            if (ParametrosSistema.PermisosUsuarioAlmacenado != null)
            {
                foreach (PermisoAplicacion p in ParametrosSistema.PermisosUsuarioAlmacenado)
                {
                    if (p.NombreOpcion == "Turnos" && p.Acceso)
                    {
                        opciones.Add(new OpcionMenu("Turnos", () => new Turnos.TurnosView()));
                    }
                    if (p.NombreOpcion == "RegistroInformacionLogistica" && p.Acceso)
                    {
                        Util util = new Util();
                        if (util.UsuarioTienePermiso("registrar_evento_logistico"))
                        {
                            opciones.Add(new OpcionMenu("Logística", () => new Logistica.MenuEventosView()));
                            opciones.Add(new OpcionMenu("Historial Manifiestos", () => new Logistica.HistorialCalifiacionViajesView()));
                        }
                        //else
                        //    opciones.Add(new OpcionMenu("Logistica", () => new Logistica.LogisticaHomeView()));
                    }
                    if (p.NombreOpcion == "RegistroNovedadesTransporte" && p.Acceso)
                    {
                        //opciones.Add(new OpcionMenu("Novedades", () => new NovedadesView()));
                    }
                    if (p.NombreOpcion == "Almacenamiento" && p.Acceso)
                    {
                        opciones.Add(new OpcionMenu("Almacenamiento", () => new Almacenamiento.AlmacenamientoHomeView()));
                    }
                    if(p.NombreOpcion == "RegistrarEventosBodega" && p.Acceso)
                    {
                        opciones.Add(new OpcionMenu("Bodega", () => new Bodega.BodegaView()));
                    }
                }
            }
            opciones.Add(new OpcionMenu("Notificaciones", () => new Notificaciones.HistorialNotificacionesView()));
            opciones.Add(new OpcionMenu("Sincronización", () => new Sincronizacion.PendientesSincronizarView()));
            opciones.Add(new OpcionMenu("Extractos", () => new ExtractoCondductores.ExtractoConductoresView()));            
            opciones.Add(new OpcionMenu("Salir", () => new LoginView()));


            //var dataTemplate = new DataTemplate(typeof(TextCell));
            //dataTemplate.SetBinding(TextCell.TextProperty, "NombreOpcion");
            lvMenu.ItemsSource = opciones;
            //lvMenu.ItemTemplate = dataTemplate;
            

            //Content = lvMenu;
        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            //DisplayAlert("Item Selected", e.SelectedItem.ToString(), "Ok");
            var opcion = (OpcionMenu)e.SelectedItem;
            if(opcion!=null && opcion.NombreOpcion == "Salir")
            {
                SeguridadBLL seguridadBLL = new SeguridadBLL();
                seguridadBLL.EliminarCredencialesUsuario();
            }
            
            Navigation.PushAsync(opcion.PageFn());
            
            
            ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        }
        void OnButtonClick(object sender, EventArgs e)
        {
            
            string nombreOpcion = ((Button)sender).Text;            

            if (nombreOpcion == "Salir")
            {
                SeguridadBLL seguridadBLL = new SeguridadBLL();
                seguridadBLL.EliminarCredencialesUsuario();                
                Navigation.PushAsync(new LoginView());
            }

            if (nombreOpcion == "Turnos")
            {
               
                Navigation.PushAsync(new Turnos.TurnosView());
            }
            if (nombreOpcion == "Logística")
            {
               
                Navigation.PushAsync(new Logistica.MenuEventosView());
            }
            if (nombreOpcion == "Notificaciones")
            {
               
                Navigation.PushAsync(new Notificaciones.HistorialNotificacionesView());
            }
            if (nombreOpcion == "Sincronización")
            {
                
                Navigation.PushAsync(new Sincronizacion.PendientesSincronizarView());
            }
            if (nombreOpcion == "Historial Manifiestos")
            {

                Navigation.PushAsync(new Logistica.HistorialCalifiacionViajesView());
            }
            if (nombreOpcion == "Almacenamiento")
            {

                Navigation.PushAsync(new Almacenamiento.CrearInventarioContenedores());
            }

            if (nombreOpcion == "Extractos")
            {

                Navigation.PushAsync(new ExtractoCondductores.ExtractoConductoresView());
            }

            if (nombreOpcion == "Bodega")
            {
                Navigation.PushAsync(new Bodega.BodegaView());
            }


            ((ListView)lvMenu).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        }

        private void btnMiRegistroSalud_Clicked(object sender, EventArgs e)
        {
        
            Device.OpenUri(new Uri("http://www.tdm.com.co/reportesalud/Persona/Index?cedula=" + ParametrosSistema.NumeroIdentificacionUsuarioActual));
        }
    }
}
