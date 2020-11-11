using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App.Interfaces;

using App.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationService))]
namespace App.Droid
{
    class NotificationService : INotificationService
    {
        public void ShowLocalNotification(string title, string text, DateTime time)
        {

            //Se comenta esta parte mientras se define que pantalla mostrar cuando se abre la notificación.
            //// Set up an intent so that tapping the notifications returns to this app:
            //Intent intent = new Intent(Application.Context, typeof(MainActivity));

            //// Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            //const int pendingIntentId = 0;
            //PendingIntent pendingIntent =
            //    PendingIntent.GetActivity(Application.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);







            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(Application.Context)
            .SetContentTitle(title).SetContentText(text)
            .SetSmallIcon(Resource.Drawable.icon);
            //.SetContentIntent(pendingIntent);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
            Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);

            builder.SetWhen(time.Millisecond);
        }
    }
}