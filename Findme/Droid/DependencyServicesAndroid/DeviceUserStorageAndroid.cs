using System;
using Findme;
using Android.App;
using Android.Content;
using Findme.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceUserStorageAndroid))]
namespace Findme.Droid
{
	public class DeviceUserStorageAndroid: IDeviceUserStorage
	{
		public DeviceUserStorageAndroid ()
		{
		}

		public void saveString(String stringToSave, String key) {
			var prefs = Forms.Context.GetSharedPreferences("FindMePref", FileCreationMode.Private);
			var prefsEditor = prefs.Edit();
			prefsEditor.PutString(key, stringToSave);
			prefsEditor.Commit ();
		}
		public string getStringForKey(String key) {
			var prefs = Forms.Context.GetSharedPreferences("FindMePref", FileCreationMode.Private);
			String stringToReturn = prefs.GetString (key, null);
			return stringToReturn;
		}
	}
}

