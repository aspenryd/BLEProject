package bluetoothleexplorer.droid;


public class BluetoothLEManager_GattCallback
	extends android.bluetooth.BluetoothGattCallback
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onConnectionStateChange:(Landroid/bluetooth/BluetoothGatt;II)V:GetOnConnectionStateChange_Landroid_bluetooth_BluetoothGatt_IIHandler\n" +
			"n_onServicesDiscovered:(Landroid/bluetooth/BluetoothGatt;I)V:GetOnServicesDiscovered_Landroid_bluetooth_BluetoothGatt_IHandler\n" +
			"";
		mono.android.Runtime.register ("BluetoothLEExplorer.Droid.BluetoothLEManager/GattCallback, Xamarin.Robotics.BluetoothLEExplorer.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BluetoothLEManager_GattCallback.class, __md_methods);
	}


	public BluetoothLEManager_GattCallback () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BluetoothLEManager_GattCallback.class)
			mono.android.TypeManager.Activate ("BluetoothLEExplorer.Droid.BluetoothLEManager/GattCallback, Xamarin.Robotics.BluetoothLEExplorer.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public BluetoothLEManager_GattCallback (bluetoothleexplorer.droid.BluetoothLEManager p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == BluetoothLEManager_GattCallback.class)
			mono.android.TypeManager.Activate ("BluetoothLEExplorer.Droid.BluetoothLEManager/GattCallback, Xamarin.Robotics.BluetoothLEExplorer.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "BluetoothLEExplorer.Droid.BluetoothLEManager, Xamarin.Robotics.BluetoothLEExplorer.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onConnectionStateChange (android.bluetooth.BluetoothGatt p0, int p1, int p2)
	{
		n_onConnectionStateChange (p0, p1, p2);
	}

	private native void n_onConnectionStateChange (android.bluetooth.BluetoothGatt p0, int p1, int p2);


	public void onServicesDiscovered (android.bluetooth.BluetoothGatt p0, int p1)
	{
		n_onServicesDiscovered (p0, p1);
	}

	private native void n_onServicesDiscovered (android.bluetooth.BluetoothGatt p0, int p1);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
