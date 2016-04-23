using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;
using Findme;
using Findme.iOS;

[assembly: ExportRenderer(typeof(RoundedBoxView), typeof(RoundedBoxRenderer))]

namespace Findme.iOS
{
	public class RoundedBoxRenderer : BoxRenderer
	{
		public RoundedBoxRenderer ()
		{
		}

		public override void Draw (CoreGraphics.CGRect rect)
		{
			RoundedBoxView rbv = (RoundedBoxView)this.Element;

			using (var context = UIGraphics.GetCurrentContext()) {

				context.SetFillColor (rbv.Color.ToCGColor ());
				context.SetStrokeColor (rbv.OutlineColor.ToCGColor ());
				context.SetLineWidth ((float)rbv.OutlineWidth);

				var rc = this.Bounds.Inset (5, 5);

				float radius = (float)rbv.CornerRadius;
				radius = (float)Math.Max (0, Math.Min (radius, Math.Max (rc.Height / 2, rc.Width / 2)));
				var path = CGPath.FromRoundedRect (rc, radius, radius);
				context.AddPath (path);
				context.DrawPath (CGPathDrawingMode.FillStroke);

			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == RoundedBoxView.OutlineColorPropriety.PropertyName
			    || e.PropertyName == RoundedBoxView.CornerRadiusPropriety.PropertyName) {
				this.SetNeedsDisplay ();
			}
		}
	}
}

