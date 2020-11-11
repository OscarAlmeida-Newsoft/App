using App.BLL.Operaciones;
using App.BLL.Turnos;
using App.Common;
using App.DAO.Operaciones;
using App.Entities.IT;
using App.Entities.Operaciones;
using App.Entities.Turnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Turnos
{
    public partial class TurnosView : ContentPage
    {
        public Int64 _idTurno = 0;
        public TurnosView()
        {
            InitializeComponent();
            Title = "Turnos";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Util util = new Util();
            if (!util.UsuarioTienePermiso("enturnarplacasilimitadas"))
            {
                txtPlaca.IsVisible = false;
                btnBuscar.IsVisible = false;
            }
            IsBusy = true;
            var turnos = await buscarTurnos();
            lvTurnos.ItemsSource = turnos;
            IsBusy = false;
        }

        private async void btnBuscar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPlaca.Text))
            {
                IsBusy = true;
                var turnos = await buscarTurnos();                
                lvTurnos.ItemsSource = turnos;
                IsBusy = false;
            }
            else
            {
                await DisplayAlert("Turnos", "Debe ingresar una placa", "Aceptar", "Cancelar");
            }


        }

        private async Task<List<Enturnamiento>> buscarTurnos()
        {
            
            Util util = new Util();
            EnturnamientoBLL enturnamientoBLL = new EnturnamientoBLL();
            List<Enturnamiento> turnos = new List<Enturnamiento>();
            
            
            if (!util.UsuarioTienePermiso("enturnarplacasilimitadas"))
            {
                turnos = await enturnamientoBLL.ObtenerTurnosPorUsuario();
                

            }
            else
            {
                
                if (!string.IsNullOrEmpty(txtPlaca.Text))
                {
                    turnos = await enturnamientoBLL.ObtenerTurnosPorPlaca(txtPlaca.Text);                    
                }
                
            }
            if (turnos != null && turnos.Count > 0)
            {   
                AgenciaBLL agenciaBLL = new AgenciaBLL();
                var agencias = agenciaBLL.SeleccionarAgencias();
                foreach (Enturnamiento turno in turnos)
                {
                    switch (turno.Estado)
                    {
                        case "TR":
                            turno.DescripcionEstado = "Tránsito";
                            break;
                        case "DP":
                            turno.DescripcionEstado = "Disponible";
                            break;
                        default:
                            break;
                    }
                    var agencia = agencias.FirstOrDefault(a=>a.CodigoAgencia == turno.PuestoExpedicion);
                    if(agencia!=null)
                    {
                        turno.DescripcionPuestoExpedicion = agencia.NombreAgencia;
                    }
                }
            }
            return turnos;
        }
        
        async void OnEditarClicked(object sender, EventArgs args)
        {
            var btn = sender as Button;           
            Enturnamiento turno = btn.BindingContext as Enturnamiento;           
           
            await Navigation.PushAsync(new DetalleTurnoView(turno));
        }

        async void OnEliminarClicked(object sender, EventArgs args)
        {
            var answer = await DisplayAlert("Eliminar Turno", "Esta seguro de eliminar este turno?", "Si", "No");
            if (answer)
            {
                var btn = sender as Button;
                Enturnamiento turno = btn.BindingContext as Enturnamiento;
                if (turno != null)
                {
                    EnturnamientoBLL enturnamientoBLL = new EnturnamientoBLL();
                    List<RespuestaServicio> respuestas = await enturnamientoBLL.EliminarTurno(turno.IdTurno);
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
            }
        }

        async void OnCrearClicked(object sender,EventArgs args)
        {
            await Navigation.PushAsync(new DetalleTurnoView(null));
        }
        
    }
}
