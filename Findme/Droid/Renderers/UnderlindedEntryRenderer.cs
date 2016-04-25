using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Findme;
using Findme.Droid;

[assembly: ExportRenderer(typeof(UnderlinedEntry), typeof(UnderlindedEntryRenderer))]

namespace Findme.Droid
{
	public class UnderlindedEntryRenderer: EntryRenderer
	{
		public UnderlindedEntryRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			// - If you need add the bottom line logic.
			if (Control != null) {
				Control.SetBackgroundColor (Xamarin.Forms.Color.Transparent.ToAndroid ());
			}
		}
	}
}

