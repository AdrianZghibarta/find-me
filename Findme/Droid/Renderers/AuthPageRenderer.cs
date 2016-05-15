using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Android.App;
using Android.Widget;
using Android.OS;
using Findme;
using Findme.Droid;
using Xamarin.Auth;

[assembly: ExportRenderer(typeof(AuthPage), typeof(AuthPageRenderer))]

namespace Findme.Droid
{
	public class AuthPageRenderer: PageRenderer
	{
		public AuthPageRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null || Element == null) {
				return;
			}

			try {
				// - Implement the customisation here
				Console.WriteLine("Hello from android App");

				AuthPage authPage = (AuthPage)this.Element;

				// - Setup the facebook auth screen
				authPage.showFacebookAuth = () => {
					Console.WriteLine("This is Facebook Auth Screen");

					var auth = new OAuth2Authenticator (
						clientId: NetworkingConstants.FACEBOOK_APP_ID,
						scope: "",
						authorizeUrl: new Uri (NetworkingConstants.FACEBOOK_AUTH_URL),
						redirectUrl: new Uri (NetworkingConstants.FACEBOOK_REDIRECT_URL));

					auth.AllowCancel = true;

					// - If authorization succeeds or is canceled, .Completed will be fired.
					auth.Completed += (sender, eventArgs) =>
					{
						if (!eventArgs.IsAuthenticated) {;
							return;
						}

						// - And call the handler for acces token
						authPage.didGetFacebookAccessToken(eventArgs.Account.Properties["access_token"]);
					};

					var intent = auth.GetUI (MainActivity.sharedInstance);
					MainActivity.sharedInstance.StartActivity (intent);
				};

				// - Setup the google auth screen
				authPage.showGoogleAuth = () => {
					Console.WriteLine("This is Google Auth Screen");	
				};
			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine (@"          ERROR: ", ex.Message);
			}
		}
	}
}

