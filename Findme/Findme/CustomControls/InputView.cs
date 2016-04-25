using System;
using Xamarin.Forms;

namespace Findme
{
	public class InputView: RelativeLayout
	{
		// - UI Elements
		public Image image = new Image();
		public BoxView bottomLine = new BoxView();
		public UnderlinedEntry entry = new UnderlinedEntry();

		public InputView ()
		{
			this.setLayout ();
		}

		private void setLayout() {

			// - Dimensions
			double imagePadding = 5;
			double imageWidth = 15;
			double bottomLineHeight = 2;

			// - Image customisation

			image.Aspect = Aspect.AspectFit;

			// - Entry Customisation

			entry.BackgroundColor = Color.Transparent;

			// - Add the subviews to the relative layout

			this.Children.Add (
				image,
				Constraint.Constant(imagePadding),
				Constraint.Constant(imagePadding),
				Constraint.Constant(imageWidth),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height - imagePadding * 2 - bottomLineHeight;
				})
			);

			this.Children.Add (
				bottomLine,
				Constraint.Constant(0),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height - bottomLineHeight;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width;
				}),
				Constraint.Constant(bottomLineHeight)
			);

			this.Children.Add (
				entry,
				Constraint.Constant(imagePadding * 2 + imageWidth),
				Constraint.Constant(bottomLineHeight),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width - imagePadding * 3 - imageWidth;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height - bottomLineHeight * 2;
				})
			);
		}
	}
}

