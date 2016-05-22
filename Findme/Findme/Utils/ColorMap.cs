using System;
using Xamarin.Forms;

namespace Findme
{
	public class ColorMap
	{	
		/// <summary>
		/// The color of the blue.
		/// </summary>
		public static Color DarkBlueColor = Color.FromHex("2C3E50");

		/// <summary>
		/// The color of the blue.
		/// </summary>
		public static Color DarkTransparentBlueColor = DarkBlueColor.MultiplyAlpha(0.3);

		/// <summary>
		/// The color of the green.
		/// </summary>
		public static Color GreenColor = Color.FromHex("18BC9C");

		/// <summary>
		/// The color of the light gray.
		/// </summary>
		public static Color LightGrayColor = Color.FromHex("AAAAAA");

		/// <summary>
		/// The black transparent background.
		/// </summary>
		public static Color BlackTransparentBackground = Color.FromRgba(0.3, 0.3, 0.3, 0.1);

		/// <summary>
		/// The red transparent background.
		/// </summary>
		public static Color RedBackgroundColor = Color.FromHex("F34235");

		/// <summary>
		/// The red transparent background.
		/// </summary>
		public static Color RedTransparentBackground = RedBackgroundColor.MultiplyAlpha(0.3);

		/// <summary>
		/// The loading view background.
		/// </summary>
		public static Color LoadingViewBackground = Color.FromRgba(0.3, 0.3, 0.3, 0.5);


		public ColorMap () {}
	}
}

