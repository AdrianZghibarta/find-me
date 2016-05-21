using System;
using Findme;
using Findme.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(UnderlinedEntry), typeof(UnderlinedEntryRenderer))]

namespace Findme.iOS
{
	public class UnderlinedEntryRenderer: EntryRenderer
	{
		private UIView bottomView = new UIView();

		public UnderlinedEntryRenderer ()
		{
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);
			Console.WriteLine ("Drawing");
			double lineHeight = 2;
			var size = this.Frame.Size;
			this.bottomView.Frame = new CGRect (0, size.Height - lineHeight, size.Width, lineHeight);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);
			if (Control != null) {
				UnderlinedEntry ue = (UnderlinedEntry)this.Element;
				if (ue != null) {
					this.bottomView.BackgroundColor = ue.BottomLineColor.ToUIColor();
					//Control.AddSubview (this.bottomView);
					Control.BorderStyle = UITextBorderStyle.None;
				}
			}

		}
	}
}

