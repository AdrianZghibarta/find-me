using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Findme
{
	public class ScanPage: ContentPage
	{
		public ScanPage ()
		{
			this.Title = "Scan Page";
			ScannerManager.SharedInstance.didFoundiBeaconsDelegate = ( ibeaconsList => {

				this.ProcessiBeaconList(ibeaconsList);
			});
		}

		override protected void OnAppearing() {

			base.OnAppearing ();
			ScannerManager.SharedInstance.StartScannerForCurrentPlatform ();
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			ScannerManager.SharedInstance.StopScannerForCurrentPalatform ();
		}

		private void ProcessiBeaconList(List<FMBeacon> iBeaconsList) {

			foreach (FMBeacon beacon in iBeaconsList) {

				ConsoleOutput.PrintLine ("New iBeacon found: " + beacon.uuid + "/" + 
					beacon.major + "/" + beacon.minor + " >> " + beacon.proximity.ToString());
			}
		}
	}
}

