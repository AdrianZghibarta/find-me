using System;
using Xamarin.Forms;

namespace Findme
{
	public class NoDataView: RelativeLayout
	{
		public NoDataView ()
		{
			var infoImage = new Image () { 
				Aspect = Aspect.AspectFit,
				Source = new FileImageSource() {
					File = "infoIcon.png"
				}
			};

			var noDataLabel = new Label () {

				TextColor = Color.White,
				FontSize = 20,
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center,
				Text = "No Data"
			};

			this.Children.Add (
				infoImage,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent( parent => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Height * 0.7;
				})
			);

			this.Children.Add (
				noDataLabel,
				Constraint.Constant(0),
				Constraint.RelativeToParent( parent => {
					return parent.Height * 0.8;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Height * 0.2;
				})
			);

			this.IsVisible = false;
		}

		/// <summary>
		/// Adds to another relative layout and center him.
		/// </summary>
		/// <param name="relativeLayout">Relative layout.</param>
		public void AddTo(RelativeLayout relativeLayout) {

			var width = 150;
			var height = 120;
			relativeLayout.Children.Add (
				this,
				Constraint.RelativeToParent( (parent) => {
					return (parent.Width - width) / 2;
				}),
				Constraint.RelativeToParent( (parent) => {
					return (parent.Width - height) / 2;
				}),
				Constraint.Constant(width),
				Constraint.Constant(height)
			);
		}

		/// <summary>
		/// Show the No data view.
		/// </summary>
		public void Show() {

			Device.BeginInvokeOnMainThread ( () => {
				
				this.IsVisible = true;
			});
		}

		/// <summary>
		/// Hide the No data view.
		/// </summary>
		public void Hide() {

			Device.BeginInvokeOnMainThread ( () => {
				
				this.IsVisible = false;
			});
		}
	}
}

