using System;

namespace Findme
{
	public class NetworkingUrls
	{

		// The base urls for the application
		static String BASE_URL = "http://www.google.com/";
		static String API_VERSION_ONE = "api/v1/";
		static String VERSION_ONE_ROOT = NetworkingUrls.BASE_URL + API_VERSION_ONE;

		// URls for request
		static readonly String REGISTER_URL = NetworkingUrls.VERSION_ONE_ROOT + "registration";
		static readonly String LOGIN_URL = NetworkingUrls.VERSION_ONE_ROOT + "login";

		public NetworkingUrls () { }
	}
}

