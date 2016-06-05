using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Findme
{
	public class RepportsListPage: ContentPage
	{
		#region -> UIElements

		LoadingView loadingView = new LoadingView();
		NoDataView noDataView = new NoDataView ();
		ListView repportsListView = new ListView();
		HeaderView headerView = new HeaderView();

		#endregion

		#region -> Propriete

		public String itemId;
		List<Repport> repportsList = new List<Repport>();
		List<RepportCellData> repportsCellDataList = new List<RepportCellData> ();

		#endregion

		public RepportsListPage (String itemId, String itemName)
		{
			this.Title = "Repports";
			this.itemId = itemId;

			// - Set the layout
			this.SetLayout ();

			// - Get the commands for current authentificated user
			this.repportsListView.BeginRefresh ();

			// - Deselect a cell after she is clicked
			this.repportsListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {

				if (this.repportsListView.SelectedItem != null) {

					RepportCellData selectedRepport = (RepportCellData)this.repportsListView.SelectedItem;
					MapPage mapPage = new MapPage(selectedRepport.Latitude, selectedRepport.Longitude, itemName, "Repported by " + selectedRepport.UserNameValue);
					Navigation.PushAsync(mapPage);
					this.repportsListView.SelectedItem = null;	
				}
			};
		}

		#region -> Web Work

		private void GetRepportsForCurrentItemId() {

			RepportsManager.SharedInstance.GetRepports (this.itemId).ContinueWith ( task => {
				FindMeResponse response = (FindMeResponse)task.Result;

				Device.BeginInvokeOnMainThread( () => {
					this.repportsListView.EndRefresh();
				});

				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {

					this.repportsList = (List<Repport>)response.Result;
					this.repportsCellDataList = RepportCellData.GetRepportCellDataList(this.repportsList);
					if (this.repportsCellDataList.Count > 0) {
						this.noDataView.Hide();
					} else {
						this.noDataView.Show();
					}

					Device.BeginInvokeOnMainThread( () => {

						this.headerView.Title = "Repports List";

						this.repportsListView.ItemsSource = null;
						this.repportsListView.ItemsSource = this.repportsCellDataList;
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

			var cellTemplate = new DataTemplate (typeof(RepportCell));
			this.repportsListView = new ListView () { 
				ItemsSource = this.repportsCellDataList,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Transparent,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = cellTemplate,
				RowHeight = 125,
				IsPullToRefreshEnabled = true
			};

			this.repportsListView.Refreshing += (object sender, EventArgs e) => {
				this.GetRepportsForCurrentItemId();
			};

			rootLayout.Children.Add (
				this.repportsListView,
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
				Title = "Loading...",
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

