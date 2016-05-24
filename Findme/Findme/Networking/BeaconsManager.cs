using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace Findme
{
	public class BeaconsManager: NetworkingManager
	{
		public static BeaconsManager singletonInstance;
		public static String AvailableBeaconType = "available";
		public static String BussyBeaconType = "bussy";

		/// <summary>
		/// Gets the shared instance.
		/// </summary>
		/// <value>The shared instance.</value>
		public static BeaconsManager SharedInstance
		{
			get 
			{
				if (singletonInstance == null)
				{
					singletonInstance = new BeaconsManager ();
				}
				return singletonInstance;
			}
		}

		public BeaconsManager ()
		{
		}

		/// <summary>
		/// Gets the user beacons.
		/// </summary>
		/// <returns>The user beacons.</returns>
		/// <param name="beaconsType">Beacons type.</param>
		public async Task<FindMeResponse> GetUserBeacons(String beaconsType = null)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var requestUrl = NetworkingUrls.GET_USER_BEACONS_URL + "?token=" + UserStorage.GetAccessToken();
				if (beaconsType != null) {
					requestUrl += "&type=" + beaconsType;
				}
				var result = await client.GetAsync(requestUrl);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "beacons");

				List<FMBeacon> beaconList = JsonConvert.DeserializeObject<List<FMBeacon>>((String)findMeResponse.Result);
				if (null == beaconList) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = beaconList;
				}
			}
			catch (Exception ex)
			{
				ConsoleOutput.PrintLine ("Error message : " + ex.Message);
				findMeResponse.ErrorInfo = ex.Message;
			}

			return findMeResponse;
		}

		/// <summary>
		/// Deletes the beacon.
		/// </summary>
		/// <returns>Response</returns>
		/// <param name="beaconId">Beacon identifier.</param>
		public async Task<FindMeResponse> DeleteBeacon(String beaconId)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var keyValues = new List<KeyValuePair<string, string>>();
				keyValues.Add(new KeyValuePair<string, string>("token", UserStorage.GetAccessToken()));
				keyValues.Add(new KeyValuePair<string, string>("id", beaconId));

				var parameters = new FormUrlEncodedContent(keyValues);

				var result = await client.PostAsync(NetworkingUrls.DELETE_BEACON, parameters);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "message");

				String succesMessage = (String)findMeResponse.Result;
				if (null == succesMessage) {
					findMeResponse.ErrorInfo = "An error has occured, please try later.";
				}
				else {
					findMeResponse.Result = succesMessage;
				}
			}
			catch (Exception ex)
			{
				ConsoleOutput.PrintLine ("Error message : " + ex.Message);
				findMeResponse.ErrorInfo = ex.Message;
			}

			return findMeResponse;
		}
	}
}

