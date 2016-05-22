using System;
using Xamarin.Forms;

namespace Findme
{
	public class LoadingView: RelativeLayout
	{

		private ActivityIndicator loadingIndicator = new ActivityIndicator();

		public LoadingView ()
		{

			this.loadingIndicator = new ActivityIndicator {
				Color = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				IsRunning = false
			};

			this.BackgroundColor = ColorMap.LoadingViewBackground;
			this.Children.Add(
				this.loadingIndicator,
				Constraint.RelativeToParent( (parent) => {
					return (parent.Width - 20) / 2;
				}),
				Constraint.RelativeToParent( (parent) => {
					return (parent.Height - 20) / 2;
				}),
				Constraint.Constant(20),
				Constraint.Constant(20)
			);
			this.IsVisible = false;
		}

		public void AddTo(RelativeLayout relativeLayout) {

			relativeLayout.Children.Add (
				this,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent( (parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( (parent) => {
					return parent.Height;
				})
			);
		}

		/// <summary>
		/// Show the loading indicator.
		/// </summary>
		public void Show() {

			Device.BeginInvokeOnMainThread ( () => {

				this.loadingIndicator.IsRunning = true;
				this.IsVisible = true;
			});
		}

		/// <summary>
		/// Hide the loading inidcator.
		/// </summary>
		public void Hide() {

			Device.BeginInvokeOnMainThread ( () => {

				this.loadingIndicator.IsRunning = false;
				this.IsVisible = false;
			});
		}
	}
}

