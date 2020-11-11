using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using App.BLL.Operaciones;
using App.Common;
using App.Entities.Operaciones;
using App.Interfaces;
using Xamarin.Forms;

namespace App.Droid.Services {

    [Service]
    public class LocationService : Service, ILocationListener {

        const int SERVICE_RUNNING_NOTIFICATION_ID = 345;
        const string NOTIFICATION_CHANNEL_ID = "com.tdm.app";

        readonly string logTag = "LocationService";
        IBinder binder;

        // Set our location manager as the system location service
        protected LocationManager LocMgr = Android.App.Application.Context.GetSystemService("location") as LocationManager;

        // ILocationListener is a way for the Service to subscribe for updates
        // from the System location Service

        public void OnLocationChanged(Android.Locations.Location location) {


            try {

                if (DependencyService.Get<ICredentialsService>().DoCredentialsExist()) {

                    //LocationChanged(this, new LocationChangedEventArgs(location));

                    // This should be updating every time we request new location updates
                    // both when teh app is in the background, and in the foreground
                    Log.Debug(logTag, $"Latitude is {location.Latitude}");
                    Log.Debug(logTag, $"Longitude is {location.Longitude}");
                    Log.Debug(logTag, $"Altitude is {location.Altitude}");
                    Log.Debug(logTag, $"Speed is {location.Speed}");
                    Log.Debug(logTag, $"Accuracy is {location.Accuracy}");
                    Log.Debug(logTag, $"Bearing is {location.Bearing}");

                    Transporte viajeActivo = null;

                    Util util = new Util();
                    bool? tercero = null;
                    if (util.UsuarioTienePermiso("eventoslogisticosterceros")) {
                        tercero = true;
                    }

                    EventoLogisticoBLL eventoBLL = new EventoLogisticoBLL();
                    List<Transporte> transportesHabilitadosRegistroEventos = new List<Transporte>();



                    Task.Run(async () => {
                        transportesHabilitadosRegistroEventos = await eventoBLL.SeleccionarTransporteHabilitadoRegistroEventos(!await ParametrosSistema.isOnline, tercero);

                        if (transportesHabilitadosRegistroEventos != null && transportesHabilitadosRegistroEventos.Count > 0)
                            viajeActivo = transportesHabilitadosRegistroEventos[0];


                        if (viajeActivo != null) {


                            //The Geocoder class retrieves a list of address from Google over the internet
                            Geocoder geocoder = new Geocoder(Android.App.Application.Context);
                            IList<Android.Locations.Address> addressList = null;
                            Android.Locations.Address address = null;
                            double lat = 0;
                            double lon = 0;
                            string _address = string.Empty;

                            try {
                                if (location.Latitude != 0) {
                                    addressList = geocoder.GetFromLocation(location.Latitude, location.Longitude, 1);
                                    address = addressList.FirstOrDefault();

                                    _address = address.SubThoroughfare + " " + address.Thoroughfare +
                                        ", " + address.Locality + ", " + address.AdminArea + ", " + address.CountryCode;
                                    //_address = address.

                                }

                                lat = location.Latitude;
                                lon = location.Longitude;

                            } catch (Exception ex) {

                            }


                            //Se crea evento logistico para guardar la posicion del gps
                            EventoLogistico evento = new EventoLogistico();

                            //Cero es solo reportar posicion
                            evento.IdTipoEvento = 44; //Evento reportar posicion
                            evento.Placa = viajeActivo.Placa;
                            evento.NumeroManifiesto = viajeActivo.NumeroTransporte;
                            evento.Latitud = (float)lat;
                            evento.Longitud = (float)lon;
                            evento.DescripcionPosicion = _address;
                            evento.FechaEvento = DateTime.Now;
                            evento.NumeroDocumentoConductor = viajeActivo.NumeroDocConductor.ToString();

                            await eventoBLL.GuardarEventoLogistico(evento);
                        }
                    });







                    //Log.Debug("TAG", "Funciona Inicio " + i + " long " + location.Latitude + " lat " + location.Longitude + " - " + _address);





                    //else {
                    //    //Log.Debug("TAG", "Funciona Inicio Sin viaje activo " + i);
                    //}



                }

            } catch {

            }

            

        }

        public void OnProviderDisabled(string provider) {
            ProviderDisabled(this, new ProviderDisabledEventArgs(provider));
        }

        public void OnProviderEnabled(string provider) {
            ProviderEnabled(this, new ProviderEnabledEventArgs(provider));
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras) {
            StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));
        }

        public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
        public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
        public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
        public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };

        public override void OnCreate() {
            base.OnCreate();
            Log.Debug(logTag, "OnCreate called in the Location Service");
        }

        // This gets called when StartService is called in our App class
        [Obsolete("deprecated in base class")]
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId) {
            Log.Debug(logTag, "LocationService started");

            // Check if device is running Android 8.0 or higher and call StartForeground() if so
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) {
                var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                                   //.SetContentTitle(Resources.GetString(Resource.String.app_name)
                                   .SetContentTitle("TDM App")
                                   //.SetContentText(Resources.GetString(Resource.String.notification_text))
                                   .SetContentText("Aplicación ejecutandose en background")
                                   .SetSmallIcon(Resource.Drawable.notification_icon_background)
                                   .SetOngoing(true)
                                   .Build();

                var notificationManager =
                    GetSystemService(NotificationService) as NotificationManager;

                var chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "On-going Notification", NotificationImportance.Min);

                notificationManager.CreateNotificationChannel(chan);

                StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }

            return StartCommandResult.Sticky;
        }

        // This gets called once, the first time any client bind to the Service
        // and returns an instance of the LocationServiceBinder. All future clients will
        // reuse the same instance of the binder
        public override IBinder OnBind(Intent intent) {
            Log.Debug(logTag, "Client now bound to service");

            binder = new LocationServiceBinder(this);
            return binder;
        }

        // Handle location updates from the location manager
        public void StartLocationUpdates() {
            //we can set different location criteria based on requirements for our app -
            //for example, we might want to preserve power, or get extreme accuracy
            var locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.NoRequirement;
            locationCriteria.PowerRequirement = Power.NoRequirement;

            // get provider: GPS, Network, etc.
            var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);
            Log.Debug(logTag, string.Format("You are about to get location updates via {0}", locationProvider));

            // Get an initial fix on location
            LocMgr.RequestLocationUpdates(locationProvider, 180000, 0, this);

            Log.Debug(logTag, "Now sending location updates");
        }

        public override void OnDestroy() {
            base.OnDestroy();
            Log.Debug(logTag, "Service has been terminated");

            // Stop getting updates from the location manager:
            LocMgr.RemoveUpdates(this);
        }

    }
}