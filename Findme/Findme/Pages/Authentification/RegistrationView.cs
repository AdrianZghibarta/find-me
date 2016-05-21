using System;
using Xamarin.Forms;
using XLabs.Forms.Services;
using XLabs.Platform.Services.Media;
using System.Threading.Tasks;
using XLabs.Ioc;

namespace Findme
{
	public delegate void RegistrationHandler();

	public class RegistrationView: RelativeLayout
	{
		#region -> UI Elements

		// - Registration Elements
		Image avatarImage;
		InputView emailEntry;
		InputView firstNameEntry;
		InputView lastNameEntry;
		InputView passwordEntry;
		InputView countryEntry;
		InputView cityEntry;
		InputView addressEntry;
		InputView phoneNumber;
		Button selectImageButton;
		public Button registrationButton;
		public Button showLogInButton;

		#endregion

		#region -> Proprietes

		public AuthentificationPage authPage;
		public RegistrationHandler didPerformedRegistration;
		private String imageInBase64;

		#endregion

		#region -> Functionalities

		public bool isDataValid() {

			Entry[] entriesToValidate = new Entry[] { 
				emailEntry.entry, 
				firstNameEntry.entry, 
				lastNameEntry.entry, 
				passwordEntry.entry
			};
			return Validator.ValidateEntires (entriesToValidate);
		}

		private void setButtonHandlers() {

			this.registrationButton.Clicked += (object sender, EventArgs e) => {

				// - Validate the data from the page
				if (!this.isDataValid()) {
					this.authPage.DisplayAlert("Validation Error", "Please complete all fields!", "OK");
					return;
				}

				this.authPage.loadingView.IsVisible = true;
				// - If it's all ok then perform the requst
				AuthentificationManager.SharedInstance.RegisterUser(
					email: this.emailEntry.entry.Text,
					password: this.passwordEntry.entry.Text,
					firstName: this.firstNameEntry.entry.Text,
					lastName: this.lastNameEntry.entry.Text,
					imageBase64: this.imageInBase64
				).ContinueWith( task => {

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

			this.selectImageButton.Clicked += (object sender, EventArgs e) => {

				this.selectPhoto().ContinueWith( task => {});
			};
		}

		private async Task selectPhoto() {

			try {
				var mediaPicker = Resolver.Resolve<IMediaPicker> ();//DependencyService.Get<IMediaPicker> ();
				var mediaFile = await mediaPicker.SelectPhotoAsync (new CameraMediaStorageOptions {
					DefaultCamera = CameraDevice.Front,
					MaxPixelDimension = 400
				});

				byte[] bytes = await StreamHelper.GetBytesArrayFromStream(mediaFile.Source);
				this.imageInBase64 = Convert.ToBase64String(bytes);
				this.avatarImage.Source = ImageSource.FromStream(() => mediaFile.Source);
			} 
			catch (System.Exception ex) {
				Device.BeginInvokeOnMainThread (() => {
					this.authPage.DisplayAlert("Error", ex.Message, "Ok");
				});
			}
		}

		#endregion

		#region -> Layout Setup

		public RegistrationView ()
		{
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

			double textEntriesPadding = 40;
			double textEntriesHeight = 40;
			double imageDim = 100;

			var imageContainer = new RelativeLayout () { 
				HeightRequest = imageDim
			};

			this.avatarImage = new ImageCircle () {
				Source = new FileImageSource () {
					File = "photoPlaceholder.png"
				},
				BorderColor = Color.White,
				BorderWidth = 2,
				Aspect = Aspect.AspectFill
			};

			this.selectImageButton = new Button () { 
				BackgroundColor = Color.Transparent,
				Text = ""
			};

			imageContainer.Children.Add (
				avatarImage,
				Constraint.RelativeToParent( (parent) => {
					return (parent.Width - imageDim) / 2;
				}),
				Constraint.Constant(0),
				Constraint.Constant(imageDim),
				Constraint.Constant(imageDim)
			);

			imageContainer.Children.Add (
				selectImageButton,
				Constraint.RelativeToParent( (parent) => {
					return (parent.Width - imageDim) / 2;
				}),
				Constraint.Constant(0),
				Constraint.Constant(imageDim),
				Constraint.Constant(imageDim)
			);

			HeaderView generalInfoHeader = new HeaderView() {
				Title = "General Info",
				ImagesName = "circle.png",
				HeightRequest = textEntriesHeight
			};

			emailEntry = new InputView ();
			emailEntry.entry.TextColor = Color.White;
			emailEntry.entry.PlaceholderColor = Color.White;
			emailEntry.entry.Keyboard = Keyboard.Email;
			emailEntry.bottomLine.BackgroundColor = Color.White;
			emailEntry.image.Source = new FileImageSource () { File = "inbox.png"}; 
			emailEntry.entry.Placeholder = "Email";
			emailEntry.HeightRequest = textEntriesHeight;

			firstNameEntry = new InputView ();
			firstNameEntry.entry.TextColor = Color.White;
			firstNameEntry.entry.PlaceholderColor = Color.White;
			firstNameEntry.bottomLine.BackgroundColor = Color.White;
			firstNameEntry.image.Source = new FileImageSource () { File = "user_white.png"}; 
			firstNameEntry.entry.Placeholder = "First Name";
			firstNameEntry.HeightRequest = textEntriesHeight;

			lastNameEntry = new InputView ();
			lastNameEntry.entry.TextColor = Color.White;
			lastNameEntry.entry.PlaceholderColor = Color.White;
			lastNameEntry.bottomLine.BackgroundColor = Color.White;
			lastNameEntry.image.Source = new FileImageSource () { File = "many_user.png"}; 
			lastNameEntry.entry.Placeholder = "Last Name";
			lastNameEntry.HeightRequest = textEntriesHeight;

			passwordEntry = new InputView ();
			passwordEntry.entry.TextColor = Color.White;
			passwordEntry.entry.PlaceholderColor = Color.White;
			passwordEntry.entry.IsPassword = true;
			passwordEntry.bottomLine.BackgroundColor = Color.White;
			passwordEntry.image.Source = new FileImageSource () { File = "lock_white.png"}; 
			passwordEntry.entry.Placeholder = "Password";
			passwordEntry.HeightRequest = textEntriesHeight;

			HeaderView addressHeader = new HeaderView() {
				Title = "Address Info",
				ImagesName = "circle.png",
				HeightRequest = textEntriesHeight
			};

			countryEntry = new InputView ();
			countryEntry.entry.TextColor = Color.White;
			countryEntry.entry.PlaceholderColor = Color.White;
			countryEntry.bottomLine.BackgroundColor = Color.White;
			countryEntry.image.Source = new FileImageSource () { File = "country.png"}; 
			countryEntry.entry.Placeholder = "Country";
			countryEntry.HeightRequest = textEntriesHeight;


			cityEntry = new InputView ();
			cityEntry.entry.TextColor = Color.White;
			cityEntry.entry.PlaceholderColor = Color.White;
			cityEntry.bottomLine.BackgroundColor = Color.White;
			cityEntry.image.Source = new FileImageSource () { File = "city.png"}; 
			cityEntry.entry.Placeholder = "City";
			cityEntry.HeightRequest = textEntriesHeight;

			addressEntry = new InputView ();
			addressEntry.entry.TextColor = Color.White;
			addressEntry.entry.PlaceholderColor = Color.White;
			addressEntry.bottomLine.BackgroundColor = Color.White;
			addressEntry.image.Source = new FileImageSource () { File = "address.png"}; 
			addressEntry.entry.Placeholder = "Address";
			addressEntry.HeightRequest = textEntriesHeight;

			phoneNumber = new InputView ();
			phoneNumber.entry.TextColor = Color.White;
			phoneNumber.entry.PlaceholderColor = Color.White;
			phoneNumber.entry.Keyboard = Keyboard.Telephone;
			phoneNumber.bottomLine.BackgroundColor = Color.White;
			phoneNumber.image.Source = new FileImageSource () { File = "phone.png"}; 
			phoneNumber.entry.Placeholder = "Phone";
			phoneNumber.HeightRequest = textEntriesHeight;

			var bottomSeparatorView = new RelativeLayout () { 
				HeightRequest = 5
			};

			registrationButton = new Button() {
				BackgroundColor = ColorMap.BlackTransparentBackground,
				TextColor = Color.White,
				BorderRadius = 3,
				BorderColor = Color.White,
				BorderWidth = 2,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				Text = "REGISTER",
				HeightRequest = textEntriesHeight
			};

			var stackContentView = new StackLayout () {
				Padding = new Thickness(textEntriesPadding, 125, textEntriesPadding, 25),
				Spacing = 5,
				VerticalOptions = LayoutOptions.Start,
				Children = {
					imageContainer,
					generalInfoHeader,
					this.emailEntry,
					this.firstNameEntry,
					this.lastNameEntry,
					this.passwordEntry,
					//addressHeader,
					//this.countryEntry,
					//this.cityEntry,
					//this.addressEntry,
					//this.phoneNumber,
					bottomSeparatorView,
					this.registrationButton
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
					return -(circleDiametre - parent.Width) / 2;
				}),
				Constraint.Constant(-circleDiametre * (1 - visibleCirclePropotion)),
				Constraint.Constant(circleDiametre),
				Constraint.Constant(circleDiametre)
			);

			circleView.IsVisible = false;

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

			this.setButtonHandlers ();
		}

		#endregion
	}
}

