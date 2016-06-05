using System;
using Xamarin.Forms;

namespace Findme
{
	public class ItemCell: ViewCell
	{
		public ItemCell ()
		{

			var relativeLayout = new RelativeLayout () {
				BackgroundColor = Color.Transparent
			};

			// - Add the background rounded box

			var roundedBackground = new RoundedBoxView () {
				Color = Color.White,
				CornerRadius = 4,
				OutlineWidth = 2
			};
			roundedBackground.SetBinding (RoundedBoxView.OutlineColorPropriety, "StatusColor");

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

			// - Name fields
			var nameImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "nameIcon.png" 
				}
			};

			var nameLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center
			};
			nameLabel.SetBinding (Label.TextProperty, "Name");

			var nameStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					nameImage,
					nameLabel
				}
			};

			// - Description fields
			var descriptionImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "info2Icon.png" 
				}
			};

			var descriptionLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center
			};
			descriptionLabel.SetBinding (Label.TextProperty, "Description");

			var descriptionStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					descriptionImage,
					descriptionLabel
				}
			};

			// - Category fields
			var categoryImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "categoryIcon.png" 
				}
			};

			var categoryLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center
			};
			categoryLabel.SetBinding (Label.TextProperty, "Category");

			var categoryStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					categoryImage,
					categoryLabel
				}
			};

			// - Minor Major fields
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
			minorMajorLabel.SetBinding (Label.TextProperty, "MinorMajor");

			var minorMajorStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					minorMajorImage,
					minorMajorLabel
				}
			};

			// - The container for general information fields

			var generalInformationContainer = new StackLayout {

				Padding = new Thickness (0, 0, 0, 0),
				Spacing = 2,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Start,
				Children = { 
					nameStackLayout,
					descriptionStackLayout,
					categoryStackLayout,
					minorMajorStackLayout
				}
			};

			var placeholderImage = new ImageCircle () {
				Source = new FileImageSource () {
					File = "photoPlaceholder.png"
				},
				BorderColor = Color.Transparent,
				BorderWidth = 0,
				Aspect = Aspect.AspectFill
			};

			var itemImage = new ImageCircle () {
				Aspect = Aspect.AspectFill,
				BorderWidth = 3
			};
			itemImage.SetBinding (ImageCircle.BorderColorPropriety, "StatusColor");
			itemImage.SetBinding (ImageCircle.SourceProperty, "ItemImageSource");

			var itemImageContainer = new RelativeLayout () { 
				HeightRequest = 60,
				WidthRequest = 60
			};

			itemImageContainer.Children.Add (

				placeholderImage,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.Constant(50),
				Constraint.Constant(50)
			);

			itemImageContainer.Children.Add (

				itemImage,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.Constant(50),
				Constraint.Constant(50)
			);

			var upperContainer = new StackLayout () {

				Padding = new Thickness (0, 0, 0, 0),
				Spacing = 0,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Start,
				Children = { 
					itemImageContainer,
					generalInformationContainer
				}
			};

			// - The Delete / Show Repports options flow

			var deleteButton = new ListViewButton () { 
				TextColor = ColorMap.RedBackgroundColor,
				BorderColor = ColorMap.RedBackgroundColor,
				BackgroundColor = ColorMap.RedTransparentBackground,
				Text = "Delete",
			};
			deleteButton.SetBinding (Button.CommandParameterProperty, "Id");
			deleteButton.Clicked += (object sender, EventArgs e) => {

				var button = (Button)sender;
				var parameterToSend = (String)button.CommandParameter;

				int parentDepth = 10;
				Element currentParent = this.Parent;
				while (parentDepth > 0) {

					ItemsListPage itemListPage = currentParent as ItemsListPage;
					if (itemListPage != null) {
						itemListPage.DeleteItem(parameterToSend).ContinueWith( task => {} );
						break;
					} else {
						currentParent = currentParent.Parent;
						parentDepth--;
					}
				}
			};

			var showRepportsButton = new ListViewButton () {
				Text = "Repports"
			};
			showRepportsButton.SetBinding (Button.CommandParameterProperty, "Id");
			showRepportsButton.SetBinding (Button.IsVisibleProperty, "NeedToShowFoundRepports");
			showRepportsButton.Clicked += (object sender, EventArgs e) => {

				var button = (Button)sender;
				var parameterToSend = (String)button.CommandParameter;

				int parentDepth = 10;
				Element currentParent = this.Parent;
				while (parentDepth > 0) {

					ItemsListPage itemListPage = currentParent as ItemsListPage;
					if (itemListPage != null) {
						itemListPage.ShowFoundRepports(parameterToSend).ContinueWith( task => {} );
						break;
					} else {
						currentParent = currentParent.Parent;
						parentDepth--;
					}
				}
			};

			var actionFlowRelativeLayout = new RelativeLayout () { 
				HeightRequest = 25
			};

			actionFlowRelativeLayout.Children.Add (
				deleteButton,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent( parent => {
					return (parent.Width - 10) / 2;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Height + 8;
				})
			);

			actionFlowRelativeLayout.Children.Add (
				showRepportsButton,
				Constraint.RelativeToParent( parent => {
					return (parent.Width - 10) / 2 + 10;
				}),
				Constraint.Constant(0),
				Constraint.RelativeToParent( parent => {
					return (parent.Width - 10) / 2;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Height + 8;
				})
			);

			// - The fields stack container

			var verticalContainer = new StackLayout {

				Padding = new Thickness (0, 0, 0, 0),
				Spacing = 15,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Start,
				Children = { 
					upperContainer,
					actionFlowRelativeLayout
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

			this.View = relativeLayout;
		}
	}	
}

