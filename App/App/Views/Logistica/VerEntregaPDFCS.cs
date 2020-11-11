using App.CustomViewPDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.Views.Logistica
{
    public class VerEntregaPDFCS : ContentPage
    {
        public VerEntregaPDFCS(string ruta)
        {
            Padding = new Thickness(0, 20, 0, 0);
            Content = new StackLayout
            {
                Children = {
                    new CustomWebView {
                        Uri = ruta,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    }
                }
            };
        }
    }
}
