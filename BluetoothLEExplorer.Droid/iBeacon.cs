using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iBeaconImg;
using Triangulation;

namespace iBeaconImg
{
    public class Position
    {
        public long X {get;set;}
        public long Y {get;set;}

        public Position() { }

        public Position(long x, long y) {
            X = x;
            Y = y;
        }
    }

    public class iBeacon : Position
    {
		private double distance;
        public double? RSSI { get; set; }
        public double Distance { 
            get
            {
                if (RSSI.HasValue)
                    return Trilateration.SignalStrengthToMeter(RSSI.Value);
                else
                    return distance;
            }
            set 
            {
                RSSI = null;
                distance = value;
            }
        }
        public iBeacon() { }

        public iBeacon(long x, long y, double rssi) 
            :base(x, y)
        {
            this.RSSI = rssi;
        }

        public iBeacon(long x, long y, double? rssi, double distance)
            : base(x, y)
        {
            this.RSSI = rssi;
            this.Distance = distance;
        }
    }

}
