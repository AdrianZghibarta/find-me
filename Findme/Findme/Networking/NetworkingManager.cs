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
	public class NetworkingManager
	{
		private static NetworkingManager singletonInstance;
		protected HttpClient client;

		/// <summary>
		/// Gets the shared instance.
		/// </summary>
		/// <value>The shared instance.</value>
		public static NetworkingManager sharedInstance
		{
			get 
			{
				if (singletonInstance == null)
				{
					singletonInstance = new NetworkingManager ();
				}
				return singletonInstance;
			}
		}


		public NetworkingManager ()
		{
			client = new HttpClient ();
		}

		public async Task<String> googleRequest() {

			var response = await this.client.GetAsync ("http://www.google.com");

			var result = await response.Content.ReadAsStringAsync ();
			return result;
		}
	}
}

