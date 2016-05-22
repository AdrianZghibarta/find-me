using Xamarin.Forms;

namespace Findme {

	public class MenuCell :ViewCell
	{

		public Image imageView;
		public Label titleLabel;

		public MenuCell ()
		{
			this.imageView = new Image {
				HeightRequest = 25,
				WidthRequest = 25,
			};
			imageView.SetBinding (Image.SourceProperty, "IconSource");

			titleLabel = new Label {
				TextColor = ColorMap.LightGrayColor,
				HeightRequest = 35,
				FontSize = 18,
				VerticalTextAlignment = TextAlignment.Center,

			};
			titleLabel.SetBinding (Label.TextProperty, "Title");

			var layout = new StackLayout {
				Padding = new Thickness (20, 0, 0, 0),
				Spacing = 10,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Children = { imageView, titleLabel }
			};

			View = layout;
		}
	}
}