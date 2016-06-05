using System;
using System.Collections.Generic;

namespace Findme
{
	public class RepportCellData
	{
		public String Id { get; set; }
		public String UserNameValue { get; set; }
		public String EmailValue { get; set; }
		public String DateValue { get; set; }
		public float Latitude { get; set; }
		public float Longitude { get; set; }

		public RepportCellData (Repport repport)
		{
			this.Id = repport._id;
			this.UserNameValue = repport.reportedBy.firstname + " " + repport.reportedBy.lastname;
			this.EmailValue = repport.reportedBy.email;
			this.DateValue = repport.date.ToString ("yyyy MMMMM dd");

			this.Latitude = repport.latitude;
			this.Longitude = repport.longitude;
		}

		public static List<RepportCellData> GetRepportCellDataList(List<Repport> repportList) {

			List<RepportCellData> listToReturn = new List<RepportCellData>();

			if (repportList != null) {

				foreach (Repport repport in repportList) {
					RepportCellData repportCellData = new RepportCellData (repport);
					listToReturn.Add (repportCellData);
				}
			}

			return listToReturn;
		}
	}
}

