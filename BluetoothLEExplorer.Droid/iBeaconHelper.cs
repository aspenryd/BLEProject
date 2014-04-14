using System;

namespace BluetoothLEExplorer.Droid
{
	public class iBeaconHelper
	{
		public iBeaconHelper ()
		{

		}

		public static double calculateAccuracy(int txPower, double rssi)
		{
			if (rssi == 0)
			{
				return -1.0; // if we cannot determine accuracy, return -1.
			}

			double ratio = rssi * 1.0 / txPower;
			return Math.Pow(ratio, 10)*1000000;
		}   


	}
}

