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

