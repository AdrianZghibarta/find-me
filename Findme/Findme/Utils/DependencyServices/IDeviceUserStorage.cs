using System;

namespace Findme
{
	public interface IDeviceUserStorage
	{
		void saveString(String stringToSave, String key);
		String getStringForKey(String key);
	}
}

