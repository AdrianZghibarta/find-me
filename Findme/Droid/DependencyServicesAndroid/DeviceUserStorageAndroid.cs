using System;
using Findme;
using Android.App;
using Android.Content;
using Findme.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceUserStorageAndroid))]
namespace Findme.Droid
{
	public class DeviceUserStorageAndroid: IDeviceUserStorage
	{
		public DeviceUserStorageAndroid ()
		{
		}

		public void saveString(String stringToSave, String key) {
			var prefs = Application.Context.GetSharedPreferences("FindMePref", FileCreationMode.Private);
			var prefsEditor = prefs.Edit();
			prefsEditor.PutString(stringToSave, key);
			prefsEditor.Commit();
		}
		public String getStringForKey(String key) {
			var prefs = Application.Context.GetSharedPreferences("FindMePref", FileCreationMode.Private);
			string token = prefs.GetString(key, null);
			return token;
		}
	}
}

