using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Findme
{
	public class AuthentificationPage : AuthPage
	{
		#region -> UI Elements

		// - Containers for login and registration
		LogInView loginContainer = new LogInView ();
		RegistrationView registerContainer = new RegistrationView ();

		// - Other proprietes
		uint animationSpeed = 300;

		#endregion

		#region -> Base class and initialisation methods

		// - Constructor
		public AuthentificationPage ()
		{
			this.setMainLayout ();
			this.BackgroundColor = Color.White;
			this.setButtonsHandlers ();
			this.setSocialTokenHandlers ();
		}

		private void setButtonsHandlers() {
			
			loginContainer.showRegistrationButton.Clicked += (object sender, EventArgs e) => {

				Rectangle newLoginContainerBounds = new Rectangle(0, -Content.Bounds.Height, Content.Width, Content.Height);
				Rectangle newRegisterContainerBounds = new Rectangle(0, 0, Content.Width, Content.Height);

				loginContainer.LayoutTo(newLoginContainerBounds, animationSpeed, Easing.Linear);
				registerContainer.LayoutTo(newRegisterContainerBounds, animationSpeed, Easing.Linear);
			};

			registerContainer.showLogInButton.Clicked += (object sender, EventArgs e) => {

				Rectangle newLoginContainerBounds = new Rectangle(0, 0, Content.Width, Content.Height);
				Rectangle newRegisterContainerBounds = new Rectangle(0, Content.Bounds.Height, Content.Width, Content.Height);

				loginContainer.LayoutTo(newLoginContainerBounds, animationSpeed, Easing.Linear);
				registerContainer.LayoutTo(newRegisterContainerBounds, animationSpeed, Easing.Linear);
			};
		}

		private void setSocialTokenHandlers() {

			this.didGetFacebookAccessToken = ((accessToken) => {

				AuthentificationManager.SharedInstance.AuthentificateUserWithFacebookToken(accessToken).ContinueWith( task => {

					FindMeResponse response = task.Result;

				});
			});

			this.didGetGoogleAccesToken = ((accessToken) => {

				ConsoleOutput.PrintLine(accessToken);
			});
		}

		#endregion

		#region -> Setup for main layout

		private void setMainLayout() {
			// - The page conteiner

			var rootLayout = new RelativeLayout ();

			loginContainer.IsClippedToBounds = true;
			registerContainer.IsClippedToBounds = true;

			rootLayout.Children.Add (
				loginContainer,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height;
				})
			);
				
			rootLayout.Children.Add (
				registerContainer,
				Constraint.Constant(0),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height;
				})
			);

			this.loginContainer.authPage = this;
			this.registerContainer.authPage = this;
			this.Content = rootLayout;
		}

		#endregion
	}
}


