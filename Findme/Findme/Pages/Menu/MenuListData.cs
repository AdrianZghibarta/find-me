using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Findme
{

	public class MenuListData : List<MenuItem>
	{
		public MenuListData ()
		{
			this.Add (new MenuItem () { 
				Title = "Items", 
				IconSource = "menuItem.png", 
				TargetType = typeof(ItemsListPage)
			});

			this.Add (new MenuItem () { 
				Title = "Beacons", 
				IconSource = "menuBeacon.png", 
				TargetType = typeof(BeaconListPage)
			});

			this.Add (new MenuItem () { 
				Title = "Commands", 
				IconSource = "menuCommand.png", 
				TargetType = typeof(CommandsListPage)
			});

			this.Add (new MenuItem () {
				Title = "Notifications",
				IconSource = "menuNote.png",
				TargetType = typeof(NotificationsListPage)
			});

			this.Add (new MenuItem () {
				Title = "Scan",
				IconSource = "menuSearch.png",
				TargetType = typeof(ScanPage)
			});

			this.Add (new MenuItem () {
				Title = "Account",
				IconSource = "menuAccount.png",
				TargetType = typeof(EditAccountPage)
			});
		}
	}
}