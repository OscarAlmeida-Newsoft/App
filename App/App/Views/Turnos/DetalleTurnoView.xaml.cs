using App.Entities.Turnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XLabs.Forms.Controls;
using System.Collections.ObjectModel;
using App.BLL.Turnos;
using App.Entities.Operaciones;
using App.Common;
using System.Net.Http;
using App.Entities.IT;
using App.BLL.Operaciones;

namespace App.Views.Turnos
{
    public partial class DetalleTurnoView : ContentPage
    {
        private Enturnamiento _turnoSeleccionado = null;
        List<Agencia> agencias = new List<Agencia>();

        public DetalleTurnoView(Enturnamiento turnoSeleccionado)
        {
            _turnoSeleccionado = turnoSeleccionado;
            InitializeComponent();
            if (turnoSeleccionado != null)
                this.BindingContext = turnoSeleccionado;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;
            EnturnamientoBLL enturnamientoBLL = new EnturnamientoBLL();
            AgenciaBLL agenciaBLL = new AgenciaBLL();
            agencias = await enturnamientoBLL.ObtenerAgenciasEnturnamiento();

            var pickerPuestos = new ExtendedPicker() { DisplayProperty = "NombreAgencia" };
            pickerPuestos.ItemsSource = agencias.Where(d => d.EsAgencia).ToList();
            puestoExpedicion.Children.Add(pickerPuestos);

            //List<string> destinos = new List<string> { "NACIONAL", "BARRANQUILLA", "BOGOTÁ", "BUENAVENTURA", "CALI", "CARTAGENA", "MEDELLÍN" };
            var pickerDestinos = new ExtendedPicker() { DisplayProperty = "NombreAgencia" };
            pickerDestinos.ItemsSource = agencias.Where(d => d.EsDestino).ToList();
            destino.Children.Add(pickerDestinos);

            List<string> tipoTRailer = new List<string>() { "Carroceria", "Plancha", "Ambos" };
            rbTipoTRailer.ItemsSource = tipoTRailer;
            Util util = new Util();

            if (!util.UsuarioTienePermiso("enturnarplacasilimitadas"))
            {
                txtCedulaConductor.IsVisible = false;

                var pickerConductores = new ExtendedPicker() { DisplayProperty = "Nombre" };
                List<Proveedor> listaConductores = await enturnamientoBLL.ObtenerConductoresTurnosPorUsuario();
                pickerConductores.ItemsSource = listaConductores;
                pickerConductores.SelectedIndexChanged += (sender, args) =>
                {
                    string conductor = pickerConductores.Items[pickerConductores.SelectedIndex].ToString();
                    Proveedor conductorSeleccionado = (from p in listaConductores
                                                       where p.Nombre == conductor
                                                       select p).FirstOrDefault();
                    if (conductorSeleccionado != null)
                    {
                        _turnoSeleccionado.NombreConductor = conductorSeleccionado.Nombre;
                        _turnoSeleccionado.NumeroDocConductor = conductorSeleccionado.NumIdentificacionFiscal;
                    }
                };
                if (_turnoSeleccionado != null)
                {
                    //Se está modificando un turno

                    if (!String.IsNullOrEmpty(_turnoSeleccionado.NumeroDocConductor))
                    {
                        var conductorActual = listaConductores.FirstOrDefault(c=>c.NumIdentificacionFiscal.TrimStart('0') == _turnoSeleccionado.NumeroDocConductor.TrimStart('0'));
                        if(conductorActual!=null)
                        {
                            pickerConductores.SelectedIndex = pickerConductores.Items.IndexOf(conductorActual.Nombre);
                        }
                        
                    }

                }
                layoutConductor.Children.Add(pickerConductores);
            }
            else
            {
                txtCedulaConductor.IsVisible = false;


                List<Proveedor> proveedores = new List<Proveedor>();
                List<string> nombreConductores = new List<string>();
                AutoCompleteView autocompleteConductores = new AutoCompleteView();
                autocompleteConductores.ShowSearchButton = false;

                autocompleteConductores.TextChanged += async (sender, args) =>
                {
                    if (String.IsNullOrEmpty(autocompleteConductores.Text))
                    {
                        autocompleteConductores.Suggestions = new List<string>();
                    }
                    else if (autocompleteConductores.Text.Length >= 4)
                    {
                        IsBusy = true;
                        proveedores = await enturnamientoBLL.ObtenerConductoresPorNombre(autocompleteConductores.Text);
                        IsBusy = false;
                        nombreConductores = (from p in proveedores
                                             select p.Nombre).DefaultIfEmpty().ToList();
                        autocompleteConductores.Suggestions = nombreConductores;
                    }

                };

                autocompleteConductores.SelectedItemChanged += (sender, args) =>
                {
                    Proveedor conductorSeleccionado = (from p in proveedores
                                                       where p.Nombre == autocompleteConductores.Text
                                                       select p).FirstOrDefault();
                    if (conductorSeleccionado != null)
                    {
                        txtCedulaConductor.Text = conductorSeleccionado.NumIdentificacionFiscal;
                        _turnoSeleccionado.NombreConductor = conductorSeleccionado.Nombre;
                        _turnoSeleccionado.NumeroDocConductor = conductorSeleccionado.NumIdentificacionFiscal;
                    }
                };
                layoutConductor.Children.Add(autocompleteConductores);
            }
            if (_turnoSeleccionado != null)
            {
                //Si están modificando un turno                
                var codigoPuestoExpedicion = _turnoSeleccionado.PuestoExpedicion;
                var PuestoExpedicion = (from a in agencias
                                        where a.CodigoAgencia == _turnoSeleccionado.PuestoExpedicion
                                        select a).FirstOrDefault();

                if (PuestoExpedicion != null)
                {
                    pickerPuestos.SelectedIndex = pickerPuestos.Items.IndexOf(PuestoExpedicion.NombreAgencia);
                    pickerPuestos.IsEnabled = false;
                }

                txtPlacaCabezote.Text = _turnoSeleccionado.PlacaCabezote;
                txtPlacaTrailer.Text = _turnoSeleccionado.PlacaTrailer;
                lblTipoTrailer.Text = _turnoSeleccionado.TipoTrailer;
                txtCedulaConductor.Text = _turnoSeleccionado.NumeroDocConductor;

                if (_turnoSeleccionado.PlacaCabezote.Length > 0)
                    txtPlacaCabezote.IsEnabled = false;

                if (String.IsNullOrEmpty(_turnoSeleccionado.Destino))
                {
                    pickerDestinos.SelectedIndex = pickerDestinos.Items.IndexOf("Nacional");
                }
                else
                {
                    pickerDestinos.SelectedIndex = pickerDestinos.Items.IndexOf(_turnoSeleccionado.Destino);
                }
                    
                if (_turnoSeleccionado.CodigoTipoTrailer == "13")
                {
                    rbTipoTRailer.IsVisible = true;
                }
            }
            else
            {
                //Si se está creando un turno
                _turnoSeleccionado = new Enturnamiento();
                

                cbDisponible.IsVisible = false;

                if (!util.UsuarioTienePermiso("enturnarplacasilimitadas"))
                {
                    txtPlacaCabezote.IsVisible = false;
                    


                    List<Vehiculo> vehiculos = new List<Vehiculo>();
                    vehiculos = await enturnamientoBLL.ObtenerCabezotesTurnosPorUsuarioActual();

                    var pickerPlaca = new ExtendedPicker() { DisplayProperty = "Placa" };
                    pickerPlaca.ItemsSource = vehiculos;
                    placaCabezote.Children.Add(pickerPlaca);
                    pickerPlaca.SelectedIndexChanged += (sender, args) =>
                    {
                        string placa = pickerPlaca.Items[pickerPlaca.SelectedIndex].ToString();
                        if (!string.IsNullOrEmpty(placa))
                            _turnoSeleccionado.PlacaCabezote = placa;
                    };

                    
                    
                }
                
            }
           
            //Se agregan los botones de guardar y cancelar          
            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.Clicked += Btn_Clicked;
            layoutBotones.Children.Add(btnGuardar);

            Button btn = new Button();
            btn.Text = "Cancelar";
            btn.Clicked += Btn_Clicked;
            layoutBotones.Children.Add(btn);
                        
            pickerPuestos.SelectedIndexChanged += (sender, args) =>
            {
                string puesto = pickerPuestos.Items[pickerPuestos.SelectedIndex].ToString();
                Agencia puestoSeleccionado = (from a in agencias
                                              where a.NombreAgencia == puesto
                                              select a).FirstOrDefault();
                if (puestoSeleccionado != null)
                    _turnoSeleccionado.PuestoExpedicion = puestoSeleccionado.CodigoAgencia;
            };
            pickerDestinos.SelectedIndexChanged += (sender, args) =>
            {
                string destino = pickerDestinos.Items[pickerDestinos.SelectedIndex].ToString();
                if (destino != "NACIONAL")
                {                   
                    destino = util.RemoveAccentsWithRegEx(destino);
                    _turnoSeleccionado.Destino = destino.ToUpper();
                }
                else
                    _turnoSeleccionado.Destino = "";
            };
            IsBusy = false;
        }
        private async void Btn_Clicked(object sender, EventArgs e)
        {
            string comando = ((Button)sender).Text.ToLower();
            if (comando == "cancelar")
            {
                var answer = await DisplayAlert("Atención", "Seguro desea cancelar la modificación del truno. Se pederán todos los datos.", "Si", "No");
                if (answer)
                {
                    Navigation.PopAsync();
                }

            }
            else if (comando == "guardar")
            {
                IsBusy = true;
                ((Button)sender).IsEnabled = false;

                if (string.IsNullOrEmpty(_turnoSeleccionado.PlacaCabezote))
                {
                    _turnoSeleccionado.PlacaCabezote = txtPlacaCabezote.Text;
                }

                if(!String.IsNullOrEmpty(txtPlacaTrailer.Text))
                {
                    _turnoSeleccionado.PlacaTrailer = txtPlacaTrailer.Text.ToUpper();
                }               
                
                _turnoSeleccionado.Usuario = ParametrosSistema.UsuarioActual;              
                  
                EnturnamientoBLL enturnamientoBLL = new EnturnamientoBLL();

                bool trailerValidado = false;
                if (String.IsNullOrEmpty(_turnoSeleccionado.PuestoExpedicion))
                {
                    await DisplayAlert("Atención", "Debe seleccionar un origen.","Aceptar");
                    return;
                }
                if (String.IsNullOrEmpty(_turnoSeleccionado.PlacaCabezote))
                {
                    await DisplayAlert("Atención", "Debe seleccionar una placa de cabezote.", "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(_turnoSeleccionado.NumeroDocConductor))
                {
                    await DisplayAlert("Atención", "Debe seleccionar un conductor.", "Aceptar");
                    return;
                }
                if (!string.IsNullOrEmpty(_turnoSeleccionado.PlacaTrailer))
                {
                    if (!trailerValidado)
                    {
                        Vehiculo veh = await enturnamientoBLL.ObtenerVehiculo(_turnoSeleccionado.PlacaTrailer);
                        if (veh == null)
                        {
                            await DisplayAlert("Atención", "No existe la placa " + _turnoSeleccionado.PlacaTrailer, "Aceptar");
                            return;
                        }
                        else
                        {
                            foreach (CaracteristicaVehiculo caracteristica in veh.Caracteristicas)
                            {
                                if (caracteristica.Nombre == "GRUPO_TRAILER")
                                {
                                    _turnoSeleccionado.CodigoTipoTrailer = caracteristica.Valor;
                                }
                                if (_turnoSeleccionado.CodigoTipoTrailer == "13")
                                {
                                    _turnoSeleccionado.TipoTrailer = "Carrocería";
                                }
                                else
                                {
                                    _turnoSeleccionado.TipoTrailer = "--";
                                }
                            }
                        }
                        trailerValidado = true;
                    }
                }
                _turnoSeleccionado.PlacaCabezote = _turnoSeleccionado.PlacaCabezote.ToUpper();
                if (_turnoSeleccionado.IdTurno > 0)
                {
                    //Se esta modificando un turno existente
                    if (_turnoSeleccionado.Estado != "DP" && cbDisponible.Checked)
                    {
                        var answer = await DisplayAlert("Atención", "Si se pone disponible, no podrá modificar ninguno de los datos registrados. Desea continuar?", "Si", "No");
                        if (!answer)
                        {
                            Navigation.PopAsync();
                        }
                        _turnoSeleccionado.Estado = "DP";
                    }

                    if (_turnoSeleccionado.CodigoTipoTrailer == "13")
                    {
                        var tipotrailer = rbTipoTRailer.Items[rbTipoTRailer.SelectedIndex].Text;
                        if (tipotrailer == "Carroceria")
                        {
                            _turnoSeleccionado.TipoTrailer1 = "13";
                            _turnoSeleccionado.ClaseTrailer = "C";
                            _turnoSeleccionado.TipoTrailer2 = "";
                        }
                        else if (tipotrailer == "Plancha")
                        {
                            _turnoSeleccionado.TipoTrailer1 = "12";
                            _turnoSeleccionado.ClaseTrailer = "P";
                            _turnoSeleccionado.TipoTrailer2 = "";
                        }
                        else if (tipotrailer == "Ambos")
                        {
                            _turnoSeleccionado.TipoTrailer1 = "12";
                            _turnoSeleccionado.ClaseTrailer = "13";
                            _turnoSeleccionado.TipoTrailer2 = "A";
                        }
                        else
                        {
                            _turnoSeleccionado.TipoTrailer1 = "";
                            _turnoSeleccionado.ClaseTrailer = "";
                            _turnoSeleccionado.TipoTrailer2 = "";
                        }                       
                    }
                    List<RespuestaServicio> respuestas = await enturnamientoBLL.GuardarTurnos(new List<Enturnamiento>() { _turnoSeleccionado });
                    if (respuestas != null && respuestas.Count > 0)
                    {
                        if (respuestas[0].Exito)
                        {
                            Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Atención", respuestas[0].Mensaje, "Aceptar");
                            return;
                        }

                    }
                }
                else
                {
                    //se esta creando un turno
                    List<Enturnamiento> turnos = await enturnamientoBLL.ObtenerTurnosPorPlaca(_turnoSeleccionado.PlacaCabezote);
                    if (turnos != null && turnos.Count > 0)
                    {
                        await DisplayAlert("Atención", "La placa " + _turnoSeleccionado.PlacaCabezote + " se encuentra registrada en " + turnos[0].PuestoExpedicion + ". Debe eliminar el registro de la placa en la pantalla principal para poder crear un nuevo registro.", "Aceptar");
                    }
                    else {
                        _turnoSeleccionado.Estado = "TR";
                        List<RespuestaServicio> respuestas = await enturnamientoBLL.GuardarTurnos(new List<Enturnamiento>() { _turnoSeleccionado });
                        if (respuestas != null && respuestas.Count>0)
                        {
                            if (respuestas[0].Exito)
                            {
                                Navigation.PopAsync();
                            }
                            else
                            {
                                await DisplayAlert("Atención", respuestas[0].Mensaje , "Aceptar");
                                return;
                            }

                        }
                    }
                }
                IsBusy = false;
                ((Button)sender).IsEnabled = true;
            }
        }
    }
}
