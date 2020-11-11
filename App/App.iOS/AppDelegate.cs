using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.IO;
using HockeyApp;
using XLabs.Platform.Device;
using XLabs.Ioc;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Media;
using XLabs.Forms;

namespace App.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate //global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            //Se comenta taskscheduler 20200213
            //TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            //Se configura HockeyApp
            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("ded4af869e434fc3a04cdf3269d86680");
            manager.StartManager();
            
            DevExpress.Mobile.Forms.Init();

            var container = new SimpleContainer();
            container.Register<IDevice>(t => AppleDevice.CurrentDevice);
            container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            container.Register<INetwork>(t => t.Resolve<IDevice>().Network);
            container.Register<IMediaPicker>(t => t.Resolve<IMediaPicker>());
            Resolver.SetResolver(container.GetResolver());

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        //Se comenta taskscheduler 20200213
        //private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        //{
        //    var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
        //    LogUnhandledException(newExc);
        //}

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Resources);
                var errorFilePath = System.IO.Path.Combine(libraryPath, errorFileName);
                var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                DateTime.Now, exception.ToString());
                System.IO.File.WriteAllText(errorFilePath, errorMessage);

                
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private static void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            var alertView = new UIAlertView("Crash Report", errorText, null, "Close", "Clear") { UserInteractionEnabled = true };
            alertView.Clicked += (sender, args) =>
            {
                if (args.ButtonIndex != 0)
                {
                    File.Delete(errorFilePath);
                }
            };
            alertView.Show();
        }
    }
}
