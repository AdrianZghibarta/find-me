using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//using XLabs.Forms;
//using System.IO;
//using XLabs;
//using XLabs.Ioc;
//using XLabs.Platform.Services.Media;

namespace Findme.Droid
{
	[Activity (Label = "Findme.Droid", 
		Icon = "@drawable/icon", 
		MainLauncher = true, 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		WindowSoftInputMode = SoftInput.AdjustPan)]

	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity//XFormsApplicationDroid
	{
		public static MainActivity sharedInstance;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

//			if (!Resolver.IsSet)
//				this.SetIoc ();

			LoadApplication (new App ());

			MainActivity.sharedInstance = this;
		}

//		private void SetIoc()
//		{
//			var resolverContainer = new SimpleContainer();
//			resolverContainer.Register<IMediaPicker, MediaPicker> ();
//			Resolver.SetResolver(resolverContainer.GetResolver());
//		}
	}
}

