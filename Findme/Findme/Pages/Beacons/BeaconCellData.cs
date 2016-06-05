using System;
using System.Collections.Generic;

namespace Findme
{
	public class BeaconCellData
	{
		public String Id { get; set; }
		public String PriceValue { get; set; }
		public String GeneralInfo { get; set; }
		public String MinorMajorInfo { get; set; }
		public String Status { get; set; }
		public bool CanBeDeleted { get; set; }

		public BeaconCellData (FMBeacon beacon)
		{
			if (beacon._id != null) {
				this.Id = beacon._id;
			} else {
				this.Id = "NO_ID";
			}

			if (beacon.price != null) {
				this.PriceValue = "Price: " + beacon.price.ToString () + " $";
			} else {
				this.PriceValue = "No Information";
			}

			this.GeneralInfo = beacon.brand + ", " + beacon.range + " range";
			this.MinorMajorInfo = "Minor: " + beacon.minor + " / Major: " + beacon.major;

			if (beacon.isAttached) {
				this.Status = "Attached, can't be deleted";
			} else {
				this.Status = "Not attached";
			}
		
			this.CanBeDeleted = !beacon.isAttached;
		}

		public static List<BeaconCellData> GetBeconsCellDataList(List<FMBeacon> beaconsList) {

			List<BeaconCellData> listToReturn = new List<BeaconCellData>();

			if (beaconsList != null) {

				foreach (FMBeacon beacon in beaconsList) {
					BeaconCellData beaconCellData = new BeaconCellData (beacon);
					listToReturn.Add (beaconCellData);
				}
			}

			return listToReturn;
		}
	}
}

