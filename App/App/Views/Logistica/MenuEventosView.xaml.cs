using App.BLL.Comercial;
using App.BLL.Operaciones;
using App.Common;
using App.DAO.IT;
using App.DAO.Operaciones;
using App.Entities;
using App.Entities.Comercial;
using App.Entities.Operaciones;
using App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Logistica
{
    public partial class MenuEventosView : ContentPage
    {
        public ListView Menu;
        
        private Transporte viajeActivo = null;

        public MenuEventosView()
        {           
            InitializeComponent();
            Title = "Eventos Logísticos";
            Padding = new Thickness(10, 20);

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            IsBusy = true;
            btnVerDocumentos.IsVisible = true;
            //activityIndicator.IsRunning = true;
            //activityIndicator.IsVisible = true;
            
            //********************************************** CREAR TABLA HISTORIALACTIVACION MANIFIESTO *******************************************//
            DatabaseDAO dao = new DatabaseDAO();
            EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();

            if (await ParametrosSistema.isOnline)
            {
                using (var db = DependencyService.Get<ISQLite>().GetConnection())
                {
                    db.DropTable<HistorialActivacionManifiesto>();
                    db.DropTable<Entrega>();
                    db.CreateTable(typeof(Entities.Operaciones.HistorialActivacionManifiesto));
                    db.CreateTable(typeof(Entities.Comercial.Entrega));

                }

                bool? aplicaTerceros = null;
                if (ParametrosSistema.PermisosUsuarioAlmacenado.Count(p => p.NombreOpcion.ToLower() == "eventoslogisticosterceros") > 0)
                {
                    aplicaTerceros = true;
                }
                

                var transportes = await eventoBLL.SeleccionarTransporteHabilitadoRegistroEventos(consultaLocal: false, tercero: aplicaTerceros);
                if (transportes != null && transportes.Count > 0)
                {
                    Transporte transporte = transportes[0];

                    HistorialActivacionManifiesto historial = new HistorialActivacionManifiesto();
                    historial.Activo = true;
                    historial.FechaActivacion = DateTime.Now;
                    historial.NumeroManifiesto = transporte.NumeroTransporte;
                    historial.Placa = transporte.Placa;
                    historial.NumeroDocConductor = transporte.NumeroDocConductor.ToString();
                    historial.NombreRuta = transporte.NombreRuta;
                    historial.UsuarioActivacion = ParametrosSistema.UsuarioActual;

                    //Se busca localmente el evento de activación del transporte activo
                    var eventosActivacionTransporte = eventoBLL.SeleccionarEventosLogisticos(transporte.NumeroTransporte, codigoTipoEvento: (int)TipoEventoLogisticoEnum.ActivarViaje, consultaLocal: true);
                    if (eventosActivacionTransporte != null & eventosActivacionTransporte.Count > 0)
                    {
                        historial.FechaActivacion = eventosActivacionTransporte[0].FechaEvento;
                    }


                    HistorialActivacionManifiestoDAO historialActivacionDAO = new HistorialActivacionManifiestoDAO();
                    historialActivacionDAO.GuardarHistorialActivacionManifiesto(historial);

                    //Se guardan las entregas del transporte
                    if (transporte.Entregas != null && transporte.Entregas.Count > 0)
                    {
                        EntregaBLL entregaBLL = new EntregaBLL();
                        foreach (Entrega entrega in transporte.Entregas)
                        {
                            try
                            {
                                entregaBLL.GuardarEntrega(entrega);
                            }
                            catch (Exception ex)
                            {
                                DisplayAlert("Error", ex.Message, "Aceptar");
                            }
                            
                        }
                    }
                }
            }

            //********************************************** FIN CREAR TABLA HISTORIALACTIVACION MANIFIESTO *******************************************//

            //EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();
            var tiposEventos = new List<TipoEventoLogistico>();

            //Se verifica si hay eventos pendientes por sincronizar
            var eventosPendientes = eventoBLL.SeleccionarEventosPendientesSincronizar();
            if(eventosPendientes!=null && eventosPendientes.Count>0 && await ParametrosSistema.isOnline)
            {
                IsBusy = false;
                await DisplayAlert("Atención","No puede registrar nuevos eventos porque tiene "+eventosPendientes.Count+"  eventos pendientes por sincronizar. Debe ir al menú 'Sincronización' para realizar la sincronización manualmente.","Aceptar");
                
            }
            else
            {
                Util util = new Util();
                bool? tercero = null;
                if (util.UsuarioTienePermiso("eventoslogisticosterceros"))
                {
                    tercero = true;
                }    

                List<Transporte> transportesHabilitadosRegistroEventos = new List<Transporte>();
                transportesHabilitadosRegistroEventos = await eventoBLL.SeleccionarTransporteHabilitadoRegistroEventos(! await ParametrosSistema.isOnline, tercero);
                if (transportesHabilitadosRegistroEventos != null && transportesHabilitadosRegistroEventos.Count > 0)
                    viajeActivo = transportesHabilitadosRegistroEventos[0];

                if(viajeActivo == null && tercero == true)
                {
                    DisplayAlert("Alerta", "Ud no posee un viaje activo, por lo tando no puede registrar eventos.", "Aceptar");
                    await Navigation.PushAsync(new HomeView());
                }
                else
                {
                    if (tercero.HasValue && tercero.Value)
                    {
                        if (viajeActivo != null)
                        {
                            try
                            {
                                tiposEventos = await eventoBLL.SeleccionarSiguienteEventoporManifiesto(viajeActivo.NumeroTransporte, consultaLocal: !await ParametrosSistema.isOnline);
                                lblNumeroViajeActivo.Text = viajeActivo.NumeroTransporte.ToString();
                                lblRuta.Text = viajeActivo.NombreRuta;
                            }
                            catch (Exception ex)
                            {
                                DisplayAlert("Alerta", "Ha ocurrido un inconveniente, por favor ingrese nuevamente.", "Aceptar");
                            }

                        }
                        else
                        {
                            tiposEventos = await eventoBLL.SeleccionarSiguienteEventoporManifiesto(0, consultaLocal: !await ParametrosSistema.isOnline);
                            lblNumeroViajeActivo.Text = "Sin viaje activo";
                            lblNumeroViajeActivo.TextColor = Color.Red;
                            btnVerDocumentos.IsVisible = false;
                        }
                    }
                    else
                    {
                        TransporteBLL transporteBLL = new TransporteBLL();
                        HistorialActivacionManifiesto historial = transporteBLL.SeleccionarHistorialManifiestoActivoPorConductor(ParametrosSistema.NumeroIdentificacionUsuarioActual);

                        if (historial != null)
                        {
                            tiposEventos = await eventoBLL.SeleccionarSiguienteEventoporManifiesto(historial.NumeroManifiesto, consultaLocal: !await ParametrosSistema.isOnline);
                            lblNumeroViajeActivo.Text = historial.NumeroManifiesto.ToString();
                            lblRuta.Text = historial.NombreRuta;
                        }
                        else
                        {
                            tiposEventos = await eventoBLL.SeleccionarSiguienteEventoporManifiesto(0, consultaLocal: !await ParametrosSistema.isOnline);
                            lblNumeroViajeActivo.Text = "Sin viaje activo";
                            lblNumeroViajeActivo.TextColor = Color.Red;
                            btnVerDocumentos.IsVisible = false;
                        }

                    }
                    //var dataTemplate = new DataTemplate(typeof(TextCell));
                    //dataTemplate.SetBinding(TextCell.TextProperty, "NombreEvento");


                    lvMenuEventos.ItemsSource = tiposEventos;
                    //lvMenuEventos.ItemTemplate = dataTemplate;

                    IsBusy = false;
                    //activityIndicator.IsRunning = false;
                    //activityIndicator.IsVisible = false;
                }

            }
            
        }


        async void  OnSelectionEvento(object sender, SelectedItemChangedEventArgs e)
        {
           
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }            
            var opcion = (TipoEventoLogistico)e.SelectedItem;
            //DisplayAlert("Evento seleccionado", e.SelectedItem.ToString(), "Ok");
            if (opcion.CodigoEvento != 24)
            {
                if(await ParametrosSistema.isOnline == false)
                {
                    //Se valida si el evento seleccionado se permite registras offline
                    if(opcion.RegistrarOffLine == false)
                    {
                        await DisplayAlert("Atención","No es posible registrar el evento "+opcion.NombreEvento+ " sin conexión a Internet.","Aceptar");
                        ((ListView)sender).SelectedItem = null;
                        return;
                    }
                }                
                await Navigation.PushAsync(new Logistica.CrearEventoView(opcion.CodigoEvento, opcion.NombreEvento));
                ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
            }
            else
            {
                Util util = new Util();
                bool? tercero = null;
                if (util.UsuarioTienePermiso("eventoslogisticosterceros"))
                {
                    tercero = true;
                }
                EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();
                List<TipoEventoLogistico> eventos = new List<TipoEventoLogistico>();
                //Si es la opcion de otros
                if (viajeActivo != null)
                {
                    eventos = await eventoBLL.SeleccionarEventosTipoOtros(viajeActivo.NumeroTransporte, aplicaTerceros:tercero);
                }                    
                else
                {
                    eventos = await eventoBLL.SeleccionarEventosTipoOtros(0, aplicaTerceros:tercero);
                }

                //var dataTemplate = new DataTemplate(typeof(TextCell));
                //dataTemplate.SetBinding(TextCell.TextProperty, "NombreEvento");

                lvMenuEventos.ItemsSource = eventos;
                //lvMenuEventos.ItemTemplate = dataTemplate;





                //Navigation.PushAsync(new Logistica.MenuEventosView(opcion.CodigoEvento));
                //((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
            }
            
        }

        void btnHistorialEventos_Clicked(object sender, EventArgs e)
        {        
            Navigation.PushAsync(new Logistica.HistorialEventosView());
        }

        private void btnVerDocumentos_Clicked(object sender, EventArgs e) {
            Device.OpenUri(new Uri("http://www.tdm.com.co/dv/Documento/List/" + lblNumeroViajeActivo.Text));
        }
    }
}
