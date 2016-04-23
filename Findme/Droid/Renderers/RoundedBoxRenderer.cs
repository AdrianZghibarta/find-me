using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Findme;
using Findme.Droid;

[assembly: ExportRenderer(typeof(RoundedBoxView), typeof(RoundedBoxRenderer))]

namespace Findme.Droid
{
	public class RoundedBoxRenderer : BoxRenderer
	{
		public RoundedBoxRenderer ()
		{
			this.SetWillNotDraw (false);
		}

		public override void Draw (Canvas canvas)
		{
			RoundedBoxView rbv = (RoundedBoxView)this.Element;

			Rect rc = new Rect();
			GetDrawingRect(rc);

			Rect interior = rc;
			interior.Inset((int)rbv.OutlineWidth, (int)rbv.OutlineWidth);

			Paint p = new Paint() {
				Color = rbv.Color.ToAndroid(),
				AntiAlias = true,
			};

			canvas.DrawRoundRect(new RectF(interior), (float)rbv.CornerRadius, (float)rbv.CornerRadius, p);

			p.Color = rbv.OutlineColor.ToAndroid();
			p.StrokeWidth = (float)rbv.OutlineWidth;
			p.SetStyle(Paint.Style.Stroke);

			canvas.DrawRoundRect(new RectF(rc), (float)rbv.CornerRadius, (float)rbv.CornerRadius, p);
		}

	}
}

