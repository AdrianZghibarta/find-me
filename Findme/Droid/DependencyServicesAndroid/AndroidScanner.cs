using System;
using Findme.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidScanner))]
namespace Findme.Droid
{
	public class AndroidScanner: IScannerInterface
	{
		public AndroidScanner ()
		{
		}

		public void StartScanner() {

			MainActivity.sharedInstance.StartScannerService ();
		}
		public void StopScanner() {

			MainActivity.sharedInstance.StopScannerService ();
		}
	}
}

