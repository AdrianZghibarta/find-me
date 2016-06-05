using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Linq;
using RadiusNetworks.IBeaconAndroid;
using Color = Android.Graphics.Color;
using Android.Support.V4.App;
using Findme;

//using XLabs.Forms;
//using System.IO;
//using XLabs;
//using XLabs.Ioc;
//using XLabs.Platform.Services.Media;
using System.Collections.Generic;

namespace Findme.Droid
{
	[Activity (Label = "Findme.Droid", 
		Icon = "@android:color/transparent", 
		MainLauncher = true, 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		WindowSoftInputMode = SoftInput.AdjustPan)]

	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, IBeaconConsumer//XFormsApplicationDroid
	{
		public static MainActivity sharedInstance;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

//			if (!Resolver.IsSet)
//				this.SetIoc ();

			Xamarin.FormsMaps.Init (this, bundle);

			LoadApplication (new App ());

			MainActivity.sharedInstance = this;
		}

//		private void SetIoc()
//		{
//			var resolverContainer = new SimpleContainer();
//			resolverContainer.Register<IMediaPicker, MediaPicker> ();
//			Resolver.SetResolver(resolverContainer.GetResolver());
//		}

		protected override void OnResume ()
		{
			base.OnResume ();
			this.paused = false;
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			this.paused = true;
		}


		// - Scaner Logic

		bool paused;
		IBeaconManager iBeaconManager;
		MonitorNotifier monitorNotifier;
		RangeNotifier rangeNotifier;
		Region monitoringRegion;
		Region rangingRegion;

		int previousProximity;

		public void StartScannerService() {

			this.iBeaconManager = IBeaconManager.GetInstanceForApplication(this);

			this.monitorNotifier = new MonitorNotifier();
			this.rangeNotifier = new RangeNotifier();

			this.monitoringRegion = new Region(ScannerManager.ScanerID, ScannerManager.FindMeUUID, null, null);
			this.rangingRegion = new Region(ScannerManager.ScanerID, ScannerManager.FindMeUUID, null, null);

			this.iBeaconManager.Bind(this);

			this.monitorNotifier.EnterRegionComplete += EnteredRegion;
			this.monitorNotifier.ExitRegionComplete += ExitedRegion;

			this.rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;
		}

		public void StopScannerService() {

			this.monitorNotifier.EnterRegionComplete -= EnteredRegion;
			this.monitorNotifier.ExitRegionComplete -= ExitedRegion;

			this.rangeNotifier.DidRangeBeaconsInRegionComplete -= RangingBeaconsInRegion;

			this.iBeaconManager.StopMonitoringBeaconsInRegion(this.monitoringRegion);
			this.iBeaconManager.StopRangingBeaconsInRegion(this.rangingRegion);
			this.iBeaconManager.UnBind(this);
		}

		void EnteredRegion(object sender, MonitorEventArgs e)
		{
			if(this.paused)
			{
				ShowNotification();
			}
		}

		void ExitedRegion(object sender, MonitorEventArgs e)
		{
			// - Handle when the beacon exit the region
		}

		void RangingBeaconsInRegion(object sender, RangeEventArgs e)
		{
			List<FMBeacon> iBeaconsList = new List<FMBeacon> ();
			if (e.Beacons.Count > 0) {

				foreach (RadiusNetworks.IBeaconAndroid.IBeacon beacon in e.Beacons) {

					var fmBeacon = new FMBeacon ();

					fmBeacon.uuid = beacon.ProximityUuid.ToString();
					fmBeacon.major = beacon.Major.ToString();
					fmBeacon.minor = beacon.Minor.ToString();

					switch((ProximityType)beacon.Proximity)
					{
					case ProximityType.Immediate:
						Console.WriteLine ("Immediate Proximity Entered");
						fmBeacon.proximity = 1;
						break;
					case ProximityType.Near:
						Console.WriteLine ("Near Proximity Entered");
						fmBeacon.proximity = 2;
						break;
					case ProximityType.Far:
						Console.WriteLine ("Far Proximity Entered");
						fmBeacon.proximity = 3;
						break;
					case ProximityType.Unknown:
						Console.WriteLine ("Unkown Proximity Entered");
						fmBeacon.proximity = 4;
						break;
					}

					iBeaconsList.Add (fmBeacon);
				}
			}
			ScannerManager.SharedInstance.SetNewAvailableListOfiBeacons (iBeaconsList);
		}

		#region IBeaconConsumer impl

		public void OnIBeaconServiceConnect()
		{
			this.iBeaconManager.SetMonitorNotifier(this.monitorNotifier);
			this.iBeaconManager.SetRangeNotifier(this.rangeNotifier);

			this.iBeaconManager.StartMonitoringBeaconsInRegion(this.monitoringRegion);
			this.iBeaconManager.StartRangingBeaconsInRegion(this.rangingRegion);
		}

		#endregion

		private void ShowNotification()
		{
			//			var resultIntent = new Intent(this, typeof(MainActivity));
			//			resultIntent.AddFlags(ActivityFlags.ReorderToFront);
			//			var pendingIntent = PendingIntent.GetActivity(this, 0, resultIntent, PendingIntentFlags.UpdateCurrent);
			//			var notificationId = Resource.String.monkey_notification;
			//
			//			var builder = new NotificationCompat.Builder(this)
			//				.SetSmallIcon(Resource.Drawable.Xamarin_Icon)
			//				.SetContentTitle(this.GetText(Resource.String.app_label))
			//				.SetContentText(this.GetText(Resource.String.monkey_notification))
			//				.SetContentIntent(pendingIntent)
			//				.SetAutoCancel(true);
			//
			//			var notification = builder.Build();
			//
			//			var notificationManager = (NotificationManager)GetSystemService(NotificationService);
			//			notificationManager.Notify(notificationId, notification);
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			this.StopScannerService ();
		}
	}
}

