using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Findme
{
	public class CommandsListPage: ContentPage
	{

		#region -> UIElements

		LoadingView loadingView = new LoadingView();
		NoDataView noDataView = new NoDataView ();
		ListView commandListView = new ListView();
		HeaderView headerView = new HeaderView();

		#endregion

		#region -> Propriete

		private List<CommandCellData> commandsCellDataList = new List<CommandCellData>();
		private String currentFilter = CommandsManager.DeliveredCommandsStatus;

		#endregion

		public CommandsListPage ()
		{
			this.Title = "My Commands";

			// - Set the layout
			this.SetLayout ();

			// - Set the toolbar buttons
			this.AddToolbarButtons ();

			// - Get the commands for current authentificated user
			this.commandListView.BeginRefresh ();

			// - Deselect a cell after she is clicked
			this.commandListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {
				this.commandListView.SelectedItem = null;
			};
		}

		#region -> Toolbar buttons 

		private void AddToolbarButtons() {

			ToolbarItems.Add (new ToolbarItem( "Filter", "filterIcon.png", async () =>
				{
					var action = await DisplayActionSheet ("Select command type", "Cancel", null, "Delivered", "Pending");
					if (action == "Delivered") {
						this.currentFilter = CommandsManager.DeliveredCommandsStatus;
						this.commandListView.BeginRefresh();
					} else if (action == "Pending") {
						this.currentFilter = CommandsManager.PendingCommandStatus;
						this.commandListView.BeginRefresh();
					}
				}
			));

			ToolbarItems.Add (new ToolbarItem( "New", "addIcon.png", () =>
				{
					Device.OpenUri(new Uri(NetworkingUrls.WEB_BUY_URL));
				}
			));
		}

		#endregion

		#region -> Confirm / Cancel Command methods

		public async Task ConfirmCommand(String commandId) {

			var action = await DisplayActionSheet ("This will confirm that you have received the command !", "Cancel", null, "Confirm");
			if (action == "Confirm") {
				this.ModifyCommand (commandId, CommandsManager.DeliveredCommandsStatus);
			}
		}

		public async Task CancelCommand(String commandId) {

			var action = await DisplayActionSheet ("This will cancel your order and will refound your money !", "Cancel", null, "Confirm");
			if (action == "Confirm") {
				this.ModifyCommand (commandId, CommandsManager.CanceledCommandStatus);
			}
		}

		private void ModifyCommand(String commandID, String commadStatus) {
			
			this.loadingView.Show ();
			CommandsManager.SharedInstance.ModifyCommandStatus (commandID, commadStatus).ContinueWith ( task => {

				this.loadingView.Hide();
				FindMeResponse response = (FindMeResponse)task.Result;
				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {
					this.GetCommandsForCurrentFilter();
				}
			});
		}

		#endregion

		#region -> Web Work

		private void GetCommandsForCurrentFilter() {
			
			CommandsManager.SharedInstance.GetCommands (this.currentFilter).ContinueWith ( task => {

				FindMeResponse response = (FindMeResponse)task.Result;

				Device.BeginInvokeOnMainThread( () => {
					this.commandListView.EndRefresh();
				});

				if (null != response.ErrorInfo) {
					Device.BeginInvokeOnMainThread( () => {
						this.DisplayAlert("Error", response.ErrorInfo, "Ok");
					});
				}
				else {
					
					List<Command> commandsList = (List<Command>)response.Result;
					this.commandsCellDataList = CommandCellData.GetCommandCellList(commandsList);
					if (this.commandsCellDataList.Count > 0) {
						this.noDataView.Hide();
					} else {
						this.noDataView.Show();
					}

					Device.BeginInvokeOnMainThread( () => {

						if (currentFilter == CommandsManager.DeliveredCommandsStatus) {
							this.headerView.Title = "Delivered Commands";
						} else {
							this.headerView.Title = "Pending Commands";
						}

						this.commandListView.ItemsSource = null;
						this.commandListView.ItemsSource = this.commandsCellDataList;
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

			var cellTemplate = new DataTemplate (typeof(CommandCell));
			this.commandListView = new ListView () { 
				ItemsSource = this.commandsCellDataList,
				VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Transparent,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = cellTemplate,
				RowHeight = 125,
				IsPullToRefreshEnabled = true
			};

			this.commandListView.Refreshing += (object sender, EventArgs e) => {
				this.GetCommandsForCurrentFilter();
			};

			rootLayout.Children.Add (
				this.commandListView,
				Constraint.Constant(0),
				Constraint.Constant(45),
				Constraint.RelativeToParent( parent => {
					return parent.Width;
				}),
				Constraint.RelativeToParent( parent => {
					return parent.Height;
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

