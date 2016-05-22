using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using XLabs.Platform.Services.Media;
using XLabs.Ioc;

namespace Findme
{
	public class EditAccountPage: ContentPage
	{
		public EditAccountPage ()
		{
			this.Title = "Edit Account";
			this.setUpLayout ();
			this.setButtonHandlers ();
			this.GetUserAndUpdateUI ();
		}

		#region -> UI Elements

		// - Registration Elements
		Image avatarImage;
		InputView firstNameEntry;
		InputView lastNameEntry;
		InputView passwordEntry;
		InputView secPasswordEntry;
		InputView countryEntry;
		InputView cityEntry;
		InputView addressEntry;
		InputView phoneNumber;
		Button selectImageButton;
		LoadingView loadingView;
		Button registrationButton;
		Button logoutButton;

		#endregion

		#region -> Proprietes

		private String imageInBase64;

		#endregion

		#region -> Functionalities

		public void GetUserAndUpdateUI() {
		
			this.loadingView.Show ();
			AuthentificationManager.SharedInstance.GetUser ().ContinueWith ( task => {

				this.loadingView.Hide();
				FindMeResponse response = (FindMeResponse)task.Result;
				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {
					Device.BeginInvokeOnMainThread( () => {
						User user = (User)response.Result;
						this.firstNameEntry.entry.Text = user.firstname;
						this.lastNameEntry.entry.Text = user.lastname;
						var photoUrl = NetworkingUrls.BASE_URL + user.photourl;
						this.avatarImage.Source = ImageSource.FromUri(new Uri(photoUrl));
						// TODO - Show the image here
					});
				}
			});
		}

		public bool isDataValid() {

			if (passwordEntry.entry.Text != null || secPasswordEntry.entry.Text != null) {
				if (passwordEntry.entry.Text != "" || secPasswordEntry.entry.Text != "") {
					if (passwordEntry.entry.Text != secPasswordEntry.entry.Text) {
						return false;
					}
				}
			}

			Entry[] entriesToValidate = new Entry[] {
				firstNameEntry.entry, 
				lastNameEntry.entry
			};
			return Validator.ValidateEntires (entriesToValidate);
		}

		private void setButtonHandlers() {

			this.registrationButton.Clicked += (object sender, EventArgs e) => {

				// - Validate the data from the page
				if (!this.isDataValid()) {
					this.DisplayAlert("Validation Error", "Please complete all fields!", "OK");
					return;
				}

				this.loadingView.Show();
				// - If it's all ok then perform the requst
				AuthentificationManager.SharedInstance.EditUser(
					firstName: this.firstNameEntry.entry.Text,
					lastName: this.lastNameEntry.entry.Text,
					password: this.passwordEntry.entry.Text,
					imageBase64: this.imageInBase64
				).ContinueWith( task => {

					this.loadingView.Hide();
					FindMeResponse response = (FindMeResponse)task.Result;
					if (null != response.ErrorInfo) {
						Device.BeginInvokeOnMainThread( () => {
							this.DisplayAlert("Error", response.ErrorInfo, "Ok");
						});
					}
					else {
						Device.BeginInvokeOnMainThread( () => {
							this.DisplayAlert("Succes", "User was successful edited.", "Ok");
						});
					}
				});
			};

			this.selectImageButton.Clicked += (object sender, EventArgs e) => {

				this.selectPhoto().ContinueWith( task => {});
			};

			this.logoutButton.Clicked += (object sender, EventArgs e) => {
				this.showLogoutPopup().ContinueWith( task => {});
			};
		}

		private async Task showLogoutPopup() {
			var action = await DisplayActionSheet ("Do you want to exit ?", "Cancel", null, "Logout");
			if (action == "Logout") {
				UserStorage.DeleteAccessAndRefreshToken ();
				Device.BeginInvokeOnMainThread (() => {
					Xamarin.Forms.Application.Current.MainPage = new AuthentificationPage ();
				});
			}
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
					this.DisplayAlert("Error", ex.Message, "Ok");
				});
			}
		}

		#endregion

		#region -> Layout Setup

		private void setUpLayout ()
		{
			var rootRelativeLayout = new RelativeLayout ();
			
			var backgroundImage = new Image () {
				BackgroundColor = ColorMap.GreenColor,
				Aspect = Aspect.AspectFill
			};

			rootRelativeLayout.Children.Add (
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
				HeightRequest = imageDim + 10
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

			secPasswordEntry = new InputView ();
			secPasswordEntry.entry.TextColor = Color.White;
			secPasswordEntry.entry.PlaceholderColor = Color.White;
			secPasswordEntry.entry.IsPassword = true;
			secPasswordEntry.bottomLine.BackgroundColor = Color.White;
			secPasswordEntry.image.Source = new FileImageSource () { File = "lock_white.png"}; 
			secPasswordEntry.entry.Placeholder = "Retype Password";
			secPasswordEntry.HeightRequest = textEntriesHeight;

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
				Text = "EDIT",
				HeightRequest = textEntriesHeight
			};

			logoutButton = new Button() {
				BackgroundColor = ColorMap.RedTransparentBackground,
				TextColor = Color.White,
				BorderRadius = 3,
				BorderColor = Color.White,
				BorderWidth = 2,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				Text = "LOG OUT",
				HeightRequest = textEntriesHeight
			};

			var stackContentView = new StackLayout () {
				Padding = new Thickness(textEntriesPadding, 25, textEntriesPadding, 25),
				Spacing = 5,
				VerticalOptions = LayoutOptions.Start,
				Children = {
					imageContainer,
					generalInfoHeader,
					this.firstNameEntry,
					this.lastNameEntry,
					this.passwordEntry,
					this.secPasswordEntry,
					//addressHeader,
					//this.countryEntry,
					//this.cityEntry,
					//this.addressEntry,
					//this.phoneNumber,
					bottomSeparatorView,
					this.registrationButton,
					new RelativeLayout() {
						HeightRequest = 15
					},
					this.logoutButton

				}
			};

			var scrollView = new ScrollView () {
				Content =  stackContentView,
			};

			rootRelativeLayout.Children.Add (
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

			this.loadingView = new LoadingView ();
			loadingView.AddTo (rootRelativeLayout);

			this.Content = rootRelativeLayout;
		}

		#endregion
	}
}

