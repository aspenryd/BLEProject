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
using System.Threading.Tasks;
using BluetoothLEExplorer.Droid.UI.Controls;
using Android.Bluetooth;
using BluetoothLEExplorer.Droid.UI.Adapters;
using iBeaconImg;
using Triangulation;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Graphics.Drawables.Shapes;

namespace BluetoothLEExplorer.Droid.Screens.Scanner.Home
{
	[Activity (Label = "ScannerHome", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]			
	public class ScannerHome : Activity
	{
		protected ListView _listView;
		protected ScanButton _scanButton;
		protected DevicesAdapter _listAdapter;
		protected ProgressDialog _progress;
		protected BLEDeviceInfo _deviceToConnect; //not using State.SelectedDevice because it may not be connected yet

		// röd
		int b1X = 40;
		int b1Y = 450;
		//string b1N = "e2c56db5-dffb-48d2-b060-d0f5a71096e2";
		// grön
		int b2X = 200;
		int b2Y = 100;
		//string b2N = "e2c56db5-dffb-48d2-b060-d0f5a71096e3";
		// gul
		int b3X = 470;
		int b3Y = 300;
		//string b3N = "e2c56db5-dffb-48d2-b060-d0f5a71096e1";

		// external handlers
		EventHandler<BluetoothLEManager.DeviceDiscoveredEventArgs> deviceDiscoveredHandler;
		EventHandler<BluetoothLEManager.DeviceConnectionEventArgs> deviceConnectedHandler;
		EventHandler deviceScanTimeoutHandler;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// load our layout
			SetContentView (Resource.Layout.ScannerHome);

			// find our controls
			this._listView = FindViewById<ListView> (Resource.Id.DevicesTable);
			this._scanButton = FindViewById<ScanButton> (Resource.Id.ScanButton);

			// create our list adapter
			this._listAdapter = new DevicesAdapter(this, BluetoothLEManager.Current.DiscoveredDevices);
			this._listView.Adapter = this._listAdapter;

			Button button = FindViewById<Button> (Resource.Id.myButton);
			button.Click += delegate {

				ImageView dot = FindViewById<ImageView> (Resource.Id.dotID);
				dot.SetImageResource (Resource.Drawable.positionCursor);
				var dotpos = GetPositionInGrid((int) _listAdapter.B, (int) _listAdapter.A, (int) _listAdapter.C);

//				dot.SetX(238);
//				dot.SetY(129);
				dot.SetX(unchecked((int)dotpos.X));
				dot.SetY(unchecked((int)dotpos.Y));



			};

			Button button2 = FindViewById<Button> (Resource.Id.BaconButton);
			button2.Click += delegate {
				ImageView dotb1 = FindViewById<ImageView> (Resource.Id.b1);
				ImageView dotb2 = FindViewById<ImageView> (Resource.Id.b2);
				ImageView dotb3 = FindViewById<ImageView> (Resource.Id.b3);

				// Röd
				dotb1.SetX(b1X);
				dotb1.SetY(b1Y);
				// Grön
				dotb2.SetX(b2X);
				dotb2.SetY(b2Y);
				// Gul
				dotb3.SetX(b3X);
				dotb3.SetY(b3Y);

			};
		}



		public Position GetPositionInGrid(int sA, int sB, int sC)
		{
			ImageView dotb1 = FindViewById<ImageView> (Resource.Id.b1);
			ImageView dotb2 = FindViewById<ImageView> (Resource.Id.b2);
			ImageView dotb3 = FindViewById<ImageView> (Resource.Id.b3);

			var b1 = new iBeacon(b1X, b1Y, null, sA);
			var b2 = new iBeacon(b2X, b2Y, null, sB);
			var b3 = new iBeacon(b3X, b3Y, null, sC);
			b1.RSSI = _listAdapter.one;
			b2.RSSI = _listAdapter.two;
			b3.RSSI = _listAdapter.three;

			dotb1.LayoutParameters.Width = (int)b1.Distance * 20; 
			dotb2.LayoutParameters.Width = (int)b2.Distance * 20;
			dotb3.LayoutParameters.Width = (int)b3.Distance * 20;

			dotb1.LayoutParameters.Height = (int)b1.Distance * 20; 
			dotb2.LayoutParameters.Height = (int)b2.Distance * 20;
			dotb3.LayoutParameters.Height = (int)b3.Distance * 20;

			return Trilateration.Trilaterate(b1, b2, b3);    

		}
		
		protected override void OnResume ()
		{
			base.OnResume ();
			
			this.WireupLocalHandlers ();
			this.WireupExternalHandlers ();
		}
		
		protected override void OnPause ()
		{
			base.OnPause ();

			// stop our scanning (does a check, and also runs async)
			this.StopScanning ();
			
			// unwire external event handlers (memory leaks)
			this.RemoveExternalHandlers ();
		}

		protected void WireupLocalHandlers ()
		{
			this._scanButton.Click += (object sender, EventArgs e) => {
				if ( !BluetoothLEManager.Current.IsScanning ) {
					BluetoothLEManager.Current.BeginScanningForDevices ();
				} else {
					BluetoothLEManager.Current.StopScanningForDevices ();
				}
			};

//			this._listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
//				Console.Write ("ItemClick: " + this._listAdapter.Items[e.Position]);
//
//				// stop scanning
//				this.StopScanning();
//
//				// select the item
//				this._listView.ClearFocus();
//				this._listView.Post( () => {
//					this._listView.SetSelection (e.Position);
//				});
//				//this._listView.SetItemChecked (e.Position, true);
//				// todo: for some reason, we're losing the selection, so i have to cache it
//				// think i know the issue, see the note in the GenericObjectAdapter class
//				this._deviceToConnect = this._listAdapter.Items[e.Position];
//
//
//
//				// show a connecting overlay
//				// TODO: make this conform to lifecycle, see: https://github.com/xamarin/private-samples/blob/master/EvolveCurriculum/Advanced/10%20-%20Advanced%20Android%20Application%20Lifecycle/ActivityLifecycle/MainActivity.cs
//				this.RunOnUiThread( () => {	
//					//TODO: we need to save a ref to the device when click
//					this._progress = ProgressDialog.Show(this, "Connecting", "Connecting to " + this._deviceToConnect.Device.Name, true);
//				});
//
//				// try and connect
//				BluetoothLEManager.Current.ConnectToDevice ( this._listAdapter[e.Position].Device );
//
//			};
		}

		protected void WireupExternalHandlers ()
		{
			this.deviceDiscoveredHandler = (object sender, BluetoothLEManager.DeviceDiscoveredEventArgs e) => {
				Console.WriteLine ("Discovered device: " + e.Device.Name);

				// reload the list view
				//TODO: why doens't NotifyDataSetChanged work? is it because i'm replacing the reference?
				this.RunOnUiThread( () => {
					this._listAdapter = new DevicesAdapter(this, BluetoothLEManager.Current.DiscoveredDevices);
					this._listView.Adapter = this._listAdapter;
				});
			};
			BluetoothLEManager.Current.DeviceDiscovered += this.deviceDiscoveredHandler;

			this.deviceConnectedHandler = (object sender, BluetoothLEManager.DeviceConnectionEventArgs e) => {
				this.RunOnUiThread( () => {
					this._progress.Hide();
				});
				// now that we're connected, save it
				App.Current.State.SelectedDevice = e.Device;

				// launch the details screen
				this.StartActivity (typeof(DeviceDetails.DeviceDetailsScreen));
			};
			BluetoothLEManager.Current.DeviceConnected += this.deviceConnectedHandler;


			this.deviceScanTimeoutHandler = (object sender, EventArgs e) => {
				this.RunOnUiThread( () => {
					this._scanButton.SetState (ScanButton.ScanButtonState.Normal);
				});
			};
			BluetoothLEManager.Current.ScanTimeoutElapsed += this.deviceScanTimeoutHandler;
		}

		protected void RemoveExternalHandlers()
		{
			BluetoothLEManager.Current.DeviceDiscovered -= this.deviceDiscoveredHandler;
			BluetoothLEManager.Current.DeviceConnected -= this.deviceConnectedHandler;
		}

		protected void StopScanning()
		{
			// stop scanning
			new Task( () => {
				if(BluetoothLEManager.Current.IsScanning) {
					Console.WriteLine ("Still scanning, stopping the scan and reseting the right button");
					BluetoothLEManager.Current.StopScanningForDevices();
					this._scanButton.SetState (ScanButton.ScanButtonState.Normal);
				}
			}).Start();
		}
	}
}

