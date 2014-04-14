package bluetoothleexplorer.droid;


public abstract class GenericAdapterBase_1
	extends android.widget.BaseAdapter
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_getItemId:(I)J:GetGetItemId_IHandler\n" +
			"n_getItem:(I)Ljava/lang/Object;:GetGetItem_IHandler\n" +
			"n_getCount:()I:GetGetCountHandler\n" +
			"";
		mono.android.Runtime.register ("BluetoothLEExplorer.Droid.GenericAdapterBase`1, Xamarin.Robotics.BluetoothLEExplorer.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GenericAdapterBase_1.class, __md_methods);
	}


	public GenericAdapterBase_1 () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GenericAdapterBase_1.class)
			mono.android.TypeManager.Activate ("BluetoothLEExplorer.Droid.GenericAdapterBase`1, Xamarin.Robotics.BluetoothLEExplorer.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public long getItemId (int p0)
	{
		return n_getItemId (p0);
	}

	private native long n_getItemId (int p0);


	public java.lang.Object getItem (int p0)
	{
		return n_getItem (p0);
	}

	private native java.lang.Object n_getItem (int p0);


	public int getCount ()
	{
		return n_getCount ();
	}

	private native int n_getCount ();

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
