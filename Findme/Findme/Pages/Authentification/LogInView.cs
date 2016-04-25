using System;
using Xamarin.Forms;

namespace Findme
{
	public class LogInView: RelativeLayout
	{
		#region -> UIElements

		// - Login Elements
		InputView usernameEntry;
		InputView loginpasswordEntry;
		Button loginButton;
		Button facebookLoginButton;
		Button googleLoginButton;
		public Button showRegistrationButton;

		#endregion

		#region -> Layout Setup

		public LogInView ()
		{
			// - The login views

			var backgroundImage = new Image () {
				Source = new FileImageSource () { File = "blurredBackground.jpg" },
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

			var circleView = new RoundedBoxView () {
				Color = ColorMap.OrangeColor,
				OutlineColor = ColorMap.OrangeColor,
				OutlineWidth = 1,
				CornerRadius = 2000,
				WidthRequest = 100,
				HeightRequest = 100
			};

			double circleProportions = 0.7;
			double visibleCirclePropotion = 0.3;
			this.Children.Add (
				circleView,
				Constraint.RelativeToParent( (parent) => {
					return 0 - (parent.Height * circleProportions - parent.Width) / 2;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height - parent.Height * circleProportions * visibleCirclePropotion;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height * circleProportions;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height * circleProportions;
				})
			);

			usernameEntry = new InputView ();
			usernameEntry.entry.TextColor = Color.White;
			usernameEntry.entry.PlaceholderColor = Color.White;
			usernameEntry.entry.Keyboard = Keyboard.Email;
			usernameEntry.bottomLine.BackgroundColor = Color.White;
			usernameEntry.image.Source = new FileImageSource () { File = "user_white.png"}; 
			usernameEntry.entry.Placeholder = "Email";

			loginpasswordEntry = new InputView ();
			loginpasswordEntry.entry.TextColor = Color.White;
			loginpasswordEntry.entry.PlaceholderColor = Color.White;
			loginpasswordEntry.entry.IsPassword = true;
			loginpasswordEntry.bottomLine.BackgroundColor = Color.White;
			loginpasswordEntry.image.Source = new FileImageSource () { File = "lock_white.png"}; 
			loginpasswordEntry.entry.Placeholder = "Password";

			loginButton = new Button() {
				BackgroundColor = Color.Transparent,
				TextColor = Color.White,
				BorderRadius = 3,
				BorderColor = Color.White,
				BorderWidth = 2,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				Text = "LOG IN"
			};

			double textEntriesWidth = 250;
			double textEntriesHeight = 40;
			this.Children.Add (
				usernameEntry,
				Constraint.RelativeToParent( (parent) => {
					return (parent.Width - textEntriesWidth) / 2;
				}),
				Constraint.Constant(70),
				Constraint.Constant(textEntriesWidth),
				Constraint.Constant(textEntriesHeight)
			);

			this.Children.Add (
				loginpasswordEntry,
				Constraint.RelativeToParent ((parent) => {
					return (parent.Width - textEntriesWidth) / 2;
				}),
				Constraint.Constant (115),
				Constraint.Constant (textEntriesWidth),
				Constraint.Constant (textEntriesHeight)
			);

			this.Children.Add (
				loginButton,
				Constraint.RelativeToParent ((parent) => {
					return (parent.Width - textEntriesWidth) / 2;
				}),
				Constraint.Constant (175),
				Constraint.Constant (textEntriesWidth),
				Constraint.Constant (textEntriesHeight)
			);

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

			double showRegistrationButtonWidth = 250;
			double showRegistrationButtonHeight = 30;
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
		}

		#endregion
	}
}

