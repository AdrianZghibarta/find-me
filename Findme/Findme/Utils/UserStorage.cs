using System;
using Xamarin.Forms;

namespace Findme
{
	public class UserStorage
	{
		// - All the keys for the storage
		static readonly String accessTokenKey = "access_token";
		static readonly String refreshTokenKey = "refresh_token";

		public UserStorage ()
		{
		}

		#region -> General methods

		/// <summary>
		/// Saves the string.
		/// </summary>
		/// <param name="stringToSave">String to save.</param>
		/// <param name="key">Key.</param>
		static public void SaveString(String stringToSave, String key) {
			DependencyService.Get<IDeviceUserStorage> ().saveString (stringToSave: stringToSave, key: key);
		}

		/// <summary>
		/// Gets the string.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="key">Key.</param>
		static public String GetString(String key) {
			return DependencyService.Get<IDeviceUserStorage> ().getStringForKey (key);
		}

		#endregion

		#region -> Access Tokens Methods

		/// <summary>
		/// Saves the access and refresh token.
		/// </summary>
		/// <param name="accessToken">Access token.</param>
		/// <param name="refreshToken">Refresh token.</param>
		static public void SaveAccessAndRefreshToken(String accessToken, String refreshToken) {

			SaveString (accessToken, accessTokenKey);
			SaveString (refreshToken, refreshTokenKey);
		}

		static public void DeleteAccessAndRefreshToken() {

			SaveString (null, accessTokenKey);
			SaveString (null, refreshTokenKey);
		}

		/// <summary>
		/// Gets the access token.
		/// </summary>
		/// <returns>The access token.</returns>
		static public String GetAccessToken() {

			return GetString (accessTokenKey);
		}

		/// <summary>
		/// Gets the refresh token.
		/// </summary>
		/// <returns>The refresh token.</returns>
		static public String GetRefreshToken() {

			return GetString (refreshTokenKey);
		}

		#endregion
	}
}

