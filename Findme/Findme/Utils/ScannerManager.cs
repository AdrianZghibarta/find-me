using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Findme
{
	public delegate void DidFoundiBeacons(List<FMBeacon> beacons);

	public class ScannerManager
	{
		public static readonly String FindMeUUID = "AAAAAAAA-1111-2222-3333-FFFFFFFFFFFF";
		public static readonly String ScanerID = "FindMeIndentifier";

		public DidFoundiBeacons didFoundiBeaconsDelegate;

		public static ScannerManager singletonInstance;
		/// <summary>
		/// Gets the shared instance.
		/// </summary>
		/// <value>The shared instance.</value>
		public static ScannerManager SharedInstance
		{
			get 
			{
				if (singletonInstance == null)
				{
					singletonInstance = new ScannerManager ();
				}
				return singletonInstance;
			}
		}

		public ScannerManager ()
		{
		}

		/// <summary>
		/// Starts the scanner for current platform.
		/// </summary>
		public void StartScannerForCurrentPlatform() {

			DependencyService.Get<IScannerInterface> ().StartScanner ();
		}

		/// <summary>
		/// Stops the scanner for current palatform.
		/// </summary>
		public void StopScannerForCurrentPalatform() {

			DependencyService.Get<IScannerInterface> ().StopScanner ();
		}

		public void SetNewAvailableListOfiBeacons(List<FMBeacon> iBeaconsList) {

			if (this.didFoundiBeaconsDelegate != null) {

				this.didFoundiBeaconsDelegate (iBeaconsList);
			}
		}
	}
}

