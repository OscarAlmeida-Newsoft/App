using App.BLL.Operaciones;
using App.Entities.Operaciones;
using DevExpress.Mobile.Core;
using DevExpress.Mobile.DataGrid;
using DevExpress.Mobile.DataGrid.Theme;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views.Logistica
{
    public partial class HistorialEventosView : ContentPage
    {
        private Int64 _idEvento = 0;

        public HistorialEventosView()
        {
            InitializeComponent();
            Title = "Historial eventos";
            ThemeManager.ThemeName = Themes.Light;
            EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();
            var eventos = eventoBLL.SeleccionarEventosLogisticos(numeroManifiesto: null,estado:null,codigoTipoEvento:null, consultaLocal: true);
            if (eventos != null)
            {
                foreach (EventoLogistico evento in eventos)
                {
                    if (evento.Sincronizado == false)
                    {
                        evento.NombreTipoEvento = "* " + evento.NombreTipoEvento;                        
                    }
                }
                FormatCondition condition = new FormatCondition();
                condition.FieldName = "Estado";
                condition.Expression = "[ErrorSincronizacion].Length > 0";
                condition.PredefinedFormatName = "LightRedFillWithDarkRedText";
                condition.ApplyToRow = false;                
                grid.FormatConditions.Add(condition);
                
                eventos = eventos.OrderByDescending(e => e.FechaEvento).ToList();
                grid.ItemsSource = eventos;
                //grid.SelectionChanged += (s, e) => {
                //    ((Navigation.PushModalAsync(new Logistica.DetalleEventoView(119283));
                //    //DisplayAlert("Atención", "Ver detalles.", "Si", "No");
                //};
            }

        }

        void OnCustomizeCell(CustomizeCellEventArgs e)
        {
            
            if (e.FieldName == "ErrorSincronizacion" && !e.IsSelected)
            {
                if(e.Value != null && !String.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.ForeColor = Color.Red;
                }
                e.Handled = true;
            }
        }

        void OnPopupMenuCustomization(object sender, PopupMenuEventArgs e)
        {
            if (e.MenuType == GridPopupMenuType.DataRow)
            {
                string idValue;
                idValue = grid.GetCellValue(e.RowHandle, "ID").ToString();
                if(!string.IsNullOrEmpty(idValue))
                     _idEvento= Convert.ToInt64(idValue);

                e.Menu.Items.Clear();
                PopupMenuItem itemFilter = new PopupMenuItem();
                itemFilter.Caption = "Ver evento";
                itemFilter.Click += VerEventoClick;
                e.Menu.Items.Insert(0, itemFilter);
            }
        }

        private void VerEventoClick(object sender, EventArgs e)
        {            
            Navigation.PushAsync(new Logistica.DetalleEventoView(_idEvento));
            //throw new NotImplementedException();
        }

        void grid_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            var ee = e.EditableRowData.DataObject;
        }
    }
}
