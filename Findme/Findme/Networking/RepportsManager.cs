using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace Findme
{
	public class RepportsManager: NetworkingManager
	{
		public static RepportsManager singletonInstance;

		/// <summary>
		/// Gets the shared instance.
		/// </summary>
		/// <value>The shared instance.</value>
		public static RepportsManager SharedInstance
		{
			get 
			{
				if (singletonInstance == null)
				{
					singletonInstance = new RepportsManager ();
				}
				return singletonInstance;
			}
		}

		public RepportsManager ()
		{
		}

		/// <summary>
		/// Gets the repports.
		/// </summary>
		/// <returns>The repports.</returns>
		/// <param name="itemId">Item identifier.</param>
		public async Task<FindMeResponse> GetRepports(String itemId)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var result = await client.GetAsync(NetworkingUrls.GET_REPPORTS_URL + "?token=" + UserStorage.GetAccessToken() + "&itemId=" + itemId);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "reports");

				List<Repport> repportsList = JsonConvert.DeserializeObject<List<Repport>>((String)findMeResponse.Result);
				if (null == repportsList) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = repportsList;
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
		/// Creates the repport.
		/// </summary>
		/// <returns>The repport.</returns>
		/// <param name="itemId">Item identifier.</param>
		/// <param name="longitude">Longitude.</param>
		/// <param name="latitude">Latitude.</param>
		public async Task<FindMeResponse> CreateRepport(String itemId, String longitude, String latitude)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var keyValues = new List<KeyValuePair<string, string>>();
				keyValues.Add(new KeyValuePair<string, string>("token", UserStorage.GetAccessToken()));
				keyValues.Add(new KeyValuePair<string, string>("itemId", itemId));
				keyValues.Add(new KeyValuePair<string, string>("longitude", longitude));
				keyValues.Add(new KeyValuePair<string, string>("latitude", latitude));

				var parameters = new FormUrlEncodedContent(keyValues);

				var result = await client.PostAsync(NetworkingUrls.ADD_REPPORT_URL, parameters);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "message");

				String message = (String)findMeResponse.Result;
				if (null == message) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = message;
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

