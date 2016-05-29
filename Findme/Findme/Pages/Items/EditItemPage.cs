using System;
using Xamarin.Forms;
using XLabs.Ioc;
using System.Threading.Tasks;
using XLabs.Platform.Services.Media;
using System.Collections.Generic;

namespace Findme
{
	public class EditItemPage: ContentPage
	{
		public EditItemPage ()
		{
			this.Title = "Add Item";
			this.setUpLayout ();
			this.getCategoryesAndBeacons ();
			this.setButtonHandlers ();
			this.updateUIForItem ();
		}

		public EditItemPage (Item item) {

			this.Title = "Edit Item";
			this.itemToEdit = item;
			this.setUpLayout ();
			this.getCategoryesAndBeacons ();
			this.setButtonHandlers ();
			this.updateUIForItem ();
		}

		#region -> UI Elements

		// - Registration Elements
		Image itemImage;
		InputView nameEntry;
		InputView descriptionEntry;
		InputView categoryEntry;
		InputView beaconEntry;
		Button selectImageButton;
		LoadingView loadingView;
		Button submitButton;
		Picker categoryPicker;
		Picker beaconPicker;

		#endregion

		#region -> Proprietes

		private String imageInBase64;
		private bool hasCategories;
		private bool hasBeacons;
		private List<FMBeacon> beaconsList;
		private List<Category> categoriesList;
		public Item itemToEdit;

		#endregion

		#region -> Functionalities

		public void updateUIForItem() {

			if (this.itemToEdit != null) {
				this.nameEntry.entry.Text = this.itemToEdit.name;
				this.descriptionEntry.entry.Text = this.itemToEdit.description;
				this.categoryEntry.entry.Text = this.itemToEdit.category;
				if (this.itemToEdit.beacon != null) {
					this.beaconEntry.entry.Text = this.itemToEdit.beacon.GetGeneralInformation ();
				}
				var photoUrl = NetworkingUrls.BASE_URL + this.itemToEdit.photoUrl;
				this.itemImage.Source = ImageSource.FromUri(new Uri(photoUrl));	
			}
		}

		private void getCategoryesAndBeacons() {

			this.loadingView.Show ();
			BeaconsManager.SharedInstance.GetUserBeacons ().ContinueWith ( task => {

				FindMeResponse response = (FindMeResponse)task.Result;

				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {

					this.beaconsList = (List<FMBeacon>)response.Result;
					// - Filter only the beacons that are not attaced
					this.hasBeacons = true;
					if (this.hasCategories) {
						this.loadingView.Hide();
						this.setUpPickers();
					}
				}
			});

			ItemsManager.SharedInstance.GetCategories ().ContinueWith ( task => {

				FindMeResponse response = (FindMeResponse)task.Result;

				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {

					this.categoriesList = (List<Category>)response.Result;

					this.hasCategories = true;
					if (this.hasBeacons) {
						this.loadingView.Hide();
						this.setUpPickers();
					}
				}
			});
		}

		public bool isDataValid() {

			Entry[] entriesToValidate = new Entry[] {
				nameEntry.entry, 
				descriptionEntry.entry,
				categoryEntry.entry,
				beaconEntry.entry
			};
			return Validator.ValidateEntires (entriesToValidate);
		}

		private void setButtonHandlers() {

			this.submitButton.Clicked += (object sender, EventArgs e) => {

				// - Validate the data from the page
				if (!this.isDataValid()) {
					this.DisplayAlert("Validation Error", "Please complete all fields!", "OK");
					return;
				}

				//this.loadingView.Show();
				// - If it's all ok then perform the requst
//				AuthentificationManager.SharedInstance.EditUser(
//					firstName: this.firstNameEntry.entry.Text,
//					lastName: this.lastNameEntry.entry.Text,
//					password: this.passwordEntry.entry.Text,
//					imageBase64: this.imageInBase64
//				).ContinueWith( task => {
//
//					this.loadingView.Hide();
//					FindMeResponse response = (FindMeResponse)task.Result;
//					if (null != response.ErrorInfo) {
//						Device.BeginInvokeOnMainThread( () => {
//							this.DisplayAlert("Error", response.ErrorInfo, "Ok");
//						});
//					}
//					else {
//						Device.BeginInvokeOnMainThread( () => {
//							this.DisplayAlert("Succes", "User was successful edited.", "Ok");
//						});
//					}
//				});
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
				this.itemImage.Source = ImageSource.FromStream(() => mediaFile.Source);
			} 
			catch (System.Exception ex) {
				Device.BeginInvokeOnMainThread (() => {
					this.DisplayAlert("Error", ex.Message, "Ok");
				});
			}
		}

		private void setUpPickers() {

			this.categoryEntry.entry.Focused += (object sender, FocusEventArgs e) => {
				//this.categoryEntry.entry.Unfocus();
				this.categoryPicker.IsVisible = true;
				this.categoryPicker.Focus();
				this.beaconPicker.IsVisible = false;
			};

			this.beaconEntry.entry.Focused += (object sender, FocusEventArgs e) => {
				this.beaconEntry.entry.Unfocus();
				this.beaconPicker.IsVisible = true;
				this.beaconPicker.Focus();
				this.categoryPicker.IsVisible = false;
			};
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

			this.itemImage = new ImageCircle () {
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
				itemImage,
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

			nameEntry = new InputView ();
			nameEntry.entry.TextColor = Color.White;
			nameEntry.entry.PlaceholderColor = Color.White;
			nameEntry.bottomLine.BackgroundColor = Color.White;
			nameEntry.image.Source = new FileImageSource () { File = "nameIconWhite.png"}; 
			nameEntry.entry.Placeholder = "Name";
			nameEntry.HeightRequest = textEntriesHeight;

			descriptionEntry = new InputView ();
			descriptionEntry.entry.TextColor = Color.White;
			descriptionEntry.entry.PlaceholderColor = Color.White;
			descriptionEntry.bottomLine.BackgroundColor = Color.White;
			descriptionEntry.image.Source = new FileImageSource () { File = "InfoIconWhite.png"}; 
			descriptionEntry.entry.Placeholder = "Description";
			descriptionEntry.HeightRequest = textEntriesHeight;

			categoryEntry = new InputView ();
			categoryEntry.entry.TextColor = Color.White;
			categoryEntry.entry.PlaceholderColor = Color.White;
			categoryEntry.bottomLine.BackgroundColor = Color.White;
			categoryEntry.image.Source = new FileImageSource () { File = "categoryIconWhite.png"}; 
			categoryEntry.entry.Placeholder = "Category";
			categoryEntry.HeightRequest = textEntriesHeight;

			beaconEntry = new InputView ();
			beaconEntry.entry.TextColor = Color.White;
			beaconEntry.entry.PlaceholderColor = Color.White;
			beaconEntry.bottomLine.BackgroundColor = Color.White;
			beaconEntry.image.Source = new FileImageSource () { File = "hardwareIconWhite.png"}; 
			beaconEntry.entry.Placeholder = "Beacon";
			beaconEntry.HeightRequest = textEntriesHeight;

			HeaderView addressHeader = new HeaderView() {
				Title = "Address Info",
				ImagesName = "circle.png",
				HeightRequest = textEntriesHeight
			};

			var bottomSeparatorView = new RelativeLayout () { 
				HeightRequest = 5
			};

			submitButton = new Button() {
				BackgroundColor = ColorMap.BlackTransparentBackground,
				TextColor = Color.White,
				BorderRadius = 3,
				BorderColor = Color.White,
				BorderWidth = 2,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				Text = "SUBMIT",
				HeightRequest = textEntriesHeight
			};

			var stackContentView = new StackLayout () {
				Padding = new Thickness(textEntriesPadding, 25, textEntriesPadding, 25),
				Spacing = 5,
				VerticalOptions = LayoutOptions.Start,
				Children = {
					imageContainer,
					generalInfoHeader,
					this.nameEntry,
					this.descriptionEntry,
					this.categoryEntry,
					this.beaconEntry,
					bottomSeparatorView,
					this.submitButton,
					new RelativeLayout() {
						HeightRequest = 15
					}
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

			// - Pickers
			this.categoryPicker = new Picker() {
				Title = "Select Category",
				BackgroundColor = ColorMap.DarkBlueColor
			};

			this.beaconPicker = new Picker () { 
				Title = "Select iBeacon",
				BackgroundColor = ColorMap.DarkBlueColor
			};

			var pickerHeight = 40;

			rootRelativeLayout.Children.Add (
				categoryPicker,
				Constraint.Constant(0),
				Constraint.RelativeToParent( parent => {
					return parent.Height - pickerHeight;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Width;
				}),
				Constraint.Constant(pickerHeight)
			);

			rootRelativeLayout.Children.Add (
				beaconPicker,
				Constraint.Constant(0),
				Constraint.RelativeToParent( parent => {
					return parent.Height - pickerHeight;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Width;
				}),
				Constraint.Constant(pickerHeight)
			);

			this.categoryPicker.IsVisible = false;
			this.beaconPicker.IsVisible = false;
			this.loadingView = new LoadingView ();
			loadingView.AddTo (rootRelativeLayout);

			this.Content = rootRelativeLayout;
		}

		#endregion
	}
}

