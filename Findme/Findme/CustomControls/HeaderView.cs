using System;
using Xamarin.Forms;

namespace Findme
{
	public class HeaderView : RelativeLayout
	{
		private Image leftImage = new Image();
		private Image rightImage = new Image();
		private Label titleLabel = new Label();

		public String Title {
			set { 
				this.titleLabel.Text = value;
			}
			get { 
				return this.titleLabel.Text;
			}
		}

		public String ImagesName {
			set {
				leftImage.Source = new FileImageSource () { File = value };
				rightImage.Source = new FileImageSource () { File = value };
			}
		}

		public HeaderView ()
		{
			double horizontalPadding = 20;
			double verticalSpacing = 5;
			double horizontalSpacing = 10;
			double imageWidth = 15;

			leftImage.Aspect = rightImage.Aspect = Aspect.AspectFit;

			titleLabel.HorizontalTextAlignment = TextAlignment.Center;
			titleLabel.VerticalTextAlignment = TextAlignment.Center;
			titleLabel.FontAttributes = FontAttributes.Bold;
			titleLabel.TextColor = Color.White;

			this.Children.Add (
				titleLabel,
				Constraint.Constant(horizontalPadding + imageWidth + horizontalSpacing),
				Constraint.Constant(verticalSpacing),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width - horizontalPadding * 2 - horizontalSpacing * 2 - imageWidth * 2;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height - verticalSpacing * 2;
				})
			);

			this.Children.Add (
				leftImage,
				Constraint.Constant(horizontalPadding),
				Constraint.Constant(verticalSpacing),
				Constraint.Constant(imageWidth),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height - verticalSpacing * 2;
				})
			);

			this.Children.Add (
				rightImage,
				Constraint.RelativeToParent( (parent) => {
					return parent.Width - horizontalPadding - imageWidth;
				}),
				Constraint.Constant(verticalSpacing),
				Constraint.Constant(imageWidth),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height - verticalSpacing * 2;
				})
			);
		}
	}
}

