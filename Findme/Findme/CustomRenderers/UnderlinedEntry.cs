using System;
using Xamarin.Forms;

namespace Findme
{
	public class UnderlinedEntry : Entry
	{
		public UnderlinedEntry ()
		{
		}

		// - Bindable propriety for the Bottom Line Color
		public static readonly BindableProperty BottomLineColorPropriety = 
			BindableProperty.Create<UnderlinedEntry, Color> (
				p => p.BottomLineColor, default(Color)
			); 

		public Color BottomLineColor {
			get { return (Color)GetValue (BottomLineColorPropriety);}
			set { SetValue (BottomLineColorPropriety, value);}
		}
	}
}

