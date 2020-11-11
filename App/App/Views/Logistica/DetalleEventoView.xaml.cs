using App.BLL.Operaciones;
using App.Common;
using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Logistica
{
    public partial class DetalleEventoView : ContentPage
    {
        private Int64 _idEvento = 0;
        
        public DetalleEventoView(Int64 idEvento)
        {
            _idEvento = idEvento;
            Title = "Detalles Evento";
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            ScrollView scrollView = new ScrollView();
            
            StackLayout stackPanel = new StackLayout();
            scrollView.Content = stackPanel;
            stackPanel.Padding = new Thickness(10);
            
            EventoLogisticoBLL eventoLogisticosBLL = new EventoLogisticoBLL();
            EventoLogistico evento = new EventoLogistico();
            evento = eventoLogisticosBLL.SeleccionarEventosLogisticoporID(_idEvento);
            
            if (evento != null)
            {
                List<CampoEventoLogistico> camposTipoEvento = await eventoLogisticosBLL.SeleccionarCamposPorEvento(evento.IdTipoEvento, consultaLocal: true);
                if (camposTipoEvento != null)
                {
                    List<TipoEventoLogistico> tiposEventos = await eventoLogisticosBLL.SeleccionarTiposEventoLogistico(consultaLocal: true);
                    TipoEventoLogistico tipoEvento = (from t in tiposEventos
                                                      where t.CodigoEvento == evento.IdTipoEvento
                                                      select t).FirstOrDefault();

                    if (tipoEvento != null)
                    {
                        Label lblNombreEvento = new Label();
                        lblNombreEvento.Text = tipoEvento.NombreEvento;
                        lblNombreEvento.FontSize = 30;                        
                        stackPanel.Children.Add(lblNombreEvento);
                    }

                    Label lblTituloFechaEvento = new Label();
                    lblTituloFechaEvento.Text = "Fecha Evento";
                    lblTituloFechaEvento.FontSize = 25;
                    stackPanel.Children.Add(lblTituloFechaEvento);

                    Label lblFechaEvento = new Label();
                    lblFechaEvento.Text = evento.FechaEvento.Value.ToString();
                    lblFechaEvento.FontSize = 20;
                    stackPanel.Children.Add(lblFechaEvento);

                    Label lblTituloNumeroManifiesto = new Label();
                    lblTituloNumeroManifiesto.Text = "Número Manifiesto";
                    lblTituloNumeroManifiesto.FontSize = 25;
                    stackPanel.Children.Add(lblTituloNumeroManifiesto);

                    Label lblNumeroViaje = new Label();
                    lblNumeroViaje.Text = evento.NumeroManifiesto.ToString();
                    lblNumeroViaje.FontSize = 20;
                    stackPanel.Children.Add(lblNumeroViaje);

                    Label tituloCampo;
                    Label valorCampo;

                    foreach (CampoEventoLogistico campo in camposTipoEvento)
                    {
                        tituloCampo = new Label();
                        valorCampo = new Label();

                        tituloCampo.Text = campo.TituloCampoEventoLogistico;
                        tituloCampo.FontSize = 25;

                        valorCampo.Text = Util.ObtenerValorDinamicamente(evento, campo.NombreCampoEventoLogistico);
                        valorCampo.FontSize = 20;

                        stackPanel.Children.Add(tituloCampo);
                        stackPanel.Children.Add(valorCampo);
                    }
                    if (!string.IsNullOrEmpty(evento.ErrorSincronizacion))
                    {
                        //Label lblTituloError = new Label();
                        //lblTituloError.Text = "Resultado Evento";
                        //stackPanel.Children.Add(lblTituloError);
                        Label lblError = new Label();
                        lblError.Text = evento.ErrorSincronizacion;
                        lblError.TextColor = Color.Red;
                        stackPanel.Children.Add(lblError);
                    }
                    
                    Content = scrollView;
                }
            }

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }
    }
}
