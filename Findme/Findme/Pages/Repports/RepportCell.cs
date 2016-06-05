using System;
using Xamarin.Forms;

namespace Findme
{
	public class RepportCell: ViewCell
	{
		public RepportCell ()
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

			// - Price and quantity fields
			var userNameImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "userDarkIcon.png" 
				}
			};

			var userNameLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center
			};
			userNameLabel.SetBinding (Label.TextProperty, "UserNameValue");

			var userNameStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					userNameImage,
					userNameLabel
				}
			};

			// - User email fields
			var emailImage = new Image {
				HeightRequest = 15,
				WidthRequest = 15,
				Source = new FileImageSource() { 
					File = "emailDarkIcon.png" 
				}
			};

			var emailLabel = new Label {
				TextColor = ColorMap.DarkBlueColor,
				HeightRequest = 25,
				FontSize = 16,
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Center
			};
			emailLabel.SetBinding (Label.TextProperty, "EmailValue");

			var emailStackLayout = new StackLayout() {

				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				Spacing = 10,
				Children = {
					emailImage,
					emailLabel
				}
			};

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

			// - The fields stack container

			var verticalContainer = new StackLayout {

				Padding = new Thickness (0, 0, 0, 0),
				Spacing = 2,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Start,
				Children = { 
					userNameStackLayout,
					emailStackLayout,
					dateStackContent
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

