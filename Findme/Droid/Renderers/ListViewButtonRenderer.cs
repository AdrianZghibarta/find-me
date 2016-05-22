using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Findme;
using Findme.Droid;

[assembly: ExportRenderer (typeof (ListViewButton), typeof (ListViewButtonRenderer))]
namespace Findme.Droid
{
	public class ListViewButtonRenderer: ButtonRenderer
	{
		public ListViewButtonRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged (e);

			// - This fix the bug with unselectable cells for android apps
			Control.Focusable = false;
		}
	}
}

