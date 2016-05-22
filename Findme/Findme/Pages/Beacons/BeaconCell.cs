using System;
using Xamarin.Forms;

namespace Findme
{
	public class BeaconCell: ViewCell
	{
		public BeaconCell()
		{

			var relativeLayout = new RelativeLayout () {
				BackgroundColor = Color.Transparent
			};

			// - Add the background rounded box

			var roundedBackground = new RoundedBoxView () {
				Color = Color.White,
				CornerRadius = 4,
				OutlineWidth = 2,
				OutlineColor = ColorMap.DarkBlueColor
			};

			relativeLayout.Children.Add (
				roundedBackground,
				Constraint.Constant(10),
				Constraint.Constant(5),
				Constraint.RelativeToParent( parent => {
					return parent.Width - 20;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Height - 10;
				})
			);

			// - The content for all fields

			// - Date fields
			var generalInfoImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "info2Icon.png" 
				}
			};

			var generalInfoLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center
			};
			generalInfoLabel.SetBinding (Label.TextProperty, "GeneralInfo");

			var generalInfoStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					generalInfoImage,
					generalInfoLabel
				}
			};

			// - Price and quantity fields
			var priceImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "moneyIcon.png" 
				}
			};

			var priceLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center
			};
			priceLabel.SetBinding (Label.TextProperty, "PriceValue");

			var priceStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					priceImage,
					priceLabel
				}
			};

			// - Minor major fields
			var minorMajorImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "hardwareIcon.png" 
				}
			};

			var minorMajorLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center
			};
			minorMajorLabel.SetBinding (Label.TextProperty, "MinorMajorInfo");

			var minorMajorStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					minorMajorImage,
					minorMajorLabel
				}
			};

			// - Status field
			var statusImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "pinIcon.png" 
				}
			};

			var statusLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center
			};
			statusLabel.SetBinding (Label.TextProperty, "Status");

			var statusStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					statusImage,
					statusLabel
				}
			};


			// - Delete image
			var deleteImage = new Image {
				Source = new FileImageSource() { 
					File = "deleteIcon.png" 
				}
			};
			deleteImage.SetBinding (Image.IsVisibleProperty, "CanBeDeleted");

			// - Delete button
			var deleteButton = new ListViewButton () { 
				TextColor = Color.Transparent,
				BorderColor = Color.Transparent,
				BackgroundColor = Color.Transparent,
			};
			deleteButton.SetBinding (Button.CommandParameterProperty, "Id");
			deleteButton.SetBinding (Button.IsVisibleProperty, "CanBeDeleted");
			deleteButton.Clicked += (object sender, EventArgs e) => {

				var button = (Button)sender;
				var parameterToSend = (String)button.CommandParameter;

				int parentDepth = 10;
				Element currentParent = this.Parent;
				while (parentDepth > 0) {

					BeaconListPage commandListPage = currentParent as BeaconListPage;
					if (commandListPage != null) {
						commandListPage.DeleteBeacon(parameterToSend).ContinueWith( task => {} );
						break;
					} else {
						currentParent = currentParent.Parent;
						parentDepth--;
					}
				}
			};

			// - The fields stack container

			var verticalContainer = new StackLayout {

				Padding = new Thickness (0, 0, 0, 0),
				Spacing = 2,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Start,
				Children = { 
					generalInfoStackLayout,
					priceStackLayout,
					minorMajorStackLayout,
					statusStackLayout
				}
			};

			relativeLayout.Children.Add (

				verticalContainer,
				Constraint.Constant(30),
				Constraint.Constant(25),
				Constraint.RelativeToParent( parent => {
					return parent.Width - 60;
				}), 
				Constraint.RelativeToParent( parent => {
					return parent.Height - 55;
				})
			);

			var buttonDimensions = 30;

			relativeLayout.Children.Add (
				deleteImage,
				Constraint.RelativeToParent ( parent => {
					return parent.Width - buttonDimensions - 30;
				}),
				Constraint.Constant (30),
				Constraint.Constant (buttonDimensions),
				Constraint.Constant (buttonDimensions)
			);

			relativeLayout.Children.Add (
				deleteButton,
				Constraint.RelativeToParent ( parent => {
					return parent.Width - buttonDimensions - 30;
				}),
				Constraint.Constant (30),
				Constraint.Constant (buttonDimensions),
				Constraint.Constant (buttonDimensions)
			);

			this.View = relativeLayout;
		}
	}
}

