using System;
using Xamarin.Forms;

namespace Findme
{
	public class ScannCellData
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
		public Color RangeColor { get; set; }
		public bool FirstRangeVisibility { get; set; }
		public bool SecondRangeVisibility { get; set; }
		public bool ThirdRangeVisibility { get; set; }

		public ScannCellData (Item item, int proximityRange)
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

			switch (proximityRange) {
				 
			case(1):
				this.FirstRangeVisibility = true;
				this.SecondRangeVisibility = true;
				this.ThirdRangeVisibility = true;
				this.RangeColor = Color.Green;
				break;
			case(2):
				this.FirstRangeVisibility = true;
				this.SecondRangeVisibility = true;
				this.ThirdRangeVisibility = false;
				this.RangeColor = Color.Yellow;
				break;
			case(3):
				this.FirstRangeVisibility = true;
				this.SecondRangeVisibility = false;
				this.ThirdRangeVisibility = false;
				this.RangeColor = Color.Blue;
				break;
			default:
				this.FirstRangeVisibility = false;
				this.SecondRangeVisibility = false;
				this.ThirdRangeVisibility = false;
				this.RangeColor = Color.Red;
				break;
			}

			ConsoleOutput.PrintLine ("Ceva");
		}
	}
}

