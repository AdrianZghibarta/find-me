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
	public class AuthentificationManager: NetworkingManager
	{

		public static AuthentificationManager singletonInstance;

		/// <summary>
		/// Gets the shared instance.
		/// </summary>
		/// <value>The shared instance.</value>
		public static AuthentificationManager sharedInstance
		{
			get 
			{
				if (singletonInstance == null)
				{
					singletonInstance = new AuthentificationManager ();
				}
				return singletonInstance;
			}
		}

		public AuthentificationManager ()
		{
		}

		public async Task<User> authentificateUserWithFacebookToken(String facebookToken)
		{
			SocialUser socialUser = await SocialAuthManager.sharedInstance.getFacebookSocialUserWithAuthToken(facebookToken);

			// - Implement the request to the Find Me Web Services
			return new User();
		}
	}
}

