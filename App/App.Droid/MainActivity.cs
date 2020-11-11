using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Xamarin.Forms;
using App.Entities.IT;
using Android.Content;
using HockeyApp;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using XLabs.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using Android;
using Android.Support.V4.Content;
using Android.Util;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Locations;

namespace App.Droid
{
    [Activity(Label = "Tdm App", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity {
        //20191007 - Se comenta para que no salga notificacion de gps siempre
        //No estaba incluido en version anteror
        //********** GPS Service variables **************
        ////GPSServiceBinder _binder;
        ////GPSServiceConnection _gpsServiceConnection;
        ////Intent _gpsServiceIntent;
        ////private GPSServiceReciever _receiver;

        ////public static MainActivity Instance;


        //////***************************************************

        static readonly int RC_REQUEST_LOCATION_PERMISSION = 1000;
        static readonly string TAG = "MainActivity";
        static readonly string[] REQUIRED_PERMISSIONS = { Manifest.Permission.AccessFineLocation };

        
        protected async override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            //Pregunta por permisos de camera
            await TryToGetPermissions();

            //Se configura el logueo de errores
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
                      
            //Se registra el reporte de errores de hockeyapp
            HockeyApp.Android.CrashManager.Register(this, "ded4af869e434fc3a04cdf3269d86680");
            HockeyApp.Android.Metrics.MetricsManager.Register(this, Application, "ded4af869e434fc3a04cdf3269d86680");

            MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", message => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StartService(intent);
            });

            DisplayCrashReport();

            MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StopService(intent);
            });

            // Start the location service:
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted) {
                Log.Debug(TAG, "User already has granted permission.");
                AppLS.StartLocationService();
            }
            //else {
            //    Log.Debug(TAG, "Have to request permission from the user. ");
            //    RequestLocationPermission();
            //}

            //Notification Opened Delegate
            Com.OneSignal.OneSignal.NotificationOpened exampleNotificationOpenedDelegate = delegate (string message, System.Collections.Generic.Dictionary<string, object> additionalData, bool isActive) {
                try {
                    System.Console.WriteLine("OneSignal Notification opened:\nMessage: {0}", message);

                    if (additionalData != null) {
                        if (additionalData.ContainsKey("customKey"))
                            System.Console.WriteLine("customKey: {0}", additionalData["customKey"]);

                        System.Console.WriteLine("additionalData: {0}", additionalData);
                    }
                } catch (System.Exception e) {
                    System.Console.WriteLine(e.StackTrace);
                }
            };


            

            global::Xamarin.Forms.Forms.Init(this, bundle);
            DevExpress.Mobile.Forms.Init();
            Com.OneSignal.OneSignal.Init();
            LoadApplication(new App());

            var container = new SimpleContainer();
            container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
            container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

            //20191007 - Se comenta para que no salga notificacion de gps siempre
            //No estaba incluido en version anteror
            //StartService(new Intent(this, typeof(GPSService)));
            //// register GPS Service
            //RegisterService();

        }

        protected override void OnResume() {
            Log.Debug(TAG, "OnResume: Location app is moving into foreground");
            base.OnResume();
        }

        protected override void OnPause() {
            Log.Debug(TAG, "OnPause: Location app is moving to background");
            base.OnPause();
        }

        protected override void OnDestroy() {
            Log.Debug(TAG, "OnDestroy: Location app is becoming inactive");
            base.OnDestroy();

            // Stop the location service:
            AppLS.StopLocationService();
        }

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults) {
        //    if (requestCode == RC_REQUEST_LOCATION_PERMISSION) {
        //        if (grantResults.Length == 1 && grantResults[0] == Permission.Granted) {
        //            Log.Debug(TAG, "User granted permission for location.");
        //            App.StartLocationService();
        //        } else {
        //            Log.Warn(TAG, "User did not grant permission for the location.");
        //        }
        //    } else {
        //        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //    }
        //}

        //void RequestLocationPermission() {
        //    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation)) {
        //        var layout = FindViewById(Android.Resource.Id.Content);
        //        Snackbar.Make(layout,
        //                      Resource.String.permission_location_rationale,
        //                      Snackbar.LengthIndefinite)
        //                .SetAction(Resource.String.ok,
        //                           new Action<View>(delegate {
        //                               ActivityCompat.RequestPermissions(this, REQUIRED_PERMISSIONS,
        //                                                                 RC_REQUEST_LOCATION_PERMISSION);
        //                           })
        //                          ).Show();
        //    } else {
        //        ActivityCompat.RequestPermissions(this, REQUIRED_PERMISSIONS, RC_REQUEST_LOCATION_PERMISSION);
        //    }
        //}

        public void HandleLocationChanged(object sender, LocationChangedEventArgs e) {
            var location = e.Location;
            Log.Debug(TAG, "Foreground updating");

            // these events are on a background thread, need to update on the UI thread
            Log.Debug(TAG, "Lat: " + location.Latitude + " Long: " + location.Longitude);
            //RunOnUiThread(() => {
            //    latText.Text = $"Latitude: {location.Latitude}";
            //    longText.Text = $"Longitude: {location.Longitude}";
            //    altText.Text = $"Altitude: {location.Altitude}";
            //    speedText.Text = $"Speed: {location.Speed}";
            //    accText.Text = $"Accuracy: {location.Accuracy}";
            //    bearText.Text = $"Bearing: {location.Bearing}";
            //});
        }

        public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e) {
            Log.Debug(TAG, "Location provider disabled event raised");
        }

        public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e) {
            Log.Debug(TAG, "Location provider enabled event raised");
        }

        public void HandleStatusChanged(object sender, StatusChangedEventArgs e) {
            Log.Debug(TAG, "Location status changed, event raised");
        }

        //Se comenta taskscheduler 20200213
        //private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs) {
        //    var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
        //    LogUnhandledException(newExc);
        //}

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs) {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception) {
            try {
                const string errorFileName = "Fatal.log";
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = System.IO.Path.Combine(libraryPath, errorFileName);
                var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                DateTime.Now, exception.ToString());
                System.IO.File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);
            } catch {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private void DisplayCrashReport() {
            const string errorFilename = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var errorFilePath = System.IO.Path.Combine(libraryPath, errorFilename);

            if (!System.IO.File.Exists(errorFilePath)) {
                return;
            }

            var errorText = System.IO.File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(this)
                .SetPositiveButton("Clear", (sender, args) => {
                    System.IO.File.Delete(errorFilePath);
                })
                .SetNegativeButton("Close", (sender, args) => {
                    // User pressed Close.
                })
                .SetMessage(errorText)
                .SetTitle("Crash Report")
                .Show();
        }

        //20191007 - Se comenta para que no salga notificacion de gps siempre
        //No estaba incluido en version anteror
        //// ************************ Metodos y clases internas para manejo GPS *******************
        //private void RegisterService()
        //{
        //    _gpsServiceConnection = new GPSServiceConnection(_binder);
        //    _gpsServiceIntent = new Intent(Android.App.Application.Context, typeof(GPSService));
        //    BindService(_gpsServiceIntent, _gpsServiceConnection, Bind.AutoCreate);
        //}

        //private void RegisterBroadcastReceiver()
        //{
        //    IntentFilter filter = new IntentFilter(GPSServiceReciever.LOCATION_UPDATED);
        //    filter.AddCategory(Intent.CategoryDefault);
        //    _receiver = new GPSServiceReciever();
        //    RegisterReceiver(_receiver, filter);
        //}

        //private void UnRegisterBroadcastReceiver()
        //{
        //    UnregisterReceiver(_receiver);
        //}

        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    RegisterBroadcastReceiver();
        //}

        //protected override void OnPause()
        //{
        //    base.OnPause();
        //    UnRegisterBroadcastReceiver();
        //}

        //// used to handle the message from a broadcast by implemeting BroadcastReciever
        //[BroadcastReceiver]
        //internal class GPSServiceReciever : BroadcastReceiver
        //{
        //    public static readonly string LOCATION_UPDATED = "LOCATION_UPDATED";
        //    public override void OnReceive(Context context, Intent intent)
        //    {
        //        //if (intent.Action.Equals(LOCATION_UPDATED))
        //        //{
        //        //    MainActivity.Instance.UpdateUI(intent);
        //        //}

        //    }
        //}

        #region RuntimePermissions

        async Task TryToGetPermissions() {

            if ((int)Build.VERSION.SdkInt >= 23) {
                await GetPermissionsAsync();
                return;
            }


        }
        const int RequestLocationId = 0;

        readonly string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.Camera,
                            Manifest.Permission.ReadExternalStorage,
                            Manifest.Permission.WriteExternalStorage,
                            Manifest.Permission.AccessCoarseLocation,
                            Manifest.Permission.AccessFineLocation,
                            Manifest.Permission.AccessNetworkState,
                            Manifest.Permission.Internet,
                            Manifest.Permission.AccessWifiState,
                            Manifest.Permission.ForegroundService,
                            Manifest.Permission.RequestCompanionRunInBackground,
                            Manifest.Permission.RequestCompanionUseDataInBackground

             };

        async Task GetPermissionsAsync() {

            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted) {
                //TODO change the message to show the permissions name
                Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();
                return;
            }

            if (ShouldShowRequestPermissionRationale(permission)) {
                //set alert for executing the task
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need special permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) => {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });

                Dialog dialog = alert.Create();
                dialog.Show();


                return;
            }

            RequestPermissions(PermissionsGroupLocation, RequestLocationId);

        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
            switch (requestCode) {
                case RequestLocationId: {
                    if (grantResults[0] == (int)Android.Content.PM.Permission.Granted) {
                        Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();
                        Log.Debug(TAG, "User granted permission for location.");
                        AppLS.StartLocationService();
                        

                    } else {
                        //Permission Denied :(
                        Toast.MakeText(this, "Special permissions denied", ToastLength.Short).Show();

                    }
                }
                break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion

    }

}

