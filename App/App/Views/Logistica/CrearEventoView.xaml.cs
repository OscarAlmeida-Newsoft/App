using App.BLL.Comercial;
using App.BLL.Operaciones;
using App.Common;
using App.Entities.Comercial;
using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using XLabs.Forms.Controls;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using XLabs.Platform.Services.Media;
using DevExpress.Mobile.IO;
using App.Interfaces;
using System.IO;
using App.Entities.Turnos;
using Android.Content;
using App.Entities.GPS;
using App.DataService;
using Android.App;
using Android.Locations;
using System.Text;
using Android.OS;
using Android.Runtime;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using Plugin.Media;
using CoreTDM.Entities.Operaciones;
using App.BLL.Documentos;

namespace App.Views.Logistica
{
    public partial class CrearEventoView : ContentPage, ILocationListener
    {
        static readonly string TAG = "X:" + typeof(CrearEventoView).Name;
        //TextView _addressText;
        Location _currentLocation;
        LocationManager _locationManager;

        string _locationProvider;
        //TextView _locationText;

        Context mContext;
        //***Obtener coordenadas GPS
        //GPSServiceBinder _binder;
        //GPSServiceConnection _gpsServiceConnection;
        Intent _gpsServiceIntent;

        //Location _currentLocation;
        //LocationManager _locationManager;

        //string _locationProvider;
        //***
        private string _horaInicio = "";
        private string _fechaInicio = "";
        private int _numeroManifiestoBodega = 0;
        private string _remesasViaje = "";

        private int _idTipoEvento = 0;
        private string _nombreEvento;
        private List<CampoEventoLogistico> _camposTipoEvento;

        private Button btnGuardar; 
        private Button btnNuevaFoto;
        private Button btnSeleccionarFoto;
        private Button btnVerEntrega;
        private Button btnFirmarEntrega;
        private Image img;

        private BindableRadioGroup radioGroup;

        StackLayout layoutImages = new StackLayout();
        ListView lvImagenes = new ListView();
        List<AdjuntoEventoLogistico> adjuntos = new List<AdjuntoEventoLogistico>();
        List<string> datos;

        StackLayout layoutInfoEntrega = new StackLayout();

        public IntPtr Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        //View input = new Entry();

        //private ActivityIndicator _guardarActivityIndicator = new ActivityIndicator();

        public CrearEventoView(int idTipoEvento, string nombreEvento)
        {
            _idTipoEvento = idTipoEvento;
            _nombreEvento = nombreEvento;
            InitializeComponent();
            InitializeLocationManager();
            Title = _nombreEvento+ " (crear)";
        }

       

        public CrearEventoView(Context mContext)
        {
            mContext = mContext;
        }      

        protected async override void OnAppearing()
        {
            base.OnAppearing();

          
            StackLayout layout = new StackLayout();

            StackLayout layoutSubItems = new StackLayout();

            layout.Padding = new Thickness(10);

            ScrollView scrollview = new ScrollView
            {
                Content = layout                
            };
                       
            EventoLogisticoBLL eventoLogisticosBLL = new EventoLogisticoBLL();

            _camposTipoEvento = await eventoLogisticosBLL.SeleccionarCamposPorEvento(_idTipoEvento, consultaLocal:true);
            if (_camposTipoEvento != null)
            {
                Label label;
                View input = new Entry();

                List<ItemCampoEventoLogistico> listaItem;

                PosicionSatelitalVehiculo posicion = null;

                if (_idTipoEvento == (int)TipoEventoLogisticoEnum.SolicitudTanqueo)
                {
                    TransporteBLL transporteBLL = new TransporteBLL();
                    HistorialActivacionManifiesto historialActivacion = transporteBLL.SeleccionarHistorialManifiestoActivoPorConductor(ParametrosSistema.NumeroIdentificacionUsuarioActual);
                    if (historialActivacion != null)
                    {
                        SatelitalBLL satelitalBLL = new SatelitalBLL();

                        Vehiculo vehiculo = new Vehiculo();
                        vehiculo.Placa = historialActivacion.Placa;
                        vehiculo.FechaPosicion = DateTime.Now;
                        vehiculo.MinutosTolerancia = 10;

                        posicion = await satelitalBLL.ObtenerUltimoKilometrajePorPlaca(vehiculo);
                    }
                   
                }

                var conteo = 0;
                

                foreach (CampoEventoLogistico campo in _camposTipoEvento)
                {                   
                    if (ParametrosSistema.EventoConCampoEntregas.Contains(_idTipoEvento) && campo.NombreCampoEventoLogistico == "campo1")
                    {                                              
                        campo.TipoCampoEventoLogistico = "dynamic_checkbox_select";
                    }
                        

                    label = new Label();                    
                    label.Text = campo.TituloCampoEventoLogistico;
                    layout.Children.Add(label);
                                        
                    if (campo.TituloCampoEventoLogistico.ToLower() == "hora")
                    {
                        campo.TipoCampoEventoLogistico = "hour";
                    }

                    switch (campo.TipoCampoEventoLogistico.ToLower())
                    {
                        case "text":
                            input = new Entry();                            
                            break;
                        case "date":
                            input = new DatePicker();
                            ((DatePicker)input).Date = DateTime.Now;
                            ((DatePicker)input).Format ="dd.MM.yyyy";

                            _fechaInicio = DateTime.Now.ToString("yyyy MM dd");

                            if ((_idTipoEvento == 38 || _idTipoEvento == 39) && campo.NombreCampoEventoLogistico == "campo4")
                            {
                                input.IsVisible = false;
                            }
                            break;
                        case "hour":
                            input = new TimePicker() { Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute,0)};

                            _horaInicio = DateTime.Now.ToString("HH:mm:ss");

                            if ((_idTipoEvento == 38 || _idTipoEvento == 39) && campo.NombreCampoEventoLogistico == "campo5")
                            {
                                input.IsVisible = false;
                            }
                            break;
                        case "checkbox":
                            input = new Switch();
                            break;
                        case "number":
                            input = new Entry();
                            ((Entry)input).Keyboard = Keyboard.Numeric;
                            
                            if (_idTipoEvento == (int)TipoEventoLogisticoEnum.SolicitudTanqueo && posicion != null && campo.NombreCampoEventoLogistico== "campo3")
                            {
                                if (posicion.Odometro.HasValue)
                                { input = new Entry() { Text = posicion.Odometro.Value.ToString() }; }
                            }
                            break;
                        case "select":
                            Picker inputSelect = new Picker();
                            listaItem = new List<ItemCampoEventoLogistico>();
                            listaItem = await eventoLogisticosBLL.SeleccionarItemsPorCamposEvento(_idTipoEvento, campo.NombreCampoEventoLogistico);
                            List<SubItemCampoEventoLogistico> listaSubItems = new List<SubItemCampoEventoLogistico>();
                            listaSubItems = await eventoLogisticosBLL.SeleccionarSubItemsPorCamposEvento(null);

                            if (listaItem != null && listaItem.Count>0)
                            {
                                foreach (ItemCampoEventoLogistico item in listaItem)
                                {
                                    inputSelect.Items.Add(item.Nombre);
                                }                               

                                inputSelect.SelectedIndexChanged += (sender, args) =>
                                {
                                    string motivo = inputSelect.Items[inputSelect.SelectedIndex].ToString();
                                    if (!string.IsNullOrEmpty(motivo))
                                    {
                                        ItemCampoEventoLogistico itemSeleccionado = (from i in listaItem
                                                                                     where i.Nombre.ToLower().Trim() == motivo.ToLower().Trim()
                                                                                     select i).FirstOrDefault();
                                        if (itemSeleccionado!=null && itemSeleccionado.TieneSubItems)
                                        {
                                            List<SubItemCampoEventoLogistico> subItems = (from s in listaSubItems
                                                                                          where s.IdItem == itemSeleccionado.Id
                                                                                          select s).ToList();
                                            
                                            if (subItems != null && subItems.Count>0)
                                            {
                                                Label labelSubItem= new Label();
                                                View inputSubItem = new Entry();
                                                foreach (SubItemCampoEventoLogistico subItem in subItems)
                                                {
                                                    labelSubItem.Text = subItem.Titulo;
                                                    layoutSubItems.Children.Add(labelSubItem);

                                                    switch (subItem.TipoCampo)
                                                    {
                                                        case "text":
                                                            inputSubItem = new Entry();                                                            
                                                            break;
                                                        case "slider":
                                                            Slider slider = new Slider();                   
                                                            JObject jsonObjPropiedades = JObject.Parse(subItem.Propiedades);
                                                            Dictionary<string, string> dictPropiedades = jsonObjPropiedades.ToObject<Dictionary<string, string>>();
                                                            slider.Minimum = Convert.ToDouble(dictPropiedades["Minimum"]);
                                                            slider.Maximum = Convert.ToDouble(dictPropiedades["Maximum"]);
                                                            slider.Value = Convert.ToDouble(dictPropiedades["Value"]);
                                                            
                                                            DateTime fechaReinicio = DateTime.Now.AddHours(slider.Value);
                                                            labelSubItem.Text = subItem.Titulo + "   " + fechaReinicio.ToString();

                                                            slider.ValueChanged += (s, e) =>
                                                            {
                                                                fechaReinicio = DateTime.Now.AddHours(e.NewValue);
                                                                labelSubItem.Text = subItem.Titulo + "   " + fechaReinicio.ToString();
                                                            };
                                                            inputSubItem = slider;
                                                            break;
                                                    }                                                   
                                                    inputSubItem.StyleId = subItem.CampoEventoLogistico;
                                                    layoutSubItems.Children.Add(inputSubItem);
                                                    
                                                }
                                            }
                                        }
                                    }
                                };
                            }
                            input = inputSelect;
                            break;
                        case "dynamic_select":
                            input = new Picker();
                            if (_idTipoEvento == (int)TipoEventoLogisticoEnum.ActivarViaje && campo.NombreCampoEventoLogistico == "NumeroManifiesto")
                            {
                                List<Transporte> transportesProgramados = new List<Transporte>();
                                TransporteBLL transporteBLL = new TransporteBLL();
                                transportesProgramados = await transporteBLL.SeleccionarTransportesPendientesActivacionPorConductor();
                                if (transportesProgramados != null && transportesProgramados.Count > 0)
                                {
                                    BindableRadioGroup rb = new BindableRadioGroup();                                    
                                    List<string> datos = new List<string>();                                   
                                    foreach (Transporte t in transportesProgramados)
                                    {
                                        datos.Add(t.NumeroTransporte.ToString() + " - " + t.NombreRuta + " - " + t.PlacaTrailer);
                                    }
                                    rb.ItemsSource = datos;                                    
                                    input = rb;
                                }
                            }
                            if (_idTipoEvento == (int)TipoEventoLogisticoEnum.ReanudarViaje && campo.NombreCampoEventoLogistico == "NumeroManifiesto")
                            {
                                List<Transporte> transportesSuspendidos = new List<Transporte>();
                                TransporteBLL transporteBLL = new TransporteBLL();
                                transportesSuspendidos = await transporteBLL.SeleccionarTransportesSuspendidosPorConductor();
                                if (transportesSuspendidos != null && transportesSuspendidos.Count > 0)
                                {
                                    BindableRadioGroup rb = new BindableRadioGroup();
                                    List<string> datos = new List<string>();
                                    foreach (Transporte t in transportesSuspendidos)
                                    {
                                        datos.Add(t.NumeroTransporte.ToString() + " - " + t.NombreRuta + " - " + t.PlacaTrailer);
                                    }
                                    rb.ItemsSource = datos;
                                    input = rb;
                                }
                            }
                            if (_idTipoEvento == (int)TipoEventoLogisticoEnum.DevolucionContenedor && campo.NombreCampoEventoLogistico == "campo1")
                            {
                                EntregaBLL entregaBLL = new EntregaBLL();
                                List<Entrega> entregas = new List<Entrega>();
                                HistorialActivacionManifiesto historialActivacion = null;
                                TransporteBLL transporteBLL = new TransporteBLL();
                                historialActivacion = transporteBLL.SeleccionarHistorialManifiestoActivoPorConductor(ParametrosSistema.NumeroIdentificacionUsuarioActual);
                                if (historialActivacion != null)
                                {
                                    entregas = entregaBLL.SeleccionarEntregasPorTransporteSinEventoLogistico(historialActivacion.NumeroManifiesto, _idTipoEvento);
                                    if (entregas != null && entregas.Count > 0 && entregas[0] != null)
                                    {
                                        BindableRadioGroup rb = new BindableRadioGroup();
                                        List<string> datos = new List<string>();
                                        foreach (Entrega e in entregas)
                                        {
                                            datos.Add(e.NumeroEntrega.ToString());
                                        }
                                        rb.ItemsSource = datos;
                                        input = rb;
                                    }
                                }
                            }
                            if (_idTipoEvento == (int)TipoEventoLogisticoEnum.ReportarUbicacion) {

                                TransporteBLL transporteBLL = new TransporteBLL();
                                HistorialActivacionManifiesto historialActivacion = null;
                                historialActivacion = transporteBLL.SeleccionarHistorialManifiestoActivoPorConductor(ParametrosSistema.NumeroIdentificacionUsuarioActual);
                                List<PuestoControlManifiesto> puestosControl = new List<PuestoControlManifiesto>();

                                if (historialActivacion != null) {
                                    puestosControl = await transporteBLL.ObtenerPuestosControlPendientesPorManifiesto(historialActivacion.NumeroManifiesto);

                                    if (puestosControl != null && puestosControl.Count > 0 && puestosControl[0] != null)
                                    {
                                        inputSelect = new Picker();
                                        foreach (PuestoControlManifiesto e in puestosControl)
                                        {
                                            inputSelect.Items.Add(e.NombrePuestoControl);
                                        }

                                        input = inputSelect;
                                        
                                    }
                                }
                                
                            }
                            break;
                        case "dynamic_checkbox_select":
                            if (ParametrosSistema.EventoConCampoEntregas.Contains(_idTipoEvento) && campo.NombreCampoEventoLogistico == "campo1")
                            {
                                input = new Grid();
                                EntregaBLL entregaBLL = new EntregaBLL();
                                List<Entrega> entregas = new List<Entrega>();
                                HistorialActivacionManifiesto historialActivacion = null;                              
                                TransporteBLL transporteBLL = new TransporteBLL();
                                historialActivacion = transporteBLL.SeleccionarHistorialManifiestoActivoPorConductor(ParametrosSistema.NumeroIdentificacionUsuarioActual);
                                if (historialActivacion != null)
                                {
                                    entregas = entregaBLL.SeleccionarEntregasPorTransporteSinEventoLogistico(historialActivacion.NumeroManifiesto, _idTipoEvento);
                                    if (entregas != null && entregas.Count > 0 && entregas[0]!=null)
                                    {
                                        int columnas = 2;
                                        int filas = (Int32)Math.Ceiling(((decimal)entregas.Count / (decimal)2));
                                        Grid gridEntregas = new Grid()
                                        {
                                            ColumnSpacing = 1,
                                            RowSpacing = 1,
                                            VerticalOptions = LayoutOptions.Start,
                                        };

                                        int contadorEntregas = 0;
                                        for (int fila = 0; fila < filas; fila++)
                                        {
                                            for (int col = 0; col < columnas; col++)
                                            {
                                                if (contadorEntregas < entregas.Count)
                                                {
                                                    CheckBox chk = new CheckBox() {
                                                        // se agrega el nombre del cliente a la entrega
                                                        DefaultText = entregas[contadorEntregas].NumeroEntrega.ToString() + " - " + entregas[contadorEntregas].NombreCliente
                                                        
                                                    };
                                                    gridEntregas.Children.Add(chk, col, fila);
                                                    contadorEntregas++;
                                                }

                                            }
                                        }
                                        input = gridEntregas;
                                        layout.Children.Add(input);
                                    }
                                }
                            }
                                break;                             
                    }
                    input.IsEnabled = !campo.SoloLectura;
                    input.StyleId = campo.NombreCampoEventoLogistico;
                    layout.Children.Add(input);

                    if (_idTipoEvento == 38 || _idTipoEvento == 39 || _idTipoEvento == 42 || _idTipoEvento == 43)
                    {
                        if(conteo == 0)
                        {
                            layout.Children.Add(layoutInfoEntrega);
                            
                            Button btnConsultar = new Button();
                            btnConsultar.Text = "Consultar";
                            btnConsultar.Clicked += Btn_Clicked;
                            //layoutBotones.Children.Add(btn);
                            layout.Children.Add(btnConsultar);
                            conteo++;
                        }
                    }
                }

                //Se agrega un layout para mostrar subitems cuando aplique
                layout.Children.Add(layoutSubItems);

                //Se agregan los botones de guardar y cancelar
                StackLayout layoutBotones = new StackLayout();
                layoutBotones.Orientation = StackOrientation.Horizontal;
                btnGuardar = new Button();
                btnGuardar.Text = "Guardar";

                btnGuardar.Clicked += Btn_Clicked;
                if(_idTipoEvento == 38 || _idTipoEvento == 39 || _idTipoEvento == 42 || _idTipoEvento == 43)
                {
                    btnGuardar.IsEnabled = false;                    
                }
                layoutBotones.Children.Add(btnGuardar);

                //Se agrega un indicador de actividad            
                //layout.Children.Add(_guardarActivityIndicator);


                Button btn = new Button();
                btn.Text = "Cancelar";
                btn.Clicked += Btn_Clicked;
                layoutBotones.Children.Add(btn);


                //Botones de firma de remesa solo para el evento de llegada descargue
                if (_idTipoEvento == (int)TipoEventoLogisticoEnum.LlegadaDestino) {

                    //Se crea botón para visualizar documento asociado
                    btnVerEntrega = new Button();
                    btnVerEntrega.Text = "Ver Entrega";
                    btnVerEntrega.Clicked += VerEntrega_Clicked;
                    layoutBotones.Children.Add(btnVerEntrega);

                    //Se crea boton para firmar pdf
                    btnFirmarEntrega = new Button();
                    btnFirmarEntrega.Text = "Firmar Entrega";
                    btnFirmarEntrega.Clicked += FirmarEntrega_Clicked;
                    layoutBotones.Children.Add(btnFirmarEntrega);
                }
                

                layout.Children.Add(layoutBotones);

                //Se agregan botones de imagenes                    
                StackLayout layoutCargarImagenes = new StackLayout();
                layoutCargarImagenes.Orientation = StackOrientation.Horizontal;
                btnNuevaFoto = new Button();
                btnNuevaFoto.Text = "Tomar Foto";
                btnNuevaFoto.Clicked += CapturarFoto;
                layoutCargarImagenes.Children.Add(btnNuevaFoto);

                btnSeleccionarFoto = new Button();
                btnSeleccionarFoto.Text = "Seleccionar Foto";
                btnSeleccionarFoto.Clicked += SeleccionarFoto;
                layoutCargarImagenes.Children.Add(btnSeleccionarFoto);


                

                if (_idTipoEvento == 42 || _idTipoEvento == 43)
                {
                    btnNuevaFoto.IsEnabled = false;
                    btnSeleccionarFoto.IsEnabled = false;
                }

                layout.Children.Add(layoutCargarImagenes);
                layout.Children.Add(layoutImages);

                Content = scrollview;

                //if (_idTipoEvento != 38 && _idTipoEvento != 39)
                //{
                    
                //}                
                
            }
        }

        

        //Se crea evento de visualizacion de entregas
        private async void VerEntrega_Clicked(object sender, EventArgs e) {

           
            string valor = "";
            var layout = ((StackLayout)(((ScrollView)Content).Content));

            View view = layout.Children.SingleOrDefault(d => d.StyleId == "campo1");

            CheckBox checkBox = null;
            foreach (View filaGrid in ((Grid)view).Children)
            {
                checkBox = ((CheckBox)filaGrid);
                if (checkBox != null && checkBox.Checked)
                {
                    string[] arrayEventoEntrega = checkBox.DefaultText.Split('-');
                    
                    valor = arrayEventoEntrega[0] + "," + valor;
                }

                
            }

            //Validación de entrega seleccionada
            if (string.IsNullOrEmpty(valor))
            {
                DisplayAlert("Alerta", "Debe seleccionar una entrega", "Aceptar");
            }
            else {

                valor = valor.Substring(0, (valor.Length - 1));
                //Validacion que solo sea una entrega
                if (valor.Split(',').Count() > 1)
                {
                    DisplayAlert("Alerta", "No puede seleccionar más de una entrega", "Aceptar");
                }
                else {
                    //Descargar Documento
                    DocumentosBBL documentosBBL = new DocumentosBBL();
                    btnVerEntrega.IsEnabled = false;
                    IsBusy = true;
                    string newRuta = await documentosBBL.DescargaEntrega(valor);
                    IsBusy = false;
                    btnVerEntrega.IsEnabled = true;

                    //Abrir la vista
                    Navigation.PushAsync(new VerEntregaPDFCS(newRuta));
                }
            }
            

            

            

        }

        //Se crea evento de firma de entregas
        private async void FirmarEntrega_Clicked(object sender, EventArgs e)
        {
            string valor = "";
            var layout = ((StackLayout)(((ScrollView)Content).Content));

            View view = layout.Children.SingleOrDefault(d => d.StyleId == "campo1");

            CheckBox checkBox = null;
            foreach (View filaGrid in ((Grid)view).Children)
            {
                checkBox = ((CheckBox)filaGrid);
                if (checkBox != null && checkBox.Checked)
                {
                    string[] arrayEventoEntrega = checkBox.DefaultText.Split('-');

                    valor = arrayEventoEntrega[0] + "," + valor;
                }


            }

            //Validación de entrega seleccionada
            if (string.IsNullOrEmpty(valor))
            {
                DisplayAlert("Alerta", "Debe seleccionar una entrega", "Aceptar");
            }
            else
            {

                valor = valor.Substring(0, (valor.Length - 1));
                //Validacion que solo sea una entrega
                if (valor.Split(',').Count() > 1)
                {
                    DisplayAlert("Alerta", "No puede seleccionar más de una entrega", "Aceptar");
                }
                else
                {

                    //Validacion de que la entrega no este firmada
                    EntregaBLL entregaBLL = new EntregaBLL();
                    btnFirmarEntrega.IsEnabled = false;
                    IsBusy = true;
                    Entrega miEntrega = await entregaBLL.ObtenerEntrega(Convert.ToInt32(valor));
                    IsBusy = false;
                    btnFirmarEntrega.IsEnabled = true;

                    if (miEntrega.EstaFirmada)
                    {
                        DisplayAlert("Alerta", "La entrega ya fue firmada anteriormente", "Aceptar");
                    }
                    else {
                        //Abrir la vista
                        Navigation.PushAsync(new Logistica.FirmarEntrega(valor, _idTipoEvento, _nombreEvento));
                    }

                    
                }
            }

            
            

        }

        private async void Btn_Clicked(object sender, EventArgs e)
        {           
            
            string comando = ((Button)sender).Text.ToLower();
            if(comando == "cancelar")
            {
                var answer = await DisplayAlert("Atención", "Seguro desea cancelar la creación del evento. Se pederán todos los datos.", "Si", "No");
                if(answer)
                {
                    Navigation.PopAsync();
                    //Navigation.PushAsync(new Logistica.MenuEventosView());
                }
                
            }
            else if(comando == "consultar")
            {
                ((Button)sender).IsEnabled = false;
                var resultado = "";
                var layout = ((StackLayout)(((ScrollView)Content).Content));

                if(layout != null)
                {
                    foreach(View view in layout.Children)
                    {
                        if(view.StyleId == "campo1")
                        {
                            var _numeroEntrega = (Entry)view;
                            resultado = _numeroEntrega.Text;
                            break;
                        }                        
                    }
                }              


                if (resultado == null || resultado.IndexOf(".") != -1)
                {
                    if(_idTipoEvento == 42 || _idTipoEvento == 43)
                    {
                        DisplayAlert("Alerta", "Ingrese un número de viaje válido", "Aceptar");
                    }
                    else
                    {
                        DisplayAlert("Alerta", "Ingrese un número de entrega válido", "Aceptar");
                    }                  
                    
                    ((Button)sender).IsEnabled = true;
                }
                else
                {

                    EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();
                    try
                    {
                        if(_idTipoEvento == 38 || _idTipoEvento == 39)
                        {
                            
                            var remesasPorEntrega = new List<RemesasPorNumeroEntrega>();

                            remesasPorEntrega = await eventoBLL.SeleccionarRemesaPorNumeroEntrega(Int32.Parse(resultado));
                            _numeroManifiestoBodega = remesasPorEntrega[0].NumeroManifiesto;

                            if(remesasPorEntrega == null || remesasPorEntrega.Count == 0)
                            {
                                DisplayAlert("Alerta", "No se encuentran remesas con este número de entrega.", "Aceptar");
                                ((Button)sender).IsEnabled = true;
                                return;
                            }

                            //var eventosBodega = remesasPorEntrega;

                            //lvEventosBodega.ItemsSource = remesasPorEntrega;

                            //StackLayout layoutInfoEntrega = new StackLayout();
                            layoutInfoEntrega.Orientation = StackOrientation.Vertical;

                            //Nombre Cliente
                            StackLayout layoutNombreCliente = new StackLayout();
                            layoutNombreCliente.Orientation = StackOrientation.Horizontal;

                            Label txtNombreCliente = new Label();
                            txtNombreCliente.Text = "Cliente: ";
                            txtNombreCliente.Font = Font.SystemFontOfSize(20, FontAttributes.Bold);
                            layoutNombreCliente.Children.Add(txtNombreCliente);

                            Label txtNombreClienteValue = new Label();
                            txtNombreClienteValue.Text = remesasPorEntrega[0].NombreCliente;
                            txtNombreClienteValue.Font = Font.SystemFontOfSize(20, FontAttributes.None);
                            layoutNombreCliente.Children.Add(txtNombreClienteValue);

                            layoutInfoEntrega.Children.Add(layoutNombreCliente);

                            //Documento Cliente
                            StackLayout layoutDocCliente = new StackLayout();
                            layoutDocCliente.Orientation = StackOrientation.Horizontal;

                            Label txtDocCliente = new Label();
                            txtDocCliente.Text = "Pedido: ";
                            txtDocCliente.Font = Font.SystemFontOfSize(20, FontAttributes.Bold);
                            layoutDocCliente.Children.Add(txtDocCliente);

                            Label txtDocClienteValue = new Label();
                            txtDocClienteValue.Text = remesasPorEntrega[0].DocumentoCliente;
                            txtDocClienteValue.Font = Font.SystemFontOfSize(20, FontAttributes.None);
                            layoutDocCliente.Children.Add(txtDocClienteValue);

                            layoutInfoEntrega.Children.Add(layoutDocCliente);

                            //Placa Vehículo
                            StackLayout layoutPlaca = new StackLayout();
                            layoutPlaca.Orientation = StackOrientation.Horizontal;

                            Label txtPlaca = new Label();
                            txtPlaca.Text = "Placa: ";
                            txtPlaca.Font = Font.SystemFontOfSize(20, FontAttributes.Bold);
                            layoutPlaca.Children.Add(txtPlaca);

                            Label txtPlacaValue = new Label();
                            txtPlacaValue.Text = remesasPorEntrega[0].PlacaVehiculo;
                            txtPlacaValue.Font = Font.SystemFontOfSize(20, FontAttributes.None);
                            layoutPlaca.Children.Add(txtPlacaValue);

                            layoutInfoEntrega.Children.Add(layoutPlaca);

                            //Se agregan los layout al layout general
                            layout.Children.Add(layoutInfoEntrega);
                        }
                        else
                        {
                            List<RemesasPorNumeroTransporte> remesasPorTransporte = new List<RemesasPorNumeroTransporte>();

                            remesasPorTransporte = await eventoBLL.SeleccionarRemesasPorNumeroViaje(Int32.Parse(resultado));
                            _numeroManifiestoBodega = Int32.Parse(resultado);

                            if (remesasPorTransporte == null || remesasPorTransporte.Count == 0)
                            {
                                DisplayAlert("Alerta", "No se encuentran datos con este número de viaje.", "Aceptar");
                                ((Button)sender).IsEnabled = true;
                                return; 
                            }

                            layoutInfoEntrega.Orientation = StackOrientation.Vertical;
                            //Android.Widget.RadioGroup radioGroup = new Android.Widget.RadioGroup();

                            if(remesasPorTransporte != null && remesasPorTransporte.Count > 0)
                            {

                                StackLayout layoutRadioButton = new StackLayout();
                                layoutRadioButton.Orientation = StackOrientation.Vertical;

                                View input = new Entry();
                                input = new Picker();

                                radioGroup = new BindableRadioGroup();
                                //BindableRadioGroup rb = new BindableRadioGroup();
                                datos = new List<string>();
                                datos.Add("Fotos Generales");
                                foreach (RemesasPorNumeroTransporte rt in remesasPorTransporte)
                                {
                                    datos.Add(rt.NumeroRemesa.ToString() + " - " + rt.NombreCliente);
                                    _remesasViaje = _remesasViaje + "_" + rt.NumeroRemesa;
                                }
                                                                
                                radioGroup.ItemsSource = datos;
                                radioGroup.SelectedIndex = 0;
                                input = radioGroup;

                                layoutRadioButton.Children.Add(input);

                                layoutInfoEntrega.Children.Add(layoutRadioButton);

                                btnNuevaFoto.IsEnabled = true;
                                btnSeleccionarFoto.IsEnabled = true;
                            }                           

                        }
                        

                        btnGuardar.IsEnabled = true;

                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
            else if(comando == "guardar")
            {
                //var selIndex = radioGroup.SelectedIndex;
                btnGuardar.IsEnabled = false;
                //************** Se obtienen la latitud, longitud y descripción de ubicacion del gps del dipositivo móvil *****************************
                double lat = 0;
                double lon = 0;
                string _address = string.Empty;
                //Context mContex = this;
                GPSInfo location = null;

                if(_idTipoEvento != 38 && _idTipoEvento != 39 && _idTipoEvento != 42 && _idTipoEvento != 43 && _idTipoEvento != 47)
                {
                    while (location == null)
                    {                        
                        location = await getLocation();
                    }
                }
                else
                {
                    location = new GPSInfo();
                    location.GPSEnabled = false;
                }
                
                if (location.GPSEnabled || _idTipoEvento == 38 || _idTipoEvento == 39 || _idTipoEvento == 42 || _idTipoEvento == 43 || _idTipoEvento == 47)
                {

                    IList<Android.Locations.Address> addressList = null;
                    Android.Locations.Address address = null;

                    //The Geocoder class retrieves a list of address from Google over the internet  
                    Geocoder geocoder = new Geocoder(Android.App.Application.Context);

                    try
                    {
                        if (location.Latitude != 0)
                        {
                            addressList = geocoder.GetFromLocation(location.Latitude, location.Longitude, 1);
                            address = addressList.FirstOrDefault();

                            _address = address.SubThoroughfare + " " + address.Thoroughfare +
                                        ", " + address.Locality + ", " + address.AdminArea + ", " + address.CountryCode;
                            //_address = address.ToString();


                        }

                        lat = location.Latitude;
                        lon = location.Longitude;

                    }
                    catch (Exception ex)
                    {

                    }

                    

                    if (address != null)
                    {
                        StringBuilder deviceAddress = new StringBuilder();
                        deviceAddress.AppendLine(address.GetAddressLine(0));

                        _address = deviceAddress.ToString();
                    }                   

                    //******************* Fin eventos GPS *************************

                    EventoLogistico evento = new EventoLogistico();

                    var _fechaFinal = DateTime.Now.ToString("dd.MM.yyyy");
                    var _horaFinal = DateTime.Now.ToString("HH:mm:ss");

                    evento.campo4 = _fechaFinal;
                    evento.campo5 = _horaFinal.ToString();

                    evento.Latitud = (float)lat;
                    evento.Longitud = (float)lon;
                    evento.DescripcionPosicion = _address;

                    //evento.Placa = "TMU048";

                    evento.IdTipoEvento = _idTipoEvento;
                    evento.NombreTipoEvento = _nombreEvento;
                    evento.FechaEvento = DateTime.Now;
                    evento.NumeroDocumentoConductor = ParametrosSistema.NumeroIdentificacionUsuarioActual;
                    
                    TransporteBLL transporteBLL = new TransporteBLL();
                    HistorialActivacionManifiesto historialActivacion = transporteBLL.SeleccionarHistorialManifiestoActivoPorConductor(ParametrosSistema.NumeroIdentificacionUsuarioActual);
                    if (historialActivacion != null)
                    {
                        evento.NumeroManifiesto = historialActivacion.NumeroManifiesto;
                        evento.Placa = historialActivacion.Placa;
                    }

                    if (_idTipoEvento == 38 || _idTipoEvento == 39 || _idTipoEvento == 42 || _idTipoEvento == 43 || _idTipoEvento == 47)
                    {
                        evento.campo6 = ParametrosSistema.UsuarioActual;
                        evento.NumeroManifiesto = _numeroManifiestoBodega;
                    }

                    CampoEventoLogistico campoActual;

                    //_guardarActivityIndicator.IsRunning = true;
                    IsBusy = true;
                    
                    var layout = ((StackLayout)(((ScrollView)Content).Content));

                    if (layout != null)
                    {
                        foreach (View view in layout.Children)
                        {
                            if (!String.IsNullOrEmpty(view.StyleId))
                            {
                                string valor = string.Empty;
                                Type type = view.GetType();
                                campoActual = _camposTipoEvento.Where(c => c.NombreCampoEventoLogistico == view.StyleId).FirstOrDefault();
                                if (campoActual != null)
                                {
                                    switch (type.Name)
                                    {
                                        case "Entry":
                                            valor = ((Entry)view).Text;
                                            break;
                                        case "DatePicker":
                                            valor = DateTime.Now.ToString("yyyy MM dd");//((DatePicker)view).Date.ToString("yyyy MM dd");
                                            break;
                                        case "TimePicker":
                                            valor = DateTime.Now.ToString("HH:mm:ss"); //((TimePicker)view).Time.ToString();
                                            break;
                                        case "Switch":
                                            valor = ((Switch)view).IsToggled.ToString();
                                            break;
                                        case "Picker":
                                            if (((Picker)view).SelectedIndex >= 0)
                                            {
                                                valor = ((Picker)view).Items[((Picker)view).SelectedIndex].ToString();
                                            }
                                            break;
                                        case "Grid":
                                            CheckBox checkBox = null;
                                            foreach (View filaGrid in ((Grid)view).Children)
                                            {
                                                checkBox = ((CheckBox)filaGrid);
                                                if (checkBox != null && checkBox.Checked)
                                                {
                                                    valor = checkBox.DefaultText + "," + valor;
                                                }
                                            }
                                            break;
                                        case "BindableRadioGroup":
                                            CustomRadioButton rbtn = null;
                                            foreach (View opcion in ((BindableRadioGroup)view).Children)
                                            {
                                                rbtn = ((CustomRadioButton)opcion);
                                                if (rbtn != null && rbtn.Checked)
                                                {
                                                    valor = rbtn.Text;
                                                    string[] datosViaje = valor.Split('-');
                                                    valor = datosViaje[0];
                                                }
                                            }
                                            break;
                                        case "time":

                                            break;
                                        default:
                                            throw new Exception("No se reconoce el tipo de control " + type.Name);
                                    }

                                    if (campoActual.Obligatorio && String.IsNullOrEmpty(valor))
                                    {
                                        await DisplayAlert("Error", "Debe ingesar un dato en el campo " + campoActual.TituloCampoEventoLogistico, "Aceptar");
                                        //_guardarActivityIndicator.IsRunning = false;

                                        IsBusy = false;
                                        btnGuardar.IsEnabled = true;
                                        return;
                                    }

                                    Util.EstablecerValorDinamicamente(evento, view.StyleId, valor);
                                }
                            }
                        }

                        //if (_idTipoEvento == 38 || _idTipoEvento == 39 || _idTipoEvento == 42 || _idTipoEvento == 43)
                        //{
                            
                        //}
                        //20191019 - Se comenta estaba en modificacion de bodega pero esta causando problemas al guardar solicitud de tanqueo e inventario de trailer
                        //evento.campo2 = _fechaInicio;
                        //evento.campo3 = _horaInicio;

                        if (adjuntos != null && adjuntos.Count > 0)
                        {
                            evento.Adjuntos = new List<AdjuntoEventoLogistico>();

                            foreach (AdjuntoEventoLogistico adjunto in adjuntos)
                            {
                                adjunto.NumeroManifiesto = evento.NumeroManifiesto;
                                adjunto.NombreArchivo = evento.NumeroManifiesto.ToString() + "-" + ParametrosSistema.UsuarioActual + DateTime.Now.ToString();
                                evento.Adjuntos.Add(adjunto);
                            }
                        }

                        //Se guarda el evento
                        EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();
                        RespuestaProcesoEventoLogistico respuesta = new RespuestaProcesoEventoLogistico(); ;
                        if (!ParametrosSistema.EventoConCampoEntregas.Contains(evento.IdTipoEvento))
                        {
                            respuesta = await eventoBLL.GuardarEventoLogistico(evento);
                        }
                        else
                        {
                            string[] arrayEntregas = evento.campo1.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (arrayEntregas != null)
                            {
                                foreach (string numeroEntrega in arrayEntregas)
                                {
                                    string[] arrayEventoEntrega = numeroEntrega.Split('-');

                                    if (arrayEntregas != null) {
                                        evento.campo1 = arrayEventoEntrega[0].Trim();
                                        respuesta = await eventoBLL.GuardarEventoLogistico(evento);
                                    }
                                    
                                }
                            }

                        }
                        if (respuesta != null)
                        {
                            //var respuestaActivity = new Intent("RespuestaCrearEventoView");
                            //respuestaActivity.PutExtra("idEventoLogistico", _idTipoEvento);
                            respuesta.IdTipoEvento = _idTipoEvento;

                            var respuestaView = new RespuestaCrearEventoView();
                            respuestaView.BindingContext = respuesta;

                            await Navigation.PushAsync(respuestaView);

                        }

                        //_guardarActivityIndicator.IsRunning = false;
                        IsBusy = false;
                        btnGuardar.IsEnabled = true;
                    }
                }
                btnGuardar.IsEnabled = true;
            }
        }

        private async void CapturarFoto(object sender, EventArgs e)
        {
            try
            {

                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DisplayAlert("Sin Camara", "La camara no esta disponible.", "Aceptar");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "App TDM",
                    Name = "ImagenEvento.jpg",
                    SaveToAlbum = true                        
                                    
                });

                if (file == null)
                    return;
                                
                Image image = new Image();
                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
                
                FileStream fs = new FileStream(file.Path, FileMode.Open, FileAccess.Read);
                byte[] ImageData = new byte[fs.Length];
                
                fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

                var resizer = DependencyService.Get<IImageResizer>();
                byte[] resizedImage = resizer.ResizeImage(ImageData, 500, 500);
                image.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));

                string _base64String = Convert.ToBase64String(resizedImage);

                AdjuntoEventoLogistico adjunto = new AdjuntoEventoLogistico();
                adjunto.Extension = System.IO.Path.GetExtension(file.Path);
                adjunto.Extension = adjunto.Extension.Remove(0, 1);
                adjunto.Base64String = "data:image/jpeg;base64," + _base64String;
                
                if (_idTipoEvento == 42 || _idTipoEvento == 43)
                {
                    var remesa = "";
                    var selIndex = radioGroup.SelectedIndex;
                    if(selIndex == 0)
                    {
                        remesa = _remesasViaje;
                    }
                    else
                    {
                        remesa = datos[selIndex];
                    }                    
                    adjunto.NumeroRemesa = remesa;
                }

                adjuntos.Add(adjunto);

                var icon = new Image
                {
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 50,
                    WidthRequest = 50,
                    Source=image.Source
                };
                
                layoutImages.Children.Add(icon);
            }
            catch (Exception ex)
            {
                await DisplayAlert("File Location", ex.Message.ToString(), "OK");
            }
        }

        private async void SeleccionarFoto(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            Image image = new Image();
            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();                
                return stream;
            });

            FileStream fs = new FileStream(file.Path, FileMode.Open, FileAccess.Read);
            byte[] ImageData = new byte[fs.Length];
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

            var resizer = DependencyService.Get<IImageResizer>();
            byte[] resizedImage = resizer.ResizeImage(ImageData, 600, 600);
            image.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));

            string _base64String = Convert.ToBase64String(resizedImage);

            AdjuntoEventoLogistico adjunto = new AdjuntoEventoLogistico();
            adjunto.Extension = System.IO.Path.GetExtension(file.Path);
            adjunto.Extension = adjunto.Extension.Remove(0, 1);
            adjunto.Base64String = "data:image/jpeg;base64," + _base64String;

            if (_idTipoEvento == 42 || _idTipoEvento == 43)
            {
                var remesa = "";
                var selIndex = radioGroup.SelectedIndex;
                if (selIndex == 0)
                {
                    remesa = _remesasViaje;
                }
                else
                {
                    remesa = datos[selIndex];
                }
                adjunto.NumeroRemesa = remesa;
            }

            adjuntos.Add(adjunto);

            var icon = new Image
            {
                Aspect = Aspect.AspectFit,
                HeightRequest = 50,
                WidthRequest = 50,
                Source = image.Source
            };

            layoutImages.Children.Add(icon);
        }

       public void InitializeLocationManager()
        {
            string context = Context.LocationService;
            _locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(context);
            //_locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
        }

        //public Location getLocation2()
        //{
        //    Location location = null;

        //    try
        //    {

        //        // The minimum distance to change Updates in meters
        //        long MIN_DISTANCE_CHANGE_FOR_UPDATES = 10; // 10 meters

        //        // The minimum time between updates in milliseconds
        //        long MIN_TIME_BW_UPDATES = 1000 * 60 * 1; // 1 minute

        //        double Longitude = 0;
        //        double Latitudes = 0;
        //        LocationManager locationManager = null;
        //        string context = Context.LocationService;

        //        locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(context);

        //        // getting GPS status
        //        bool isGPSEnabled = locationManager
        //                 .IsProviderEnabled(LocationManager.GpsProvider);

        //        // getting network status
        //        bool isNetworkEnabled = locationManager
        //                .IsProviderEnabled(LocationManager.NetworkProvider);

        //        if (!isGPSEnabled && !isNetworkEnabled)
        //        {
        //            // no network provider is enabled
        //            DisplayAlert("Alert", "Debe activar gps de su dispositivo.", "Aceptar");
        //        }
        //        else
        //        {
        //            Intent notificationIntent = new Intent(this, typeof(RecordingService));
        //            pendingIntent = PendingIntent.GetActivity(this, 0,
        //                notificationIntent, 0);

        //            // First get location from Network Provider
        //            if (isNetworkEnabled)
        //            {
        //                PendingIntent p = new PendingIntent() { }

        //                 locationManager.RequestLocationUpdates(
        //                         LocationManager.NetworkProvider,
        //                         MIN_DISTANCE_CHANGE_FOR_UPDATES,
        //                         MIN_TIME_BW_UPDATES, this);
        //                //Log.d("Network", "Network");

        //                if (locationManager != null)
        //                {
        //                    location = locationManager
        //                            .GetLastKnownLocation(LocationManager.NetworkProvider);

        //                    if (location != null)
        //                    {
        //                        Latitudes = location.Latitude;
        //                        Longitude = location.Longitude;
        //                    }
        //                }
        //            }
        //            // if GPS Enabled get lat/long using GPS Services
        //            if (isGPSEnabled)
        //            {
        //                if (location == null)
        //                {
        //                    locationManager.RequestLocationUpdates(
        //                            LocationManager.GpsProvider,
        //                           MIN_DISTANCE_CHANGE_FOR_UPDATES,
        //                        MIN_TIME_BW_UPDATES, this);
        //                    //Log.d("GPS Enabled", "GPS Enabled");
        //                    if (locationManager != null)
        //                    {
        //                        location = locationManager
        //                                .GetLastKnownLocation(LocationManager.GpsProvider);
        //                        if (location != null)
        //                        {
        //                            Latitudes = location.Latitude;
        //                            Longitude = location.Longitude;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        DisplayAlert("Alert", e.Message + " " + e.InnerException, "Aceptar");
        //    }

        //    return location;
        //}

        //public void OnLocationChanged(Location location)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OnProviderDisabled(string provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OnProviderEnabled(string provider)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<GPSInfo> getLocation()
        //{

        //    GPSInfo info = new GPSInfo();
        //    info.GPSEnabled = true;
        //    Position position = null;
        //    try
        //    {

        //        info.GPSEnabled = _locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        //        if (!info.GPSEnabled)
        //        {
        //            DisplayAlert("Alert", "Debe activar gps de su dispositivo.", "Aceptar");
        //            return info;
        //        }

        //        var locator = CrossGeolocator.Current;
        //        locator.DesiredAccuracy = 100;

        //        position = await locator.GetLastKnownLocationAsync();

        //        if (position != null)
        //        {
        //            //got a cahched position, so let's use it.
        //            info.Latitude = position.Latitude;
        //            info.Longitude = position.Longitude;
        //            return info;
        //        }

        //        if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
        //        {
        //            //not available or enabled
        //            info.Longitude = 0;
        //            info.Longitude = 0;
        //            info.address = "Servicio GPS no disponible";
        //            return info;
        //        }


        //        position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayAlert("Alert", ex.Message, "Aceptar");
        //    }

        //    if (position != null)
        //    {
        //        info.Latitude = position.Latitude;
        //        info.Longitude = position.Longitude;
        //    }
        //    else {
        //        info.Longitude = 0;
        //        info.Longitude = 0;
        //        info.address = "No obtuvo posición GPS";
        //    }



        //    return info;
        //}

        //20180320 - anterior funcion getgps
        public async Task<GPSInfo> getLocation(bool LastKnownLocation = false)
        {


            GPSInfo info = new GPSInfo();
            try
            {

                //string context = Context.LocationService;

                //_locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(context);

                info.GPSEnabled = _locationManager.IsProviderEnabled(LocationManager.GpsProvider);

                if (!info.GPSEnabled)
                {
                    DisplayAlert("Alert", "Debe activar gps de su dispositivo.", "Aceptar");
                }
                else
                {
                    var locator = CrossGeolocator.Current;

                    Position position = null;

                    if (LastKnownLocation)
                    {
                        position = await locator.GetLastKnownLocationAsync();
                    }
                    else
                    {
                        position = await locator.GetPositionAsync(TimeSpan.FromSeconds(40));
                    }

                    if (!LastKnownLocation && (position == null || position.Latitude == 0))
                    {
                        info = await getLocation(true);
                    }
                    else
                    {

                        if (position == null)
                        {
                            info.Latitude = 0;
                            info.Longitude = 0;
                        }
                        else
                        {
                            info.Latitude = position.Latitude;
                            info.Longitude = position.Longitude;
                        }

                    }


                    //List<string> providers = _locationManager.GetProviders(true).ToList();
                    ////Location bestLocation = null;
                    ////string provider = LocationManager.GpsProvider;


                    ////_locationManager.RequestLocationUpdates(
                    ////             LocationManager.NetworkProvider,
                    ////             MIN_DISTANCE_CHANGE_FOR_UPDATES,
                    ////             MIN_TIME_BW_UPDATES, this);

                    //foreach (String provider in providers) {

                    //    Location l = _locationManager.GetLastKnownLocation(provider);

                    //    if (l != null)
                    //    {
                    //        if (location == null || l.Accuracy < location.Accuracy)
                    //        {
                    //            // Found best last known location: %s", l);
                    //            location = l;

                    //        }
                    //    }
                    //}

                }

            }
            catch (Exception e)
            {

            }

            return info;


        }



        public void OnLocationChanged(Location location)
        {
            string example = "";
            example = "tis";
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
