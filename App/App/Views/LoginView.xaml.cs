using App.BLL.IT;
using App.BLL.Seguridad;
using App.Common;
using App.Entities.IT;
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
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            lblVersion.Text = "Versión "+ ParametrosSistema.AppVersion;

        }

        public async void btnIngresarClicked(object sender, EventArgs e)
        {
            btnLogin.IsEnabled = false;
            IsBusy = true;

            SeguridadBLL bll = new SeguridadBLL();
            //Se borran las credeciales almacenadas
            bll.EliminarCredencialesUsuario();

            if (await ParametrosSistema.isOnline)
            {
                TokenSeguridad token = null;

                try
                {
                    lblEstado.Text = "Autenticando usuario.";
                    token = await bll.Autenticar(txtUsuario.Text, txtPassword.Text);
                    if (token != null)
                    {
                        
                        lblEstado.Text = "Autenticación correcta.";
                        Com.OneSignal.OneSignal.IdsAvailable idsPrinterDelegate = async delegate (string playerID, string pushToken)
                        {
                            try
                            {
                                NotificacionBLL notificacionBLL = new NotificacionBLL();
                                CodigoNotificacionAplicacionMovil not = new CodigoNotificacionAplicacionMovil();
                                not.OneSignalId = playerID;
                                not.PushToken = string.Empty;
                                not.Plataforma = "android";
                                not.Usuario = Common.ParametrosSistema.UsuarioActual;
                                await notificacionBLL.RegistrarDispositivo(not);
                                lblEstado.Text = "Dispositivo registrado correctamente.";
                            }
                            catch (Exception ex)
                            {
                                lblEstado.Text = "Error registrando dispositivo para notificaciones.";

                            }

                        };
                        lblEstado.Text = "Registrando dispositivo para notificaciones.";
                        Com.OneSignal.OneSignal.GetIdsAvailable(idsPrinterDelegate);


                        //Se crea la base de datos local
                        lblEstado.Text = "Creando base de datos local.";
                        DatabaseBLL dbBLL = new DatabaseBLL();
                        RespuestaProceso respuesta = await dbBLL.CrearBaseDeDatos();
                        lblEstado.Text = "Base de datos creada correctamente.";
                        if (respuesta.ProcesadoCorrectamente == true)
                        {
                            btnLogin.IsEnabled = true;
                            IsBusy = false;
                            await Navigation.PopAsync();
                            await Navigation.PushAsync(new HomeView());

                        }
                        else
                        {
                            btnLogin.IsEnabled = true;
                            IsBusy = false;
                            await Navigation.PushAsync(new ErrorView(respuesta.Respuesta));

                        }


                    }
                    else
                    {
                        btnLogin.IsEnabled = true;
                        IsBusy = false;
                        DisplayAlert("Error al Ingresar", "Ocurrió un error inesperado en la autenticación.", "Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    btnLogin.IsEnabled = true;
                    IsBusy = false;
                    if (ex.Message.Contains("Bad Request"))
                    {

                        DisplayAlert("Error al Ingresar", "Nombre de usuario y/o contraseña no válidos.", "Aceptar");
                    }
                    else
                    {
                        DisplayAlert("Error al Ingresar", "Ocurrió un error inesperado en la autenticación.", "Aceptar");
                    }

                }

            }
            else
            {
                DisplayAlert("ERROR", "No tiene conexión a Internet", "Aceptar");
                IsBusy = false;
                btnLogin.IsEnabled = true;
            }



        }

        public async void btnCrearCuentaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CrearCuentaView());
        }

        public async void btnRecuperarClaveClicked(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtUsuario.Text))
            {
                await DisplayAlert("Error", "Debe ingresar su nombre de usuario para poder recuperar la contraseña.", "Aceptar");
            }
            else
            {
                SeguridadBLL seguridadBLL = new SeguridadBLL();
                try
                {
                    if(await seguridadBLL.RecuperarClave(txtUsuario.Text))
                    {
                        await DisplayAlert("Atención", "Se le ha enviado un correo con los datos de recuperación de contraseña.","Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Aceptar");
                    throw;
                }
            }
        }
    }
}
