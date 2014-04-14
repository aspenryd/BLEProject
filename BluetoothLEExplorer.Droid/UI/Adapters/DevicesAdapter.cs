using System;
using System.Collections.Generic;
using Android.App;
using Android.Bluetooth;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace BluetoothLEExplorer.Droid.UI.Adapters
{
	public class DevicesAdapter : GenericAdapterBase<BLEDeviceInfo>
	{
		public DevicesAdapter (Activity context, IList<BLEDeviceInfo> items) 
			: base(context, Android.Resource.Layout.SimpleListItem2, items)
		{

		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate (resource, null);

			view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = GetDeviceInfo(items[position]);
			view.FindViewById<TextView> (Android.Resource.Id.Text2).Text = "Address: " + items [position].Device.Address;

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
			return string.Format("UUID:{0}\nMajor:{1}, Minor:{2}, Power:{3}", uuid , HexValueToString(major), HexValueToString(minor), power);
		}

		string HexValueToString (string value)
		{
			int decval = int.Parse(value, System.Globalization.NumberStyles.HexNumber);
			return decval.ToString();
		}


		string GetDeviceInfo (BLEDeviceInfo btd)
		{
			var result = "Name: " + btd.Device.Name;
			if (btd.Device.Handle != null && !string.IsNullOrWhiteSpace(btd.Device.Handle.ToString()))
				result = result + " (" + btd.Device.Handle.ToString() + ")";
			result = result + "\n" + scanRecordToString (btd.ScanRecord);
			result = result + "\nRSSI: " + btd.RSSI.ToString();
			result = result + " Distance: " + iBeaconHelper.calculateAccuracy(221, btd.RSSI).ToString("0.0 m");

			return result;
		}
	}
}

