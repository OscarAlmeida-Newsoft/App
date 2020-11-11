using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.CustomCells
{
    public class PendienteSincronizarCustomCell: ViewCell
    {

        public static readonly BindableProperty NombreTipoEventoProperty =
        BindableProperty.Create("NombreTipoEvento", typeof(string), typeof(PendienteSincronizarCustomCell), "--");

        public static readonly BindableProperty FechaEventoProperty =
            BindableProperty.Create("FechaEvento", typeof(DateTime), typeof(PendienteSincronizarCustomCell), default(DateTime));

        public static readonly BindableProperty NumeroManifiestoProperty =
            BindableProperty.Create("NumeroManifiesto", typeof(long?), typeof(PendienteSincronizarCustomCell), null);

        public string NombreTipoEvento
        {
            get { return (string)GetValue(NombreTipoEventoProperty); }
            set { SetValue(NombreTipoEventoProperty, value); }
        }

        public DateTime FechaEvento
        {
            get { return (DateTime)GetValue(FechaEventoProperty); }
            set { SetValue(FechaEventoProperty, value); }
        }

        public long NumeroManifiesto
        {
            get { return (long)GetValue(NumeroManifiestoProperty); }
            set { SetValue(NumeroManifiestoProperty, value); }
        }














        public PendienteSincronizarCustomCell()
        {
            Label lblNombreEvento = new Label();
            lblNombreEvento.SetBinding(Label.TextProperty, ".");
            Label lblFechaCreacion = new Label();
            lblFechaCreacion.SetBinding(Label.TextProperty, new Binding("FechaEvento"));
            Label lblNumeroManifiesto = new Label();
            lblNumeroManifiesto.SetBinding(Label.TextProperty, new Binding("NumeroManifiesto"));

            View = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(15, 5, 5, 15),
                Children = {
                    new StackLayout {
                        Orientation = StackOrientation.Vertical,
                        Children = {
                            new Label() { Text = "Evento" },
                            new Label() { Text = this.NombreTipoEvento }
                        }
                    },

                    new StackLayout {
                        Orientation = StackOrientation.Vertical,
                        Children = {
                            new Label() { Text = "Manifiesto" },
                            lblNumeroManifiesto
                        }
                    },

                    new StackLayout {
                        Orientation = StackOrientation.Vertical,
                        Children = {
                            new Label() { Text = "Fecha" },
                            lblFechaCreacion
                        }
                    },

                }
            };
        }
    }
}
