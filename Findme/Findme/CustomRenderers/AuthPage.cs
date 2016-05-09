using System;
using Xamarin.Forms;

namespace Findme
{
	public delegate void ShowFacebookAuthDelegate();
	public delegate void DidGetFacebookAccessToken(String accessToken);

	public delegate void ShowGoogleAuthDelegate();
	public delegate void DidGetGoogleAccesToken(String accesToken);

	public class AuthPage: ContentPage
	{
		// - The delegates to show facebook and google auth screens
		public ShowFacebookAuthDelegate showFacebookAuth;
		public ShowGoogleAuthDelegate showGoogleAuth;

		// - The completition handlers when the acces token is received
		public DidGetFacebookAccessToken didGetFacebookAccessToken;
		public DidGetGoogleAccesToken didGetGoogleAccesToken;

		public AuthPage ()
		{
		}
	}
}

