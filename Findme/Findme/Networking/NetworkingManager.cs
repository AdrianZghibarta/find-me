using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Collections.Generic;
using System.ServiceModel;
using System.Net;

namespace Findme
{
	public class FindMeResponse {
		public Object Result;
		public String ErrorInfo;

		public FindMeResponse() 
		{
			Result = null;
			ErrorInfo = null;
		}
	}

	public class NetworkingManager
	{
		protected HttpClient client;
		protected HttpClient authClient;

		public NetworkingManager ()
		{
			client = new HttpClient ();
			//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			authClient = new HttpClient ();
			authClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}


		/// <summary>
		/// Gets the find me response from dictionary for a given string key.
		/// </summary>
		/// <returns>The find me response from dictionary.</returns>
		/// <param name="dictionary">Dictionary.</param>
		/// <param name="key">Key.</param>
		static public async Task<FindMeResponse> getFindMeResponseFromHttpResponseMessage(
			HttpResponseMessage responseMessage, 
			String key) {

			FindMeResponse response = new FindMeResponse ();

			// - Get the json string result
			var stringResult = await responseMessage.Content.ReadAsStringAsync();
			// - Convert the json String in Dictonary 
			Dictionary<String, dynamic> bodyDictionary = 
				JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(stringResult);

			// - Check the succes status in the response
			var status = (bool)bodyDictionary ["success"];

			if (status) {

				// - If the status is succes then try getting the result for the given key
				var result = bodyDictionary [key];

				if (null != result) {

					// - If there is a value for the key thant we need return this as json String
					response.Result = result.ToString();
				} else {

					// - If the value that we need is null than send an error info message
					response.ErrorInfo = "No object found for your request, please try later.";
				}
			} else {

				// - If the succes status is false then try getting the error message
				var errorMessage = bodyDictionary ["message"];

				if (null != errorMessage) {

					// - If there is an error message then send this as error response
					response.ErrorInfo = errorMessage;
				} else {

					// - If there is no error messages then send an Unknown error message
					response.ErrorInfo = "Unknown error, please try later.";
				}
			}

			// - Return the response message that we need
			return response;
		}
	}
}

