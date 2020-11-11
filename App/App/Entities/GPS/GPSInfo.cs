using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.GPS
{
    public class GPSInfo
    {
        public bool GPSEnabled { get; set; }
        public double Latitude { get; set; }

        public double Longitude { get; set; }
        public string address { get; set; }
    }
}
