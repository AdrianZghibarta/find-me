using System;
using Xamarin.Forms;

namespace Findme
{
	public class LoadingView: RelativeLayout
	{
		public LoadingView ()
		{
			this.BackgroundColor = ColorMap.LoadingViewBackground;
			this.Children.Add(
				new ActivityIndicator {
					Color = Color.White,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					IsRunning = true
				},
				Constraint.RelativeToParent( (parent) => {
					return (parent.Width - 20) / 2;
				}),
				Constraint.RelativeToParent( (parent) => {
					return (parent.Height - 20) / 2;
				}),
				Constraint.Constant(20),
				Constraint.Constant(20)
			);
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
	}
}

