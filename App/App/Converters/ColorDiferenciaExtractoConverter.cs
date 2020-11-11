using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.Converters
{
    public class ColorDiferenciaExtractoConverter: Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool saldoafavor = false;

            if ((Double)value >= 0)
                saldoafavor = true;


            switch (saldoafavor)
            {
                case true:
                    return Color.Green;
                case false:
                    return Color.Red;              
            }

            return Color.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // You probably don't need this, this is used to convert the other way around
            // so from color to yes no or maybe
            throw new NotImplementedException();
        }
    }
}
