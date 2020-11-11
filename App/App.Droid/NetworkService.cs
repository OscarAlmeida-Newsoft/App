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
using Android.Net;
using Xamarin.Forms;
using App.Droid;
using Plugin.Connectivity;
using System.Threading.Tasks;

[assembly: Dependency(typeof(NetworkService))]
namespace App.Droid
{
    public class NetworkService : INetworkService
    {
        public async Task<bool> isOnline()
        {
            return CrossConnectivity.Current.IsConnected && await CrossConnectivity.Current.IsRemoteReachable("google.com",msTimeout:15000);           

            //var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
            //var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            //if (activeNetworkInfo != null)
            //{
            //    return activeNetworkInfo.IsConnected;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public bool isOnlineSync() {

            return CrossConnectivity.Current.IsConnected;  
               
        }
    }
}