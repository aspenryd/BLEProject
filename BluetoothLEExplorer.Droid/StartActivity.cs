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
using BluetoothLEExplorer.Droid.Screens.Scanner.Home;
using Android.Content.PM;

namespace BluetoothLEExplorer.Droid
{
	[Activity (Label = "StartActivity", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]			
	public class StartActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			ActionBar.Hide(); 

			SetContentView (Resource.Layout.Start);

			Spinner sp = FindViewById<Spinner> (Resource.Id.locationSpinner);
			var items = new List<string>() {"Spree Office", "Åhlens City", "Vasamuseet"};
			items.Sort((x, y) => string.Compare(x, y));

			var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);
			sp.Adapter = adapter;


			Button startButton = FindViewById<Button> (Resource.Id.activateBt);
			startButton.Click += delegate {
				if(sp.SelectedItem.Equals("Spree Office")){
					SetContentView(Resource.Layout.ScannerHome);
					StartActivity(typeof(ScannerHome));
				}
				else{
					Toast.MakeText (this, "No maps found. \nPlease choose a different building.", ToastLength.Short).Show();
				}

			};
		}
	}
}

