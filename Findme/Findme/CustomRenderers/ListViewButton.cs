using System;
using Xamarin.Forms;

namespace Findme
{
	public class ListViewButton: Button
	{
		public ListViewButton ()
		{
			BackgroundColor = ColorMap.DarkTransparentBlueColor;
			TextColor = ColorMap.DarkBlueColor;
			BorderRadius = 3;
			BorderColor = ColorMap.DarkBlueColor;
			BorderWidth = 2;
			FontSize = 16;
			FontAttributes = FontAttributes.Bold;
		}
	}
}

