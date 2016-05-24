using System;
using Findme.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(iPhoneScanner))]
namespace Findme.iOS
{
	public class iPhoneScanner: IScannerInterface
	{
		public iPhoneScanner ()
		{
		}

		public void StartScanner() {

			ScannerService.SharedInstance.StartScannerService ();
		}
		public void StopScanner() {

			ScannerService.SharedInstance.StopScannerService ();
		}
	}
}

