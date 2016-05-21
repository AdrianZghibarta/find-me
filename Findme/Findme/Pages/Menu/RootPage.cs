using System;
using Xamarin.Forms;
using System.Linq;

namespace Findme
{
	public class RootPage : MasterDetailPage
	{
		MenuPage menuPage;

		public RootPage ()
		{
			menuPage = new MenuPage ();

			menuPage.Menu.ItemSelected += (sender, e) => NavigateTo (e.SelectedItem as MenuItem);

			Master = menuPage;
			Detail = this.GetNavigationPage(new ItemsListPage());
		}

		void NavigateTo (MenuItem menu)
		{
			if (menu == null)
				return;
			
			Page displayPage = (Page)Activator.CreateInstance (menu.TargetType);

			Detail = this.GetNavigationPage(displayPage);

			menuPage.Menu.SelectedItem = null;
			IsPresented = false;
		}

		NavigationPage GetNavigationPage(Page contentPage) {

			NavigationPage navigationPage = new NavigationPage (contentPage);
			navigationPage.BarBackgroundColor = ColorMap.DarkBlueColor;
			navigationPage.BarTextColor = Color.White;

			return navigationPage;
		}
	}
}