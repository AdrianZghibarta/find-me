using System;
using Xamarin.Forms;

namespace Findme
{
	public class IntroPage: ContentPage
	{
		public IntroPage ()
		{
			this.customizeLayout ();
		}

		override protected void OnAppearing() {

			TryToRefreshTheToken ();
		}

		private void TryToRefreshTheToken() {

			String refreshToken = UserStorage.GetRefreshToken ();
			if (null != refreshToken) {

				AuthentificationManager.SharedInstance.RefreshToken (refreshTokenValue: refreshToken).ContinueWith ( task => {

					FindMeResponse response = (FindMeResponse)task.Result;
					if (null != response.ErrorInfo) {
						ConsoleOutput.PrintLine(response.ErrorInfo);
						Device.BeginInvokeOnMainThread( () => {
							Navigation.PushModalAsync(new AuthentificationPage());
						});
					}
					else {// - Success
						Device.BeginInvokeOnMainThread( () => {
							Xamarin.Forms.Application.Current.MainPage = new RootPage ();
						});
					}
				});
			} 
			else {
				Navigation.PushModalAsync(new AuthentificationPage());
			}
		}

		private void customizeLayout() {

			var backgroundImage = new Image() {
				Source = new FileImageSource() {
					File = ""//blurredBackground.jpg
				},
				BackgroundColor = ColorMap.GreenColor,
				Aspect = Aspect.AspectFill
			};

			var titleLabel = new Label () { 
				Text = "FIND ME",
				TextColor = Color.White,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 40,
				FontAttributes = FontAttributes.Bold
			};

			var activityIndicator = new ActivityIndicator () {
				IsRunning = true,
				Color = Color.White
			};

			var relativeLayout = new RelativeLayout () { 
				
			};

			relativeLayout.Children.Add (
				backgroundImage,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height;
				})
			);

			relativeLayout.Children.Add (
				titleLabel,
				Constraint.Constant(0),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height / 2 - 60;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width;
				}),
				Constraint.Constant(60)
			);

			relativeLayout.Children.Add (
				activityIndicator,
				Constraint.Constant(0),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height / 2;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width;
				}),
				Constraint.Constant(60)
			);

			this.Content = relativeLayout;
		}
	}
}

