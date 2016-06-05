using System;
using Xamarin.Forms;
using XLabs.Ioc;
using System.Threading.Tasks;
using XLabs.Platform.Services.Media;
using System.Collections.Generic;
using System.Linq;

namespace Findme
{
	public class EditItemPage: ContentPage
	{
		public EditItemPage ()
		{
			this.Title = "Add Item";
			this.setUpOnlyLoadingIndicator ();
			this.getCategoryesAndBeacons ();
		}

		public EditItemPage (Item item) {

			this.Title = "Edit Item";
			this.setUpOnlyLoadingIndicator ();
			this.itemToEdit = item;
			this.getCategoryesAndBeacons ();
		}

		private void setUpOnlyLoadingIndicator() {

			var rootLayout = new RelativeLayout () { 
				BackgroundColor = ColorMap.GreenColor
			};
			this.loadingView = new LoadingView ();
			this.loadingView.AddTo (rootLayout);
			this.Content = rootLayout;
		}

		private void setUpAllLayout() {

			Device.BeginInvokeOnMainThread ( () => {
				this.setUpLayout ();
				this.setButtonHandlers ();
				this.updateUIForItem ();
			});
		}

		#region -> UI Elements

		// - Registration Elements
		Image itemImage;
		InputView nameEntry;
		InputView descriptionEntry;
		Button selectImageButton;
		LoadingView loadingView;
		Button detachButton;
		Button submitButton;
		Picker categoryPicker;
		Picker beaconPicker;
		Picker statusPicker;
		#endregion

		#region -> Proprietes

		private String imageInBase64;
		private bool hasCategories;
		private bool hasBeacons;
		private List<FMBeacon> beaconsList;
		private List<Category> categoriesList;
		public Item itemToEdit;
		public Item itemToAdd = new Item();
		public ItemsListPage itemListPage;

		#endregion

		#region -> Functionalities

		public void updateUIForItem() {

			if (this.itemToEdit != null) {
				this.nameEntry.entry.Text = this.itemToEdit.name;
				this.descriptionEntry.entry.Text = this.itemToEdit.description;
				var photoUrl = NetworkingUrls.BASE_URL + this.itemToEdit.photoUrl;
				this.itemImage.Source = ImageSource.FromUri(new Uri(photoUrl));	
			}
		}

		private void getCategoryesAndBeacons() {

			this.loadingView.Show ();
			BeaconsManager.SharedInstance.GetUserBeacons (BeaconsManager.AvailableBeaconType).ContinueWith ( task => {

				FindMeResponse response = (FindMeResponse)task.Result;

				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {
					this.beaconsList = new List<FMBeacon>();
					if (this.itemToEdit != null) {
						if (this.itemToEdit.beacon != null) {
							this.beaconsList.Add(this.itemToEdit.beacon);
						}
					}
					var availableBeacons = (List<FMBeacon>)response.Result;
					foreach (FMBeacon beacon in availableBeacons) {
						this.beaconsList.Add(beacon);
					}

					this.hasBeacons = true;
					if (this.hasCategories) {
						this.loadingView.Hide();
						this.setUpAllLayout();
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
						this.setUpAllLayout();
					}
				}
			});
		}

		public bool isDataValid() {

			if (this.categoryPicker.SelectedIndex == -1 || this.beaconPicker.SelectedIndex == -1 || this.statusPicker.SelectedIndex == -1) {
				return false;
			}

			Entry[] entriesToValidate = new Entry[] {
				nameEntry.entry, 
				descriptionEntry.entry
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

				this.loadingView.Show();
				// - If it's all ok then perform the requst
				if (this.itemToEdit != null ) {
					// We need to edit the item
					this.itemToEdit.name = this.nameEntry.entry.Text;
					this.itemToEdit.description = this.descriptionEntry.entry.Text;

					ItemsManager.SharedInstance.EditItem(this.itemToEdit, this.imageInBase64).ContinueWith( task => {

						this.loadingView.Hide();
						FindMeResponse response = (FindMeResponse)task.Result;
						if (null != response.ErrorInfo) {
							Device.BeginInvokeOnMainThread( () => {
								this.DisplayAlert("Error", response.ErrorInfo, "Ok");
							});
						}
						else {
							Device.BeginInvokeOnMainThread( () => {
								this.itemListPage.itemsListView.BeginRefresh();
								Navigation.PopAsync();
							});
						}
					});
				}
				else {

					// We need to add the item
					this.itemToAdd.name = this.nameEntry.entry.Text;
					this.itemToAdd.description = this.descriptionEntry.entry.Text;

					ItemsManager.SharedInstance.AddItem(this.itemToAdd, this.imageInBase64).ContinueWith( task => {

						this.loadingView.Hide();
						FindMeResponse response = (FindMeResponse)task.Result;
						if (null != response.ErrorInfo) {
							Device.BeginInvokeOnMainThread( () => {
								this.DisplayAlert("Error", response.ErrorInfo, "Ok");
							});
						}
						else {
							Device.BeginInvokeOnMainThread( () => {
								this.itemListPage.itemsListView.BeginRefresh();
								Navigation.PopAsync();
							});
						}
					});
				}
			};

			this.selectImageButton.Clicked += (object sender, EventArgs e) => {

				this.selectPhoto().ContinueWith( task => {});
			};

			this.detachButton.Clicked += (object sender, EventArgs e) => {

				// - The request to detach the current iBeacon
				this.loadingView.Show();
				ItemsManager.SharedInstance.DetachiBeaconFromItem(this.itemToEdit).ContinueWith( task => {

					this.loadingView.Hide();
					FindMeResponse response = (FindMeResponse)task.Result;
					if (null != response.ErrorInfo) {
						Device.BeginInvokeOnMainThread( () => {
							this.DisplayAlert("Error", response.ErrorInfo, "Ok");
						});
					}
					else {
						Device.BeginInvokeOnMainThread( () => {
							this.itemListPage.itemsListView.BeginRefresh();
							this.beaconPicker.IsVisible = true;
							this.detachButton.IsVisible = false;
						});
					}
				});
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

			// - Set up the category picker --------------------------------------------
			foreach (Category category in this.categoriesList) {
				this.categoryPicker.Items.Add (category.name);
			}

			if (this.itemToEdit != null) {
				for (int i = 0; i < this.categoryPicker.Items.Count; i++) {

					if (this.categoryPicker.Items [i] == this.itemToEdit.category) {
						this.categoryPicker.SelectedIndex = i;
						break;
					}
				}
			} else {
				this.categoryPicker.SelectedIndex = -1;
			}

			this.categoryPicker.SelectedIndexChanged += (object sender, EventArgs e) => {
				if (this.itemToEdit != null) {
					this.itemToEdit.category = this.categoryPicker.Items[this.categoryPicker.SelectedIndex];
				} else {
					this.itemToAdd.category = this.categoryPicker.Items[this.categoryPicker.SelectedIndex];
				}
			};

			// - Set up the beacon picker -----------------------------------------------

			foreach (FMBeacon beacon in this.beaconsList) {
				this.beaconPicker.Items.Add (beacon.brand + ", " + beacon.range + " r. " + beacon.major + "/" + beacon.minor);
			}

			this.beaconPicker.SelectedIndex = -1;

			this.beaconPicker.SelectedIndexChanged += (object sender, EventArgs e) => {

				if (this.itemToEdit != null) {
					this.itemToEdit.beacon = this.beaconsList[this.beaconPicker.SelectedIndex];
				} else {
					this.itemToAdd.beacon = this.beaconsList[this.beaconPicker.SelectedIndex];
				}
			};

			// - Set up the status picker -----------------------------------------------

			this.statusPicker.Items.Add("Found");
			this.statusPicker.Items.Add("Loosed");

			if (this.itemToEdit != null) {
				if (this.itemToEdit.isLosed == true) {
					this.statusPicker.SelectedIndex = 1;
				} else {
					this.statusPicker.SelectedIndex = 0;
				}
			} else {
				this.statusPicker.SelectedIndex = -1;
			}

			this.statusPicker.SelectedIndexChanged += (object sender, EventArgs e) => {

				if (this.statusPicker.SelectedIndex == 0) {
					if (this.itemToEdit != null) {
						this.itemToEdit.isLosed = false;
					} else {
						this.itemToAdd.isLosed = false;
					}
				}
				else {
					if (this.itemToEdit != null) {
						this.itemToEdit.isLosed = true;
					} else {
						this.itemToAdd.isLosed = true;
					}
				}
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

			var placeholderImage = new ImageCircle () {
				Source = new FileImageSource () {
					File = "photoPlaceholder.png"
				},
				BorderColor = Color.Transparent,
				BorderWidth = 0,
				Aspect = Aspect.AspectFill
			};

			this.itemImage = new ImageCircle () {
				BorderColor = Color.White,
				BorderWidth = 2,
				Aspect = Aspect.AspectFill
			};

			this.selectImageButton = new Button () { 
				BackgroundColor = Color.Transparent,
				Text = ""
			};

			imageContainer.Children.Add (
				placeholderImage,
				Constraint.RelativeToParent( (parent) => {
					return (parent.Width - imageDim) / 2;
				}),
				Constraint.Constant(0),
				Constraint.Constant(imageDim),
				Constraint.Constant(imageDim)
			);

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

			HeaderView pickerHeader = new HeaderView() {
				Title = "Picker Values",
				ImagesName = "circle.png",
				HeightRequest = textEntriesHeight
			};

			// - Pickers
			this.categoryPicker = new Picker() {
				Title = "Select Category",
				HeightRequest = textEntriesHeight,
				BackgroundColor = ColorMap.BlackTransparentBackground,
			};

			this.statusPicker = new Picker () { 
				Title = "Select Status",
				HeightRequest = textEntriesHeight,
				BackgroundColor = ColorMap.BlackTransparentBackground
			};

			this.beaconPicker = new Picker () { 
				Title = "Select iBeacon",
				HeightRequest = textEntriesHeight,
				BackgroundColor = ColorMap.BlackTransparentBackground,
			};

			this.detachButton = new Button() {
				BackgroundColor = ColorMap.RedTransparentBackground,
				TextColor = Color.White,
				BorderRadius = 3,
				BorderColor = Color.White,
				BorderWidth = 2,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				Text = "Detach iBeacon",
				HeightRequest = textEntriesHeight
			};

			var iBeaconContainer = new RelativeLayout () {
				HeightRequest = textEntriesHeight
			};

			if (this.itemToEdit != null && this.itemToEdit.beacon != null) {
				this.detachButton.IsVisible = true;
				this.beaconPicker.IsVisible = false;
			} else {
				this.detachButton.IsVisible = false;
				this.beaconPicker.IsVisible = true;
			}

			iBeaconContainer.Children.Add (
				beaconPicker,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent( parent => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Height;
				})
			);

			iBeaconContainer.Children.Add (
				detachButton,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent( parent => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Height;
				})
			);

			this.setUpPickers ();

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
					pickerHeader,
					this.statusPicker,
					this.categoryPicker,
					iBeaconContainer,
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

			this.loadingView = new LoadingView ();
			loadingView.AddTo (rootRelativeLayout);

			this.Content = rootRelativeLayout;
		}

		#endregion
	}
}

