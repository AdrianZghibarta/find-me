using System;

namespace Findme
{
	public class NetworkingUrls
	{

		// The base urls for the application
		public static String BASE_URL = "http://yourstuff.herokuapp.com/";
		public static String API_VERSION_ONE = "api/v1/";
		public static String VERSION_ONE_ROOT = NetworkingUrls.BASE_URL + API_VERSION_ONE;

		// URls for authentification requests
		public static readonly String REGISTER_URL = NetworkingUrls.VERSION_ONE_ROOT + "user/signup";
		public static readonly String LOGIN_URL = NetworkingUrls.VERSION_ONE_ROOT + "user/login";
		public static readonly String FACEBOOK_LOGIN_URL = NetworkingUrls.VERSION_ONE_ROOT + "user/facebook-login";
		public static readonly String REFRESH_TOKEN_URL = NetworkingUrls.VERSION_ONE_ROOT + "user/refresh-token";

		public NetworkingUrls () { }
	}

	public class NetworkingConstants
	{

		public static readonly String FACEBOOK_APP_ID = "1009438632474487";
		public static readonly String FACEBOOK_REDIRECT_URL = "http://yourstuff.herokuapp.com";
		public static readonly String FACEBOOK_AUTH_URL = "https://m.facebook.com/dialog/oauth/";
	}
}

