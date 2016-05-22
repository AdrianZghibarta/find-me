using System;
using Foundation;
using Findme;
using Findme.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceUserStorageiOS))]
namespace Findme.iOS
{
	public class DeviceUserStorageiOS: IDeviceUserStorage
	{
		public DeviceUserStorageiOS ()
		{
		}

		public void saveString(String stringToSave, String key) {
			NSUserDefaults.StandardUserDefaults.SetString(stringToSave, key);
		}
		public string getStringForKey(String key) {
			return NSUserDefaults.StandardUserDefaults.StringForKey(key);
		}
	}
}

