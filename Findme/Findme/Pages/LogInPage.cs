using System;

using Xamarin.Forms;

namespace Findme
{
	public class LogInPage : ContentPage
	{
		public LogInPage ()
		{
			Content = new StackLayout { 
				Children = {
					new Label { Text = "Hello ContentPage" }
				}
			};
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			NetworkingManager.sharedInstance.googleRequest ().ContinueWith( (task) => {
				ConsoleOutput.PrintLine(task.Result);
			});
		}
	}
}


