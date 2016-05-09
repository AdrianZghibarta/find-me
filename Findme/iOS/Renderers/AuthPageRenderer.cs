using System;
using Findme;
using Findme.iOS;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Auth;

#if __UNIFIED__
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

[assembly: ExportRenderer(typeof(AuthPage), typeof(AuthPageRenderer))]

namespace Findme.iOS
{
	public class AuthPageRenderer: PageRenderer
	{
		public AuthPageRenderer ()
		{
		}

		protected override void OnElementChanged (VisualElementChangedEventArgs e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null || Element == null) {
				return;
			}

			try {
				// - Implement the customisation here
				Console.WriteLine("Hello from iOS app");

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
						// - We presented the UI, so it's up to us to dismiss it.
						this.DismissViewController(true, null);

						// - And call the handler for acces token
						authPage.didGetFacebookAccessToken(eventArgs.Account.Properties["access_token"]);
					};
					this.PresentViewController(auth.GetUI(), true, null);
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

