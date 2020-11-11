using App.BLL.IT;
using App.BLL.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views
{
    public partial class CrearCuentaView : ContentPage
    {
        public CrearCuentaView()
        {
            InitializeComponent();
            pickerGenero.Items.Add("Femenino");
            pickerGenero.Items.Add("Masculino");
        }

        public async void btnRegistrarClicked(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtCelular.Text) || String.IsNullOrEmpty(txtNombre.Text) || String.IsNullOrEmpty(txtNumeroIdentificacion.Text) ||
                String.IsNullOrEmpty(txtPassword.Text) || pickerGenero.SelectedIndex < 0)
            {
                await DisplayAlert("Error", "Debe ingresar los campos obligatorios", "Aceptar");
            }
            else if(txtPassword.Text != txtPassword2.Text)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden.", "Aceptar");
            }
            else if(txtPassword.Text.Length < 6)
            {
                await DisplayAlert("Error", "La contraseña debe ser de mínimo 6 caracteres de longitud.", "Aceptar");
            }
            else
            {
                Usuario usuario = new Usuario();
                usuario.CodigoUsuario = txtNumeroIdentificacion.Text;
                usuario.ContraseñaUsuario = txtPassword.Text;
                usuario.CorreoUsuario = txtCorreoElectronico.Text;
                usuario.NombreUsuario = txtNombre.Text;
                usuario.NumeroIdentificacion = Convert.ToInt32(txtNumeroIdentificacion.Text);
                usuario.TipoIdentificacion = "CC";
                usuario.Sexo = pickerGenero.Items[pickerGenero.SelectedIndex] == "Masculino" ? "M" : "F";
                usuario.TelefonoCelular = txtCelular.Text;
                usuario.UsuarioCreacion = "tdmapp";
                SeguridadBLL seguridadBLL = new SeguridadBLL();
                string respuesta = await seguridadBLL.Registrar(usuario);
                if (await DisplayAlert("Información", respuesta, "Volver a inicio", "Cerrar"))
                {
                    await Navigation.PopAsync();
                }

            }
                
        }
        public async void btnCancelarClicked(object sender, EventArgs e)
        {
            if(await DisplayAlert("Atención","Seguro desea cancelar?","Si","No"))
            {
                await Navigation.PopAsync();
            }
        }
    }
}
