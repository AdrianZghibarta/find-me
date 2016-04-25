using System;
using Xamarin.Forms;

namespace Findme
{
	public class RegistrationView: RelativeLayout
	{
		#region -> UI Elements

		// - Registration Elements
		InputView emailEntry;
		InputView firstNameEntry;
		InputView lastNameEntry;
		InputView passwordEntry;
		InputView countryEntry;
		InputView cityEntry;
		InputView addressEntry;
		InputView phoneNumber;
		Button registrationButton;
		Button facebookRegistrationButton;
		Button googleRegistrationButton;
		public Button showLogInButton;

		#endregion

		#region -> Layout Setup

		public RegistrationView ()
		{
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



			emailEntry = new InputView ();
			emailEntry.entry.TextColor = Color.White;
			emailEntry.entry.PlaceholderColor = Color.White;
			emailEntry.entry.Keyboard = Keyboard.Email;
			emailEntry.bottomLine.BackgroundColor = Color.White;
			emailEntry.image.Source = new FileImageSource () { File = "user_white.png"}; 
			emailEntry.entry.Placeholder = "Email";

			firstNameEntry = new InputView ();
			firstNameEntry.entry.TextColor = Color.White;
			firstNameEntry.entry.PlaceholderColor = Color.White;
			firstNameEntry.bottomLine.BackgroundColor = Color.White;
			firstNameEntry.image.Source = new FileImageSource () { File = "user_white.png"}; 
			firstNameEntry.entry.Placeholder = "First Name";

			lastNameEntry = new InputView ();
			lastNameEntry.entry.TextColor = Color.White;
			lastNameEntry.entry.PlaceholderColor = Color.White;
			lastNameEntry.bottomLine.BackgroundColor = Color.White;
			lastNameEntry.image.Source = new FileImageSource () { File = "user_white.png"}; 
			lastNameEntry.entry.Placeholder = "Last Name";

			passwordEntry = new InputView ();
			passwordEntry.entry.TextColor = Color.White;
			passwordEntry.entry.PlaceholderColor = Color.White;
			passwordEntry.entry.IsPassword = true;
			passwordEntry.bottomLine.BackgroundColor = Color.White;
			passwordEntry.image.Source = new FileImageSource () { File = "lock_white.png"}; 
			passwordEntry.entry.Placeholder = "Password";

			countryEntry = new InputView ();
			countryEntry.entry.TextColor = Color.White;
			countryEntry.entry.PlaceholderColor = Color.White;
			countryEntry.bottomLine.BackgroundColor = Color.White;
			countryEntry.image.Source = new FileImageSource () { File = "user_white.png"}; 
			countryEntry.entry.Placeholder = "Country";


			cityEntry = new InputView ();
			cityEntry.entry.TextColor = Color.White;
			cityEntry.entry.PlaceholderColor = Color.White;
			cityEntry.bottomLine.BackgroundColor = Color.White;
			cityEntry.image.Source = new FileImageSource () { File = "user_white.png"}; 
			cityEntry.entry.Placeholder = "City";

			addressEntry = new InputView ();
			addressEntry.entry.TextColor = Color.White;
			addressEntry.entry.PlaceholderColor = Color.White;
			addressEntry.bottomLine.BackgroundColor = Color.White;
			addressEntry.image.Source = new FileImageSource () { File = "user_white.png"}; 
			addressEntry.entry.Placeholder = "Address";

			phoneNumber = new InputView ();
			phoneNumber.entry.TextColor = Color.White;
			phoneNumber.entry.PlaceholderColor = Color.White;
			phoneNumber.entry.Keyboard = Keyboard.Telephone;
			phoneNumber.bottomLine.BackgroundColor = Color.White;
			phoneNumber.image.Source = new FileImageSource () { File = "user_white.png"}; 
			phoneNumber.entry.Placeholder = "Phone";

			var scrollView = new ScrollView () {
				Content = new StackLayout() {

					Padding = new Thickness(50, 140, 50, 200),
					Children = {
						this.emailEntry,
						this.firstNameEntry,
						this.lastNameEntry,
						this.passwordEntry,
						this.countryEntry,
						this.cityEntry,
						this.addressEntry,
						this.phoneNumber
					}
				}
			};

			this.Children.Add (
				scrollView,
				Constraint.Constant(0),
				Constraint.Constant(20),
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
					return 0 - parent.Height * circleProportions * ( 1 - visibleCirclePropotion);
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height * circleProportions;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height * circleProportions;
				})
			);

			showLogInButton = new Button() {
				BackgroundColor = Color.Transparent,
				TextColor = Color.White,
				BorderRadius = 0,
				BorderColor = Color.Transparent,
				BorderWidth = 0,
				FontSize = 16,
				FontAttributes = FontAttributes.Italic,
				Text = "Already have an account ?"
			};

			double showLoginButtonWidth = 250;
			double showLoginButtonHeight = 30;
			this.Children.Add (
				showLogInButton,
				Constraint.RelativeToParent ((parent) => {
					return (parent.Width - showLoginButtonWidth) / 2;
				}),
				Constraint.Constant (20),
				Constraint.Constant (showLoginButtonWidth),
				Constraint.Constant (showLoginButtonHeight)
			);
		}

		#endregion
	}
}

