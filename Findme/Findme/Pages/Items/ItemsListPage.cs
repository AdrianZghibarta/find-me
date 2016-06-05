using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace Findme
{
	public class ItemsListPage: ContentPage
	{
		#region -> UIElements

		LoadingView loadingView = new LoadingView();
		NoDataView noDataView = new NoDataView ();
		public ListView itemsListView = new ListView();
		HeaderView headerView = new HeaderView();

		#endregion

		#region -> Propriete

		private List<ItemCellData> itemsCellDataList = new List<ItemCellData>();
		private List<Item> itemsList = new List<Item>();

		#endregion

		public ItemsListPage ()
		{
			this.Title = "My Items";

			// - Set the layout
			this.SetLayout ();

			// - Set the toolbar buttons
			this.AddToolbarButtons ();

			// - Get the item for current authentificated user
			this.itemsListView.BeginRefresh ();

			// - Deselect a cell after she is clicked
			this.itemsListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {

				if (this.itemsListView.SelectedItem != null) {
					ItemCellData selectedItemCellData = (ItemCellData)this.itemsListView.SelectedItem;
					Item itemToEdit = this.itemsList.Where(x => x._id == selectedItemCellData.Id).ToList().First();
					EditItemPage editItemPage = new EditItemPage(itemToEdit);
					editItemPage.itemListPage = this;
					Navigation.PushAsync(editItemPage);
					this.itemsListView.SelectedItem = null;
				}
			};
		}

		#region -> Toolbar buttons 

		private void AddToolbarButtons() {

			ToolbarItems.Add (new ToolbarItem( "New", "addIcon.png", () =>
				{
					EditItemPage editItemPage = new EditItemPage();
					editItemPage.itemListPage = this;
					Navigation.PushAsync(editItemPage);
				}
			));
		}

		#endregion

		#region -> Confirm / Cancel Command methods

		public async Task DeleteItem(String itemId) {

			var action = await DisplayActionSheet ("This will delete the item !", "Cancel", null, "Confirm");
			if (action == "Confirm") {
				this.loadingView.Show ();
				// - Implement the item delete
				ItemsManager.SharedInstance.DeleteItem(itemId).ContinueWith( task => {

					this.loadingView.Hide();

					FindMeResponse response = (FindMeResponse)task.Result;

					if (null != response.ErrorInfo) {
						Device.BeginInvokeOnMainThread( () => {
							this.DisplayAlert("Error", response.ErrorInfo, "Ok");
						});
					}
					else {
						this.GetItems();
					}
				});
			}
		}

		public async Task ShowFoundRepports(String itemId) {

			var item = this.itemsList.Where (x => x._id == itemId).ToArray ().FirstOrDefault ();
			RepportsListPage repportsListPage = new RepportsListPage (item._id, item.name);
			await Navigation.PushAsync (repportsListPage);
		}

		#endregion

		#region -> Web Work

		public void GetItems() {

			ItemsManager.SharedInstance.GetItems().ContinueWith ( task => {

				FindMeResponse response = (FindMeResponse)task.Result;

				Device.BeginInvokeOnMainThread( () => {
					this.itemsListView.EndRefresh();
					this.loadingView.Hide();
				});

				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {

					this.itemsList = (List<Item>)response.Result;
					this.itemsCellDataList = ItemCellData.GetItemsCellDataList(this.itemsList);
					if (this.itemsList.Count > 0) {
						this.noDataView.Hide();
					} else {
						this.noDataView.Show();
					}

					Device.BeginInvokeOnMainThread( () => {

						this.headerView.Title = "Items List";

						this.itemsListView.ItemsSource = null;
						this.itemsListView.ItemsSource = this.itemsCellDataList;
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

			var cellTemplate = new DataTemplate (typeof(ItemCell));
			this.itemsListView = new ListView () { 
				ItemsSource = this.itemsCellDataList,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Transparent,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = cellTemplate,
				RowHeight = 185,
				IsPullToRefreshEnabled = true
			};

			this.itemsListView.Refreshing += (object sender, EventArgs e) => {
				this.GetItems();
			};

			rootLayout.Children.Add (
				this.itemsListView,
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

