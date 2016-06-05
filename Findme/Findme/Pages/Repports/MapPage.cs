using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Findme
{
	public class MapPage: ContentPage
	{
		#region -> Proprietes

		private float Longitude;
		private float Latitude;

		#endregion

		public MapPage (float latitude, float longitude, String title, String description)
		{
			Title = "Map details";

			this.Latitude = latitude;
			this.Longitude = longitude;

			var map = new Map(
				MapSpan.FromCenterAndRadius(
					new Position(this.Latitude, this.Longitude), Distance.FromMiles(0.3))) {
				IsShowingUser = true,
				HeightRequest = 100,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			var stack = new StackLayout { Spacing = 0 };
			stack.Children.Add(map);


			var position = new Position(this.Latitude, this.Longitude); // Latitude, Longitude
			var pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = title,
				Address = description
			};
			map.Pins.Add(pin);

			Content = stack;
		}
	}
}

