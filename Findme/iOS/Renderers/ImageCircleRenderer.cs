using System;
using Findme;
using Findme.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ImageCircle), typeof(ImageCircleRenderer))]
namespace Findme.iOS
{
	public class ImageCircleRenderer: ImageRenderer
	{
		public ImageCircleRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
				return;

			CreateCircle();

		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
				e.PropertyName == VisualElement.WidthProperty.PropertyName ||
				e.PropertyName == ImageCircle.BorderColorPropriety.PropertyName ||
				e.PropertyName == ImageCircle.BorderWidthPropriety.PropertyName)
			{
				CreateCircle();
			}
		}

		private void CreateCircle()
		{
			
			try
			{
				ImageCircle imageCircle = (ImageCircle)this.Element;
				double min = Math.Min(Element.Width, Element.Height);
				Control.Layer.CornerRadius = (float)(min / 2.0);
				Control.Layer.MasksToBounds = false;
				Control.Layer.BorderColor = imageCircle.BorderColor.ToCGColor();
				Control.Layer.BorderWidth = imageCircle.BorderWidth;
				Control.ClipsToBounds = true;
			}
			catch(Exception ex)
			{
				Console.WriteLine ("Unable to create circle image: " + ex);
			}
		}
	}
}

