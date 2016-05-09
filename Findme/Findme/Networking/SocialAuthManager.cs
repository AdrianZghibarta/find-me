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
	public class SocialAuthManager: NetworkingManager
	{
		public static SocialAuthManager singletonInstance;

		/// <summary>
		/// Gets the shared instance.
		/// </summary>
		/// <value>The shared instance.</value>
		public static SocialAuthManager sharedInstance
		{
			get 
			{
				if (singletonInstance == null)
				{
					singletonInstance = new SocialAuthManager ();
				}
				return singletonInstance;
			}
		}

		public SocialAuthManager ()
		{
		}

		/// <summary>
		/// Gets the facebook social user with auth token.
		/// </summary>
		/// <returns>The facebook social user</returns>
		/// <param name="accessToken">Access token.</param>
		public async Task<SocialUser> getFacebookSocialUserWithAuthToken(String accessToken)
		{
			var response = await this.client.GetAsync ("https://graph.facebook.com/me?access_token=" + accessToken);
			var result = await response.Content.ReadAsStringAsync ();
			ConsoleOutput.PrintLine (result);

			SocialUser user = JsonConvert.DeserializeObject<SocialUser> (result);
			user.accessToken = accessToken;

			return user;
		}
	}
}

