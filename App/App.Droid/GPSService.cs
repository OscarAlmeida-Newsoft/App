using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Locations;
using App.Entities.GPS;
using App.Entities.Operaciones;
using App.Common;
using App.BLL.Operaciones;

namespace App.Droid
{
    [Service]
    public class GPSService //: Service, ILocationListener
    {
        //20191007 - Se comenta para que no salga notificacion de gps siempre
        //No estaba incluido en version anteror //Clase completa

    //    private string _location = string.Empty;
    //    private string _address = string.Empty;
    //    private string _remarks = string.Empty;

    //    public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;

    //    public const string LOCATION_UPDATE_ACTION = "LOCATION_UPDATED";
    //    private Location _currentLocation;

    //    private DateTime execDate = DateTime.Now;

    //    IBinder _binder;

    //    protected LocationManager _locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(LocationService);
    //    public override IBinder OnBind(Intent intent)
    //    {
    //        _binder = new GPSServiceBinder(this);
    //        return _binder;
    //    }

    //    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    //    {
    //        // Code not directly related to publishing the notification has been omitted for clarity.
    //        // Normally, this method would hold the code to be run when the service is started.

    //        base.OnStartCommand(intent, flags, startId);

    //        var notification = new Notification.Builder(this)
    //            .SetContentTitle("TDM App")
    //            .SetContentText("Localización GPS")
    //            .SetSmallIcon(Resource.Drawable.icon)
    //            //.SetContentIntent(BuildIntentToShowMainActivity())
    //            .SetOngoing(true)
    //            //.AddAction(BuildRestartTimerAction())
    //            //.AddAction(BuildStopServiceAction())
    //            .Build();

    //        // Enlist this instance of the service as a foreground service
    //        StartForeground((int)NotificationFlags.ForegroundService, notification);
    //        return StartCommandResult.Sticky;
    //    }

    //    public void StartLocationUpdates()
    //    {
    //        Criteria criteriaForGPSService = new Criteria
    //        {
    //            //A constant indicating an approximate accuracy  
    //            Accuracy = Accuracy.Coarse,
    //            PowerRequirement = Power.Medium
    //        };

    //        var locationProvider = _locationManager.GetBestProvider(criteriaForGPSService, true);
    //        _locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);

    //    }

    //    public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
    //    public void OnLocationChanged(Location location)
    //    {
    //        try
    //        {
    //            _currentLocation = location;

    //            DateTime dateNow = DateTime.Now;

    //            if (execDate.AddMinutes(1.0) < dateNow)
    //            {
    //                execDate = dateNow;

    //                if (_currentLocation == null)
    //                    _location = "Incapaz de determinar la ubicación.";
    //                else
    //                {
    //                    _location = String.Format("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude);

    //                    Geocoder geocoder = new Geocoder(this);

    //                    //The Geocoder class retrieves a list of address from Google over the internet  
    //                    IList<Address> addressList = geocoder.GetFromLocation(_currentLocation.Latitude, _currentLocation.Longitude, 10);

    //                    Address addressCurrent = addressList.FirstOrDefault();

    //                    _address = addressCurrent.GetAddressLine(0);

    //                    //if (addressCurrent != null)
    //                    //{
    //                    //    StringBuilder deviceAddress = new StringBuilder();

    //                    //    for (int i = 0; i < addressCurrent.MaxAddressLineIndex; i++)
    //                    //        deviceAddress.Append(addressCurrent.GetAddressLine(i))
    //                    //            .AppendLine(",");

    //                    //    _address = deviceAddress.ToString();
    //                    //}
    //                    //else
    //                    //    _address = "Incapaz de determinar la dirección.";

    //                    //IList<Address> source = geocoder.GetFromLocationName(_sourceAddress, 1);
    //                    //Address addressOrigin = source.FirstOrDefault();

    //                    //var coord1 = new LatLng(addressOrigin.Latitude, addressOrigin.Longitude);
    //                    var coord2 = new LatLng(addressCurrent.Latitude, addressCurrent.Longitude);

    //                    //var distanceInRadius = Utils.HaversineDistance(coord1, coord2, Utils.DistanceUnit.Miles);

    //                    //_remarks = string.Format("Your are {0} miles away from your original location.", distanceInRadius);

    //                    Intent intent = new Intent(this, typeof(MainActivity.GPSServiceReciever));
    //                    intent.SetAction(MainActivity.GPSServiceReciever.LOCATION_UPDATED);
    //                    intent.AddCategory(Intent.CategoryDefault);
    //                    intent.PutExtra("Location", _location);
    //                    intent.PutExtra("Address", _address);
    //                    //intent.PutExtra("Remarks", _remarks);
    //                    SendBroadcast(intent);

    //                    //Obtener información del viaje                         
    //                    TransporteBLL transporteBLL = new TransporteBLL();
    //                    HistorialActivacionManifiesto historialActivacion = transporteBLL.SeleccionarHistorialManifiestoActivoPorConductor(ParametrosSistema.NumeroIdentificacionUsuarioActual);

    //                    EventoLogistico evento = new EventoLogistico();

    //                    if (historialActivacion != null)
    //                    {
    //                        evento.Placa = historialActivacion.Placa;
    //                        evento.NumeroManifiesto = historialActivacion.NumeroManifiesto;
    //                        evento.Latitud = (float)addressCurrent.Latitude;
    //                        evento.Longitud = (float)addressCurrent.Longitude;
    //                        evento.DescripcionPosicion = _address;
    //                        evento.FechaEvento = DateTime.Now;
    //                        evento.campo1 = "Evento GPS";

    //                        BLL.Operaciones.EventoLogisticoBLL eventoBLL = new BLL.Operaciones.EventoLogisticoBLL();
    //                        eventoBLL.GuardarPosicionVehiculoGPS(evento);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _address = "Unable to determine the address.";
    //        }

    //    }

    //    public void OnStatusChanged(string provider, Availability status, Bundle extras)
    //    {
    //        //TO DO:  
    //    }

    //    public void OnProviderDisabled(string provider)
    //    {
    //        //TO DO:  
    //    }

    //    public void OnProviderEnabled(string provider)
    //    {
    //        //TO DO:  
    //    }
    //}
    //public class GPSServiceBinder : Binder
    //{
    //    public GPSService Service { get { return this.LocService; } }
    //    protected GPSService LocService;
    //    public bool IsBound { get; set; }
    //    public GPSServiceBinder(GPSService service) { this.LocService = service; }
    //}
    //public class GPSServiceConnection : Java.Lang.Object, IServiceConnection
    //{

    //    GPSServiceBinder _binder;

    //    public event Action Connected;
    //    public GPSServiceConnection(GPSServiceBinder binder)
    //    {
    //        if (binder != null)
    //            this._binder = binder;
    //    }

    //    public void OnServiceConnected(ComponentName name, IBinder service)
    //    {
    //        GPSServiceBinder serviceBinder = (GPSServiceBinder)service;

    //        if (serviceBinder != null)
    //        {
    //            this._binder = serviceBinder;
    //            this._binder.IsBound = true;
    //            serviceBinder.Service.StartLocationUpdates();
    //            if (Connected != null)
    //                Connected.Invoke();
    //        }
    //    }
    //    public void OnServiceDisconnected(ComponentName name) { this._binder.IsBound = false; }
    //}

    //public static class startServiceClass
    //{
    //    //*********************** Metodos para ejecutar servicios en primer plano ******************************************
    //    //BORRAR
    //    public static void StartForegroundServiceComapt<T>(this Context context, Bundle args = null) where T : Service
    //    {
    //        var intent = new Intent(context, typeof(T));
    //        if (args != null)
    //        {
    //            intent.PutExtras(args);
    //        }

    //        context.StartService(intent);

    //        //if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.)
    //        //{
    //        //     //context. s StartForegroundService(intent);
    //        //}
    //        //else
    //        //{
    //        //    context.StartService(intent);
    //        //}
    //    }


        //********************************************************************************************************************
    }
}