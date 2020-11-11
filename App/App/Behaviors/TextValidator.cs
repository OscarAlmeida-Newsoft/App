using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.Behaviors
{
    public class TextValidator : Behavior<Entry>
    {
        // Creating BindableProperties with Limited write access: http://iosapi.xamarin.com/index.aspx?link=M%3AXamarin.Forms.BindableObject.SetValue(Xamarin.Forms.BindablePropertyKey%2CSystem.Object) 
        const string charsRegex = @"^[a-zA-Z_áéíóúñ\s]*$";
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(TextValidator), false);

        



        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += bindable_TextChanged;
        }

        private void bindable_TextChanged(object sender, TextChangedEventArgs e)
        {

            
            IsValid = (Regex.IsMatch(e.NewTextValue, charsRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            if(IsValid == false)
            {
                if(e.OldTextValue!=null)
                {
                    ((Entry)sender).Text = e.OldTextValue;
                }
                else
                {
                    ((Entry)sender).Text = String.Empty;
                }
            }
            

            
            //((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;



        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= bindable_TextChanged;
        }
    }
}
