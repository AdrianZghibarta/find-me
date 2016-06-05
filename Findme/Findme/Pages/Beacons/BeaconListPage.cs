using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Findme
{
	public class BeaconListPage: ContentPage
	{
		#region -> UIElements

		LoadingView loadingView = new LoadingView();
		NoDataView noDataView = new NoDataView ();
		ListView beaconListView = new ListView();
		HeaderView headerView = new HeaderView();

		#endregion

		#region -> Propriete

		private List<BeaconCellData> beaconsCelldata = new List<BeaconCellData>();

		#endregion

		public BeaconListPage ()
		{
			this.Title = "My Beacons";

			// - Set the layout
			this.SetLayout ();

			// - Get the commands for current authentificated user
			this.beaconListView.BeginRefresh ();

			// - Deselect a cell after she is clicked
			this.beaconListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {
				this.beaconListView.SelectedItem = null;
			};
		}

		#region -> Confirm / Cancel Command methods

		public async Task DeleteBeacon(String beaconId) {

			var action = await DisplayActionSheet ("This will delete your buyed beacon, are you sure ?", "Cancel", null, "Confirm");
			if (action == "Confirm") {
				this.DeleteBeaconWithId(beaconId);
			}
		}

		private void DeleteBeaconWithId(String beaconId) {

			this.loadingView.Show ();
			
			BeaconsManager.SharedInstance.DeleteBeacon (beaconId).ContinueWith ( task => {

				this.loadingView.Hide();
				FindMeResponse response = (FindMeResponse)task.Result;
				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {
					this.GetBeaconsAndRefreshList();
				}
			});
		}

		#endregion

		#region -> Web Work

		private void GetBeaconsAndRefreshList() {

			BeaconsManager.SharedInstance.GetUserBeacons().ContinueWith ( task => {

				FindMeResponse response = (FindMeResponse)task.Result;

				Device.BeginInvokeOnMainThread( () => {
					this.beaconListView.EndRefresh();
				});

				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {

					List<FMBeacon> beaconList = (List<FMBeacon>)response.Result;
					this.beaconsCelldata = BeaconCellData.GetBeconsCellDataList(beaconList);

					if (this.beaconsCelldata.Count > 0) {
						this.noDataView.Hide();
					} else {
						this.noDataView.Show();
					}

					Device.BeginInvokeOnMainThread( () => {
						this.beaconListView.ItemsSource = null;
						this.beaconListView.ItemsSource = this.beaconsCelldata;
					});
				}
			});
		}

		#endregion

		#region -> Layout setup 

		private void SetLayout() {

			var rootLayout = new RelativeLayout () { 
				BackgroundColor = ColorMap.GreenColor
			};

			var cellTemplate = new DataTemplate (typeof(BeaconCell));
			this.beaconListView = new ListView () { 
				ItemsSource = this.beaconsCelldata,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Transparent,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = cellTemplate,
				RowHeight = 135,
				IsPullToRefreshEnabled = true,
			};

			this.beaconListView.Refreshing += (object sender, EventArgs e) => {
				this.GetBeaconsAndRefreshList();
			};

			rootLayout.Children.Add (
				this.beaconListView,
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
				Title = "Beacons List",
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

