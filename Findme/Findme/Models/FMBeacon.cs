using System;

namespace Findme
{
	public class FMBeacon
	{

		public String _id;
		public bool isBuyed;
		public bool isAttached;
		public String price;
		public String brand;
		public String range;
		public String major;
		public String minor;
		public String uuid;
		public int proximity;

		public FMBeacon ()
		{
		}

		public String GetMinorAndMajorInformation() {

			return this.major + "/" + minor;
		}

		public String GetGeneralInformation() {
			return this.brand + " " + this.range + " range";
		}
	}
}

