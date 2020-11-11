using App.Entities.IT;
using App.Interfaces;

using Xamarin.Forms;
using XLabs.Ioc;

namespace App
{
    public class App : Application
    {
        public static string AppName { get { return "TdmApp"; } }
        public App()
        {


            if (DependencyService.Get<ICredentialsService>().DoCredentialsExist())
            {

                MainPage = new NavigationPage(new Views.HomeView());
            }
            else
            {
                MainPage = new NavigationPage(new Views.LoginView());
            }
            
        }


        protected async override void OnStart()
        {
            // Handle when your app starts
           




        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            var message = new StartLongRunningTaskMessage();
            MessagingCenter.Send(message, "StartLongRunningTaskMessage");

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            var message = new StopLongRunningTaskMessage();
            MessagingCenter.Send(message, "StopLongRunningTaskMessage");
        }
    }
}
