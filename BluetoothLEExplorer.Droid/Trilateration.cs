using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iBeaconImg;

namespace Triangulation
{
	public class Trilateration
	{
		public static double calculateAccuracy(int txPower, double rssi)
		{
			if (rssi == 0)
			{
				return -1.0; // if we cannot determine accuracy, return -1.
			}

			double ratio = rssi * 1.0 / txPower;
			if (ratio < 1.0)
			{
				return Math.Pow(ratio, 10)*1000000;
			}
			else
			{
				double accuracy = (0.89976) * Math.Pow(ratio, 7.7095) + 0.111;
				return accuracy;
			}
		}   

		public static double SignalStrengthToMeter(double rssi) 
		{
			double exponent = -(rssi + 51.504)/16.532;
			double distance = 730.24198315 + 52.33325511*rssi + 1.35152407*Math.Pow(rssi, 2) 
				+ 0.01481265*Math.Pow(rssi, 3) + 0.00005900*Math.Pow(rssi, 4)+0.00541703*180;
			return calcFeetToMeter(distance);
		}

		// Convert Feet into Meter
		public static double calcFeetToMeter(double rssi)
		{
			return rssi * 0.3048;
		}

		// Description of calDistToDeg function
		//     
		// To get the myLocation, rssi distance should be converted into 
		// latitude and longitude unit. 
		// This function convert rssi distance into lat long decimal unit.
		// 
		public static double calDistToDeg(double dist) {
			double result;
			double DistToDeg;

			int lat = 42;
			//double EarthRadius = 6367449;
			//double a = 6378137;
			//double b = 6356752.3;
			double ang = lat*(Math.PI/180);

			// This function will calculate the longitude distance based on the latitude
			// More information is 
			// http://en.wikipedia.org/wiki/Geographic_coordinate_system#Expressing_latitude_and_longitude_as_linear_units

			//               result = Math.cos(ang)*Math.sqrt((Math.Pow(a,4)*(Math.Pow(Math.cos(ang),2))
			//                               + (Math.Pow(b,4)*(Math.Pow(Math.sin(ang),2)))) 
			//                               / (Math.Pow((a*Math.cos(ang)),2)+Math.Pow((b*Math.sin(ang)),2)))
			//                               * Math.PI/180;

			DistToDeg = 82602.89223259855;  // unit (meter), based on 42degree.
			result = dist/DistToDeg;                // convert distance to lat,long degree.
			return result;

		}

		public static double getLongitude(double Lat1, double Long1, double rssi1,
			double Lat2, double Long2, double rssi2,
			double Lat3, double Long3, double rssi3)
		{
			double dist1, dist2, dist3;
			double MyLong;

			dist1 = calDistToDeg(10);      //calDistToDeg(SignalStrengthToDistance(rssi1));
			dist2 = calDistToDeg(12);      //calDistToDeg(SignalStrengthToDistance(rssi2));
			dist3 = calDistToDeg(8);       //calDistToDeg(SignalStrengthToDistance(rssi3));

			MyLong = (2 * (Lat3 - Lat1) * (Math.Pow(dist2, 2) - Math.Pow(dist1, 2))
				- 2 * (Lat2 - Lat1) * (Math.Pow(dist3, 2) - Math.Pow(dist1, 2)))
				/ (4 * (Lat2 - Lat1) * (Long3 - Long1) - 4 * (Lat3 - Lat1) * (Long2 - Long1));

			return MyLong;
		}

		public static double getLatitude(double Lat1, double Long1, double rssi1,
			double Lat2, double Long2, double rssi2,
			double Lat3, double Long3, double rssi3)
		{

			//double magnitude = 100000000;
			double dist1, dist2, dist3;
			double MyLat;

			dist1 = calDistToDeg(SignalStrengthToMeter(rssi1));
			dist2 = calDistToDeg(SignalStrengthToMeter(rssi2));
			dist3 = calDistToDeg(SignalStrengthToMeter(rssi3));

			MyLat = (2 * (Long2 - Long1) * (Math.Pow(dist3, 2) - Math.Pow(dist1, 2))
				- 2 * (Long3 - Long1) * (Math.Pow(dist2, 2) - Math.Pow(dist1, 2)))
				/ (4 * ((Lat2 - Lat1) * (Long3 - Long1) - (Lat3 - Lat1) * (Long2 - Long1)));

			return MyLat;
		}

		public static iBeacon myRotationForBeacon(double x, double y, double dist, double deg)
		{
			double[] arr = myRotation(x, y, dist, deg);
			return new iBeacon((long)arr[0], (long)arr[1], null, (long)arr[2]);
		}

		public static double[] myRotation(double x, double y, double dist, double deg)
		{

			double tmpX, tmpY;
			//ArrayList<Double> myLocation = null;
			double[] myLocation = new double[3];

			tmpX = x * Math.Cos((Math.PI / 180) * deg) - y * Math.Sin((Math.PI / 180) * deg);
			tmpY = x * Math.Sin((Math.PI / 180) * deg) + y * Math.Cos((Math.PI / 180) * deg);

			//               myLocation.add(tmpX);
			//               myLocation.add(tmpY);
			myLocation[0] = tmpX;
			myLocation[1] = tmpY;
			myLocation[2] = dist;

			return myLocation;
		}


		public static Position Trilaterate(iBeacon beacon1, iBeacon beacon2, iBeacon beacon3)
		{    
			//Dessa tre används för att förenkla beräkningen
			var tmpWAP1 = new iBeacon();
			var tmpWAP2 = new iBeacon();
			var tmpWAP3 = new iBeacon();

			double tmpLat2, tmpLong2, tmpLat3, tmpLong3;
			double tmpSlide, deg;
			double MyLat, MyLong;

			var MyPosition = new Position();

			//Utgå från att Lat1 är origo
			tmpLat2 = beacon2.X - beacon1.X;
			tmpLong2 = beacon2.Y - beacon1.Y;
			tmpLat3 = beacon3.X - beacon1.X;
			tmpLong3 = beacon3.Y - beacon1.Y;

			tmpSlide = Math.Sqrt(Math.Pow(tmpLat2, 2) + Math.Pow(tmpLong2, 2));

			//deg = (180/Math.PI)*Math.acos( ((Math.Pow(tmpLat2,2) + Math.Pow(tmpSlide,2) - Math.Pow(tmpLong2, 2)) / (2*tmpLat2*tmpSlide)) );
			deg = (180 / Math.PI) * Math.Acos(Math.Abs(tmpLat2) / Math.Abs(tmpSlide));

			// 1 quadrant
			if ((tmpLat2 > 0 && tmpLong2 > 0))
			{
				deg = 360 - deg;
			}
			else if ((tmpLat2 < 0 && tmpLong2 > 0))
			{
				deg = 180 + deg;
			}
			// 3 quadrant
			else if ((tmpLat2 < 0 && tmpLong2 < 0))
			{
				deg = 180 - deg;
			}
			// 4 quadrant
			else if ((tmpLat2 > 0 && tmpLong2 < 0))
			{
				//deg = deg;
			}

			tmpWAP1.X = 0;
			tmpWAP1.Y = 0;
			tmpWAP1.Distance = beacon1.Distance;
			tmpWAP2 = myRotationForBeacon(tmpLat2, tmpLong2, beacon2.Distance, deg);
			tmpWAP3 = myRotationForBeacon(tmpLat3, tmpLong3, beacon3.Distance, deg);


			MyLat = (Math.Pow(tmpWAP1.Distance, 2) - Math.Pow(tmpWAP2.Distance, 2) + Math.Pow(tmpWAP2.X, 2)) / (2 * tmpWAP2.X);

			MyLong = (Math.Pow(tmpWAP1.Distance, 2) - Math.Pow(tmpWAP3.Distance, 2) - Math.Pow(MyLat, 2)
				+ Math.Pow(MyLat - tmpWAP3.X, 2) + Math.Pow(tmpWAP3.Y, 2)) / (2 * tmpWAP3.Y);

			var MyLocation = myRotation(MyLat, MyLong, 0, -deg);

			MyPosition.X = (long)(MyLocation[0] + beacon1.X);
			MyPosition.Y = (long)(MyLocation[1] + beacon1.Y);

			return MyPosition;
		}
	}
}

