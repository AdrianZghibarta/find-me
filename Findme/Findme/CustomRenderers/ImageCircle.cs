using System;
using Xamarin.Forms;

namespace Findme
{
	public class ImageCircle: Image
	{
		public ImageCircle ()
		{
		}

		// - Bindable propriety for the border color
		public static readonly BindableProperty BorderColorPropriety = 
			BindableProperty.Create<ImageCircle, Color> (
				p => p.BorderColor, default(Color)
			); 

		public Color BorderColor {
			get { return (Color)GetValue (BorderColorPropriety);}
			set { SetValue (BorderColorPropriety, value);}
		}

		// - Bindable propriety for the border width
		public static readonly BindableProperty BorderWidthPropriety = 
			BindableProperty.Create<ImageCircle, float> (
				p => p.BorderWidth, default(float)
			); 

		public float BorderWidth {
			get { return (float)GetValue (BorderWidthPropriety);}
			set { SetValue (BorderWidthPropriety, value);}
		}
	}
}

