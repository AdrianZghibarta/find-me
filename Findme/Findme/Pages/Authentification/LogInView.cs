using System;
using Xamarin.Forms;

namespace Findme
{
	public delegate void ManualLoginHandler();
	public delegate void FacebookLoginHandler();

	public class LogInView: RelativeLayout
	{
		#region -> UIElements

		// - Login Elements
		InputView usernameEntry;
		InputView loginpasswordEntry;
		public Button loginButton;
		public Button facebookLoginButton;
		public Button googleLoginButton;
		public Button showRegistrationButton;

		#endregion

		#region -> Proprietes

		public AuthentificationPage authPage;
		public ManualLoginHandler didPerformedManualLogin;
		public FacebookLoginHandler didPerformFacebookLogin;

		#endregion

		#region -> Functionalities

		public void setButtonHandlers() {

			this.facebookLoginButton.Clicked += (object sender, EventArgs e) => {

				this.authPage.showFacebookAuth();
			};

			this.loginButton.Clicked += (object sender, EventArgs e) => {

				// - Validate data
				if (!this.isDataValid()) {
					this.authPage.DisplayAlert("Validation Error", "Please complete all fields!", "OK");
					return;
				}

				this.authPage.loadingView.IsVisible = true;
				// - Perform the request
				AuthentificationManager.SharedInstance.AuthentificateUser(
					email: this.usernameEntry.entry.Text,
					password: this.loginpasswordEntry.entry.Text
				).ContinueWith ( task => {

					this.authPage.loadingView.IsVisible = false;
					FindMeResponse response = (FindMeResponse)task.Result;
					if (null != response.ErrorInfo) {
						Device.BeginInvokeOnMainThread( () => {
							authPage.DisplayAlert("Error", response.ErrorInfo, "Ok");
						});
					}
					else {
						Device.BeginInvokeOnMainThread( () => {
							Navigation.PushModalAsync(new RootPage());
						});
					}
				});
			};
		}

		public bool isDataValid() {

			Entry[] entriesToValidate = new Entry[] { 
				usernameEntry.entry,
				loginpasswordEntry.entry
			};
			return Validator.ValidateEntires (entriesToValidate);
		}

		#endregion

		#region -> Layout Setup

		public LogInView ()
		{
			// - The background image

			var backgroundImage = new Image () {
				BackgroundColor = ColorMap.GreenColor,
				Aspect = Aspect.AspectFill
			};

			this.Children.Add (
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

			// - Manual login controls

			double textEntriesPadding = 40;
			double textEntriesHeight = 40;

			HeaderView manualLoginHeader = new HeaderView() {
				Title = "Manual Login",
				ImagesName = "circle.png",
				HeightRequest = textEntriesHeight
			};

			usernameEntry = new InputView ();
			usernameEntry.entry.TextColor = Color.White;
			usernameEntry.entry.PlaceholderColor = Color.White;
			usernameEntry.entry.Keyboard = Keyboard.Email;
			usernameEntry.bottomLine.BackgroundColor = Color.White;
			usernameEntry.image.Source = new FileImageSource () { File = "user_white.png"}; 
			usernameEntry.entry.Placeholder = "Email";
			usernameEntry.HeightRequest = textEntriesHeight;

			loginpasswordEntry = new InputView ();
			loginpasswordEntry.entry.TextColor = Color.White;
			loginpasswordEntry.entry.PlaceholderColor = Color.White;
			loginpasswordEntry.entry.IsPassword = true;
			loginpasswordEntry.bottomLine.BackgroundColor = Color.White;
			loginpasswordEntry.image.Source = new FileImageSource () { File = "lock_white.png"}; 
			loginpasswordEntry.entry.Placeholder = "Password";
			loginpasswordEntry.HeightRequest = textEntriesHeight;

			loginButton = new Button() {
				BackgroundColor = ColorMap.BlackTransparentBackground,
				TextColor = Color.White,
				BorderRadius = 3,
				BorderColor = Color.White,
				BorderWidth = 2,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				HeightRequest = textEntriesHeight,
				Text = "LOG IN"
			};

			HeaderView socialLoginHeader = new HeaderView() {
				Title = "Social Login",
				ImagesName = "circle.png",
				HeightRequest = textEntriesHeight
			};

			facebookLoginButton = new Button () {
				BackgroundColor = ColorMap.BlackTransparentBackground,
				TextColor = Color.White,
				BorderRadius = 3,
				BorderColor = Color.White,
				BorderWidth = 2,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				HeightRequest = textEntriesHeight,
				Text = "Facebook"
			};

			googleLoginButton = new Button () { 
				BackgroundColor = ColorMap.BlackTransparentBackground,
				TextColor = Color.White,
				BorderRadius = 3,
				BorderColor = Color.White,
				BorderWidth = 2,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				HeightRequest = textEntriesHeight,
				Text = "Google"
			};

			var stackContentView = new StackLayout () { 
				Padding = new Thickness(textEntriesPadding, 100, textEntriesPadding, 125),
				Spacing = 5,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children = {
					manualLoginHeader,
					this.usernameEntry,
					this.loginpasswordEntry,
					this.loginButton,
					socialLoginHeader,
					this.facebookLoginButton,
					this.googleLoginButton
				}
			};

			var scrollView = new ScrollView () {
				Content =  stackContentView,
			};
					
			this.Children.Add (
				scrollView,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height;
				})
			);


			// - The circle bottom view

			var circleView = new RoundedBoxView () {
				Color = ColorMap.DarkBlueColor,
				OutlineColor = ColorMap.DarkBlueColor,
				OutlineWidth = 1,
				CornerRadius = 2000,
				WidthRequest = 100,
				HeightRequest = 100
			};

			double circleDiametre = 700;
			double visibleCirclePropotion = 0.15;
			this.Children.Add (
				circleView,
				Constraint.RelativeToParent( (parent) => {
					return 0 - (circleDiametre - parent.Width) / 2;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height -circleDiametre * visibleCirclePropotion;
				}),
				Constraint.RelativeToParent( (parent) => {
					return circleDiametre;
				}),
				Constraint.RelativeToParent( (parent) => {
					return circleDiametre;
				})
			);

			double showRegistrationButtonWidth = 250;
			double showRegistrationButtonHeight = 30;

			showRegistrationButton = new Button() {
				BackgroundColor = Color.Transparent,
				TextColor = Color.White,
				BorderRadius = 0,
				BorderColor = Color.Transparent,
				BorderWidth = 0,
				FontSize = 16,
				FontAttributes = FontAttributes.Italic,
				Text = "You don't have an account ?"
			};

			this.Children.Add (
				showRegistrationButton,
				Constraint.RelativeToParent ((parent) => {
					return (parent.Width - showRegistrationButtonWidth) / 2;
				}),
				Constraint.RelativeToParent ((parent) => {
					return parent.Height - showRegistrationButtonHeight - 20;
				}),
				Constraint.Constant (showRegistrationButtonWidth),
				Constraint.Constant (showRegistrationButtonHeight)
			);

			this.setButtonHandlers ();
		}

		#endregion
	}
}

