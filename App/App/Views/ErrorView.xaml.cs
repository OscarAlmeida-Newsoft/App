using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace App.Views
{
    public partial class ErrorView : ContentPage
    {
        public ErrorView(string mensaje)
        {
            InitializeComponent();
            lblMensaje.Text = mensaje;
        }
    }
}
