using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.GPS
{
    public class LatLng
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public LatLng(double lat, double lng)
        {
            this.Latitude = lat;
            this.Longitude = lng;
        }
    }
}
