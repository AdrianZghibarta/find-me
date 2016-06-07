using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Findme
{
	public class ScanPage: ContentPage
	{

		#region -> UIElements

		LoadingView loadingView = new LoadingView();
		NoDataView noDataView = new NoDataView ();
		public ListView scannListView = new ListView();
		HeaderView headerView = new HeaderView();

		#endregion

		#region -> Proprietes

		private bool needToContinueScan = true;
		private List<FMBeacon> scannedBeacons = new List<FMBeacon>();
		private List<FMBeacon> bufferBeacons = new List<FMBeacon>();
		private List<Item> foundedItems = new List<Item>();
		private List<ScannCellData> scannCellDataList = new List<ScannCellData>();
		private List<String> repportedItems = new List<String>();

		#endregion

		public ScanPage ()
		{
			this.Title = "Scan Page";
			this.SetLayout ();
			this.StartRequestTimer ();
			this.StartBeaconSyncTimer ();
			ScannerManager.SharedInstance.didFoundiBeaconsDelegate = ( ibeaconsList => {

				this.bufferBeacons = ibeaconsList;
				foreach (FMBeacon beacon in this.scannedBeacons) {

					ConsoleOutput.PrintLine ("New iBeacon found: " + beacon.uuid + "/" + 
						beacon.major + "/" + beacon.minor + " >> " + beacon.proximity.ToString());
				}
			});
		}

		#region -> Override mehtods

		override protected void OnAppearing() {

			base.OnAppearing ();
			ScannerManager.SharedInstance.StartScannerForCurrentPlatform ();
			this.needToContinueScan = true;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			ScannerManager.SharedInstance.StopScannerForCurrentPalatform ();
			this.needToContinueScan = false;
		}

		#endregion

		#region -> Beacon processing Logic

		private void StartRequestTimer() {
			Device.StartTimer(TimeSpan.FromSeconds(1.5), () =>
				{
					this.VerifyBeacons();
					return this.needToContinueScan;
				});
		}

		private void StartBeaconSyncTimer() {
			Device.StartTimer(TimeSpan.FromSeconds(1), () =>
				{
					this.scannedBeacons = this.bufferBeacons;
					return this.needToContinueScan;
				});
		}

		private void VerifyBeacons() {

			if (this.scannedBeacons.Count == 0) {
				this.scannListView.ItemsSource = null;
				this.noDataView.Show ();
				return;
			}

			ItemsManager.SharedInstance.GetItemsForBeacons(this.scannedBeacons).ContinueWith ( task => {

				FindMeResponse response = (FindMeResponse)task.Result;

				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {

					this.foundedItems = (List<Item>)response.Result;
					this.foundedItems = this.foundedItems.Where (x => x.isLosed == true).ToList();

					if (this.foundedItems.Count > 0) {
						this.UpdateListView();
					} else {
						this.scannListView.ItemsSource = null;
						this.noDataView.Show();
					}
				}
			});
		}

		private void UpdateListView() {

			this.scannCellDataList.Clear ();
			foreach (Item item in this.foundedItems) {

				var filteredItems = this.scannedBeacons.Where (x => (x.minor == item.beacon.minor) && (x.major == item.beacon.major)).ToList();
				if (filteredItems.Count > 0) {
					var matchBeacon = filteredItems.First();
					ScannCellData scannCellData = new ScannCellData (item, matchBeacon.proximity);
					this.scannCellDataList.Add (scannCellData);
					if (this.repportedItems.Where (x => x == item._id).ToList ().Count == 0) {
						this.repportedItems.Add(item._id);
						ItemsManager.SharedInstance.CreateRepportForItemId (item._id).ContinueWith ( task => {

							FindMeResponse response = (FindMeResponse)task.Result;

							if (null != response.ErrorInfo) {
								Device.BeginInvokeOnMainThread( () => {
									this.DisplayAlert("Error", response.ErrorInfo, "Ok");
								});
								this.repportedItems.Remove(item._id);
							}
							else {
							}
						});
					}
				}
			}

			if (this.scannCellDataList.Count == 0) {
				this.noDataView.Show ();
			} else {
				this.noDataView.Hide ();
			}

			Device.BeginInvokeOnMainThread ( () => {
				this.scannListView.ItemsSource = null;
				this.scannListView.ItemsSource = this.scannCellDataList;
			});
		}

		#endregion

		#region -> Layout setup 

		private void SetLayout() {

			var rootLayout = new RelativeLayout () { 
				BackgroundColor = ColorMap.GreenColor
			};

			var cellTemplate = new DataTemplate (typeof(ScannCell));
			this.scannListView = new ListView () { 
				ItemsSource = this.scannCellDataList,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Transparent,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = cellTemplate,
				RowHeight = 185,
				IsPullToRefreshEnabled = false
			};

			rootLayout.Children.Add (
				this.scannListView,
				Constraint.Constant(0),
				Constraint.Constant(45),
				Constraint.RelativeToParent( parent => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Height - 45;
				})
			);

			this.headerView = new HeaderView () { 
				Title = "Scanning...",
				ImagesName = "circle.png"
			};

			rootLayout.Children.Add (
				this.headerView,
				Constraint.Constant(0),
				Constraint.Constant(5),
				Constraint.RelativeToParent( parent => {
					return parent.Width;
				}),
				Constraint.Constant(40)
			);

			this.noDataView.AddTo (rootLayout);
			this.loadingView.AddTo (rootLayout);

			this.Content = rootLayout;
		}

		#endregion
	}
}

