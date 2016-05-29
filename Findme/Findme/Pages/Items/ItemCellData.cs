using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Findme
{
	public class ItemCellData
	{
		public String Id { get; set; }
		public String Name { get; set; }
		public String Category { get; set; }
		public String MinorMajor { get; set; }
		public String Description { get; set; }
		public bool NeedToShowFoundRepports { get; set;}
		public bool IsLosed { get; set; }
		public ImageSource ItemImageSource { get; set; }
		public Color StatusColor { get; set; }

		public ItemCellData (Item item)
		{

			this.Id = item._id;
			this.Name = "Name: " + item.name;
			this.Description = "Description: " + item.description;
			this.Category = "Category: " + item.category;
			this.IsLosed = item.isLosed;

			String imageUrl = NetworkingUrls.BASE_URL + item.photoUrl;
			this.ItemImageSource = ImageSource.FromUri(new Uri(imageUrl));

			if (this.IsLosed) {
				this.StatusColor = ColorMap.RedBackgroundColor;
			} else {
				this.StatusColor = ColorMap.DarkBlueColor;
			}

			if (item.beacon != null) {
				if (item.beacon._id != null) {
					this.MinorMajor = "Beacon: " + item.beacon.major + " / " + item.beacon.minor;
				} else {
					this.MinorMajor = "No beacon attached";
				}
			} else {
				this.MinorMajor = "No beacon attached";
			}

			this.NeedToShowFoundRepports = (item.foundReports.Count > 0);
		}

		public static List<ItemCellData> GetItemsCellDataList(List<Item> itemList) {

			List<ItemCellData> listToReturn = new List<ItemCellData>();

			if (itemList != null) {

				foreach (Item item in itemList) {
					ItemCellData itemCellData = new ItemCellData (item);
					listToReturn.Add (itemCellData);
				}
			}

			return listToReturn;
		}
	}
}

