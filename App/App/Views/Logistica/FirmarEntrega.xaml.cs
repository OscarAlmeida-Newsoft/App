using App.BLL.Comercial;
using App.Entities.Comercial;
using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views.Logistica
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirmarEntrega : ContentPage
    {
        private string numeroEntrega;
        private int _idevento;
        private string _nombreEvento;

        public FirmarEntrega(string entrega, int idEvento, string nombreEvento)
        {
            numeroEntrega = entrega;
            _idevento = idEvento;
            _nombreEvento = nombreEvento;
            InitializeComponent();
            Title = "Firmar Entrega " + entrega;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
            BtnGuardar.IsEnabled = false;
            
            Stream sigimage = await Padview.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);
            if (sigimage == null)
            {
                return;
            }

            BinaryReader br = new BinaryReader(sigimage);
            br.BaseStream.Position = 0;
            Byte[] All = br.ReadBytes((int)sigimage.Length);
            byte[] image = (byte[])All;
            ImageSource imageSource = null;
            imageSource = ImageSource.FromStream(() => new MemoryStream(image));

            

            string _base64String = Convert.ToBase64String(image);

            EntregaDetalleFirma adjunto = new EntregaDetalleFirma();
            adjunto.NombreArchivo = numeroEntrega;
            adjunto.Extension = "png";
            adjunto.Base64String = "data:image/png;base64," + _base64String;
            adjunto.PdfCombinado = false;
            adjunto.Observaciones = txtObservaciones.Text;

            EntregaBLL entregaBBL = new EntregaBLL();
            string respuesta = await entregaBBL.GuardarImagenFirmaEntrega(adjunto);

            IsBusy = false;
            BtnGuardar.IsEnabled = true;

            DisplayAlert("Message", "Firma guardada correctamente", "OK");
            await Navigation.PushAsync(new Logistica.CrearEventoView(_idevento, _nombreEvento));
        }
    }
}