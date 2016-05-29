using System;
using System.Collections.Generic;

namespace Findme
{
	public class Item
	{
		public String _id;
		public FMBeacon beacon;
		public bool isLosed;
		public String category;
		public String description;
		public String name;
		public String photoUrl;
		public List<Object> foundReports;
		public List<Object> missingRepports;

		public Item ()
		{
		}
	}
}

