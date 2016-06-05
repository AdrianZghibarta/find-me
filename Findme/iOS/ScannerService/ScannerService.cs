using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Foundation;
using UIKit;
using CoreBluetooth;
using CoreLocation;
using CoreFoundation;
using AVFoundation;
using MultipeerConnectivity;

namespace Findme.iOS
{
	public class ScannerService
	{
		CLLocationManager locationManager;
		CLBeaconRegion beaconRegion;

		// - BluetoothStateChangeCounter is used to see how many times the bluetooth state was changed
		private int bluetoothStateChangeCounter = 0;

		public static ScannerService singletonInstance;
		/// <summary>
		/// Gets the shared instance.
		/// </summary>
		/// <value>The shared instance.</value>
		public static ScannerService SharedInstance
		{
			get 
			{
				if (singletonInstance == null)
				{
					singletonInstance = new ScannerService ();
				}
				return singletonInstance;
			}
		}

		public ScannerService ()
		{
			this.InitializeCentralManager ();
			this.InitializeiBeaconLocationService ();
		}

		private void InitializeCentralManager() {

			//Bluetooth state check
			CBCentralManager centralManager = new CBCentralManager();
			centralManager.UpdatedState += (object sender, EventArgs e) => {
				Console.WriteLine("Bluetooth state changed to : " + centralManager.State.ToString());
				if (centralManager.State == CBCentralManagerState.PoweredOff && this.bluetoothStateChangeCounter != 0)
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Bluetooth state is OFF",
						Message = "Please turn on the bluetooth in settings"
					};
					alert.AddButton("OK");
					alert.Show();
				}

				this.bluetoothStateChangeCounter++;
			};
		}

		private void InitializeiBeaconLocationService() {
			
			string uuid = ScannerManager.FindMeUUID;
			string scannerId = ScannerManager.ScanerID;

			var scannerUUID = new NSUuid(uuid);

			beaconRegion = new CLBeaconRegion(scannerUUID, scannerId);
			beaconRegion.NotifyEntryStateOnDisplay = true;
			beaconRegion.NotifyOnEntry = true;
			beaconRegion.NotifyOnExit = true;

			locationManager = new CLLocationManager();
			locationManager.RequestAlwaysAuthorization();

			locationManager.DidRangeBeacons += (object sender, CLRegionBeaconsRangedEventArgs e) => {

				List<FMBeacon> iBeaconsList = new List<FMBeacon>();
				if (e.Beacons.Length > 0)
				{
					foreach (CLBeacon iBeacon in e.Beacons)
					{
						FMBeacon fmBeacon = new FMBeacon();

						fmBeacon.uuid = iBeacon.ProximityUuid.ToString();
						fmBeacon.major = iBeacon.Major.ToString();
						fmBeacon.minor = iBeacon.Minor.ToString();

						switch (iBeacon.Proximity)
						{
						case CLProximity.Immediate:
							Console.WriteLine ("Immediate Proximity Entered");
							fmBeacon.proximity = 1;
							break;

						case CLProximity.Near:
							Console.WriteLine ("Near Proximity Entered");
							fmBeacon.proximity = 2;
							break;

						case CLProximity.Far:
							Console.WriteLine ("Far Proximity Entered");
							fmBeacon.proximity = 3;
							break;

						case CLProximity.Unknown:
							Console.WriteLine ("Unknown Proximity Entered");
							fmBeacon.proximity = 4;
							break;
						}

						iBeaconsList.Add(fmBeacon);
					}
				}
				ScannerManager.SharedInstance.SetNewAvailableListOfiBeacons (iBeaconsList);
			};

			locationManager.RegionEntered += (object sender, CLRegionEventArgs e) => {
				// - Implement this if you need to handle when the beacon enter in region
			};
		}

		public void StartScannerService() {

			locationManager.StartMonitoring(beaconRegion);
			locationManager.StartRangingBeacons(beaconRegion);
			locationManager.StartUpdatingLocation();
		}

		public void StopScannerService() {

			locationManager.StopUpdatingLocation ();
			locationManager.StopRangingBeacons (beaconRegion);
			locationManager.StopMonitoring (beaconRegion);
		}
	}
}

