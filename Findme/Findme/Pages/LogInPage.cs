using System;

using Xamarin.Forms;

namespace Findme
{
	public class LogInPage : ContentPage
	{

		// - Constructor
		public LogInPage ()
		{
			this.setMainLayout ();
			this.BackgroundColor = Color.White;
		}

		// - Test request
		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			NetworkingManager.sharedInstance.googleRequest ().ContinueWith( (task) => {
				ConsoleOutput.PrintLine(task.Result);
			});
		}

		// - Cunstructing the main page
		private void setMainLayout() {

			var backgroundImageBottom = new Image () {
				Source = new FileImageSource () { File = "city_drawing3x.jpg" },
				Aspect = Aspect.AspectFill,
				IsOpaque = true,
				Opacity = 0.8
			};

			var backgroundImageUpper = new Image () {
				Source = new FileImageSource () { File = "city_drawing3x.jpg" },
				Aspect = Aspect.AspectFill,
				IsOpaque = true,
				Opacity = 0.8
			};

			var circleView = new RoundedBoxView () {
				Color = Color.White,
				OutlineColor = Color.Black,
				OutlineWidth = 1,
				CornerRadius = 2000,
				WidthRequest = 100,
				HeightRequest = 100
			};

			var relativeLayout = new RelativeLayout () {
				HeightRequest = 100,
			};

			relativeLayout.Children.Add (
				backgroundImageUpper,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ( (parent) => {
					return parent.Width;
				} ),
				Constraint.RelativeToParent ( (parent) => {
					return parent.Height * 0.5;
				} )
			);

			relativeLayout.Children.Add (
				backgroundImageBottom,
				Constraint.Constant(0),
				Constraint.RelativeToParent ( (parent) => {
					return parent.Height - parent.Height * 0.5;	
				}),
				Constraint.RelativeToParent ( (parent) => {
					return parent.Width;
				} ),
				Constraint.RelativeToParent ( (parent) => {
					return parent.Height * 0.5;
				} )
			);

			double circleProportions = 0.7;
			relativeLayout.Children.Add (
				circleView,
				Constraint.RelativeToParent( (parent) => {
					return 0 - (parent.Height * circleProportions - parent.Width) / 2;
				}),
				Constraint.RelativeToParent( (parent) => {
					return (parent.Height - parent.Height * circleProportions) / 2;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height * circleProportions;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height * circleProportions;
				})
			);

			var usernameTextEntry = new Entry() {
				TextColor = Color.Black,
				PlaceholderColor = Color.Gray,
				Placeholder = "Email"
			};

			var passwordTextEntry = new Entry () {
				TextColor = Color.Black,
				PlaceholderColor = Color.Gray,
				Placeholder = "Password",
				IsPassword = true
			};

			relativeLayout.Children.Add (
				usernameTextEntry,
				Constraint.Constant(40),
				Constraint.Constant(300),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width - 40 * 2;
				}),
				Constraint.Constant(40)
			);

			relativeLayout.Children.Add (
				passwordTextEntry,
				Constraint.Constant(40),
				Constraint.Constant(350),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width - 40 * 2;
				}),
				Constraint.Constant(40)
			);

			this.Content = relativeLayout;
		}

	}
}


