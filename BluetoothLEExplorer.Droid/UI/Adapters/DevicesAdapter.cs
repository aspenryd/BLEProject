using System;
using System.Collections.Generic;
using Android.App;
using Android.Bluetooth;
using Android.Views;
using Android.Widget;
using Android.Content;
using BluetoothLEExplorer.Droid.Screens.Scanner.Home;
using System.Threading.Tasks;
using System.Linq;

namespace BluetoothLEExplorer.Droid.UI.Adapters
{
	public class DevicesAdapter : GenericAdapterBase<BLEDeviceInfo>
	{

		public double A;
		public double B;
		public double C;

		public string beacon1;
		public string beacon2;
		public string beacon3;

		public double one;
		public double two;
		public double three;

		public DevicesAdapter (Activity context, IList<BLEDeviceInfo> items) 
			: base(context, Android.Resource.Layout.SimpleListItem2, items)
		{

		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate (resource, null);

			items = items.OrderByDescending(o => o.RSSI * -1).ToList();
			view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = GetDeviceInfo(items[position]);

			try{
//				if(items[1].Device.Address.EndsWith("71")){
					string t1 = DeviceInfo2 (items [1]);
					one = Convert.ToDouble(t1);
					string test1 = DeviceInfo (items [1]).ToString ();
					A = Convert.ToDouble (test1);
//				}
//				if(items[2].Device.Address.EndsWith("C1")){
					string t2 = DeviceInfo2 (items [2]).ToString ();
					two = Convert.ToDouble(t2);
					string test2 = DeviceInfo (items [2]).ToString ();
					B = Convert.ToDouble (test2);
//				}
//				if(items[0].Device.Address.EndsWith("A5")){
					string t3 = DeviceInfo2 (items [0]).ToString ();
					three = Convert.ToDouble(t3);
					string test3 = DeviceInfo (items [0]).ToString ();
					C = Convert.ToDouble (test3);
//				}
			}
			catch{}

			return view;
		}

		string ByteToHex (byte item)
		{
			return string.Format ("{0:x2}", item);
		}

		string scanRecordToString (byte[] scanRecord)
		{
			var adress = "";
			var pduheader = "";
			var macadress = "";
			var unknown = "";
			var uuid = "";
			var major = "";
			var minor = "";
			var power = "";
			var checksum = "";
			var octet = 0;
			if (scanRecord.Length < 30) {
				foreach (byte item in scanRecord) {
					unknown += ByteToHex (item);
				}
				return "Unknown format: " + unknown;
			}
			foreach (byte item in scanRecord) 
			{
				if (octet < 9)
					unknown += ByteToHex (item);
				else if (octet < 9 + 16)
					uuid += ByteToHex (item);
				else if (octet < 9 + 16 + 2)
					major += ByteToHex (item);
				else if (octet < 9+16+2+2)
					minor+= ByteToHex (item);
				else if (octet < 9+16+2+2+1)
					power+= ByteToHex(item);
				octet++;
			}
			//return string.Format("UUID:{0}\nMajor:{1}, Minor:{2}, Power:{3}", uuid , HexValueToString(major), HexValueToString(minor), power);
			if (uuid.EndsWith ("1")) {
				beacon1 = "iBeacon1: "; 
				return beacon1;
			}
			if (uuid.EndsWith ("2")) {
				beacon2 = "iBeacon2: "; 
				return beacon2;
			}
			if (uuid.EndsWith ("3")) {
				beacon3 = "iBeacon3: "; 
				return beacon3;
			} else {
				return string.Format("UUID:{0}", uuid);
			}
		}

		string HexValueToString (string value)
		{
			int decval = int.Parse(value, System.Globalization.NumberStyles.HexNumber);
			return decval.ToString();
		}


		public string GetDeviceInfo (BLEDeviceInfo btd)
		{
			//var result = "Name: " + btd.Device.Name;
			var result = btd.Device.Address;
			if (btd.Device.Handle != null && !string.IsNullOrWhiteSpace(btd.Device.Handle.ToString()))
				//result = result + " (" + btd.Device.Handle.ToString() + ")";
				result = result + scanRecordToString (btd.ScanRecord);
			//result = result + "RSSI: " + btd.RSSI.ToString();
			result = result + iBeaconHelper.calculateAccuracy(221, btd.RSSI).ToString("0.0");
			result = result + "\nRSSI: " + btd.RSSI.ToString();

			return result; 
		}
		public string DeviceInfo (BLEDeviceInfo btd)
		{
			var result = iBeaconHelper.calculateAccuracy (221, btd.RSSI).ToString ();
			return result;
		}
		public string DeviceInfo2 (BLEDeviceInfo btd)
		{
			var result = btd.RSSI.ToString();
			return result;
		}
	}
}

