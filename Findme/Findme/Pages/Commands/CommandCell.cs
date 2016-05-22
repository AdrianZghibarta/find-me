using System;
using Xamarin.Forms;

namespace Findme
{
	public class CommandCell :ViewCell
	{
		public CommandCell ()
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
			var dateImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "calendarIcon.png" 
				}
			};

			var dateLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center
			};
			dateLabel.SetBinding (Label.TextProperty, "DateValue");

			var dateStackContent = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					dateImage,
					dateLabel
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
			priceLabel.SetBinding (Label.TextProperty, "PriceAndQuantityValue");

			var priceStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					priceImage,
					priceLabel
				}
			};

			// - The confirm / cancel options flow

			// - Price and quantity fields
			var doneImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "doneIcon.png" 
				}
			};

			var doneLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center,
				Text = "Command is delivered"
			};

			var doneStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					doneImage,
					doneLabel
				}
			};
			doneStackLayout.SetBinding (StackLayout.IsVisibleProperty, "IsDelivered");

			var confirmButton = new ListViewButton () { 
				Text = "Confirm",
			};
			confirmButton.SetBinding (Button.CommandParameterProperty, "Id");
			confirmButton.SetBinding (Button.IsVisibleProperty, "NeedToShowActions");
			confirmButton.Clicked += (object sender, EventArgs e) => {

				var button = (Button)sender;
				var parameterToSend = (String)button.CommandParameter;

				int parentDepth = 10;
				Element currentParent = this.Parent;
				while (parentDepth > 0) {

					CommandsListPage commandListPage = currentParent as CommandsListPage;
					if (commandListPage != null) {
						commandListPage.ConfirmCommand(parameterToSend).ContinueWith( task => {} );
						break;
					} else {
						currentParent = currentParent.Parent;
						parentDepth--;
					}
				}
			};

			var cancelButton = new ListViewButton () { 
				TextColor = ColorMap.RedBackgroundColor,
				BorderColor = ColorMap.RedBackgroundColor,
				BackgroundColor = ColorMap.RedTransparentBackground,
				Text = "Cancel"
			};
			cancelButton.SetBinding (Button.CommandParameterProperty, "Id");
			cancelButton.SetBinding (Button.IsVisibleProperty, "NeedToShowActions");
			cancelButton.Clicked += (object sender, EventArgs e) => {

				var button = (Button)sender;
				var parameterToSend = (String)button.CommandParameter;

				int parentDepth = 10;
				Element currentParent = this.Parent;
				while (parentDepth > 0) {

					CommandsListPage commandListPage = currentParent as CommandsListPage;
					if (commandListPage != null) {
						commandListPage.CancelCommand(parameterToSend).ContinueWith( task => {} );
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
				doneStackLayout,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent( parent => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Height;
				})
			);

			actionFlowRelativeLayout.Children.Add (
				confirmButton,
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
				cancelButton,
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
				Spacing = 2,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Start,
				Children = { 
					dateStackContent,
					priceStackLayout,
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

