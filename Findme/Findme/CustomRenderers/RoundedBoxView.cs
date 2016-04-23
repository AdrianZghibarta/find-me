using System;
using Xamarin.Forms;

namespace Findme
{
	public class RoundedBoxView : BoxView
	{
		public RoundedBoxView ()
		{
		}

		// - Bindable propriety for the corner radius
		public static readonly BindableProperty CornerRadiusPropriety = 
			BindableProperty.Create<RoundedBoxView, double> (
				p => p.CornerRadius, default(double)
			); 

		public double CornerRadius {
			get { return (double)GetValue (CornerRadiusPropriety);}
			set { SetValue (CornerRadiusPropriety, value);}
		}


		// - Bindable prorpiety for the outline color
		public static readonly BindableProperty OutlineColorPropriety = 
			BindableProperty.Create<RoundedBoxView, Color> (
				p => p.OutlineColor, default(Color)
			); 

		public Color OutlineColor {
			get { return (Color)GetValue (OutlineColorPropriety);}
			set { SetValue (OutlineColorPropriety, value);}
		}

		// - Bindable propriety for the outline width
		public static readonly BindableProperty OutlineWidthPropriety = 
			BindableProperty.Create<RoundedBoxView, double> (
				p => p.OutlineWidth, default(double)
			); 

		public double OutlineWidth {
			get { return (double)GetValue (OutlineWidthPropriety);}
			set { SetValue (OutlineWidthPropriety, value);}
		}
	}
}

