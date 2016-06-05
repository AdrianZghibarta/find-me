using System;
using System.Collections.Generic;

namespace Findme
{
	public class User
	{
		public String _id;
		public String firstname;
		public String lastname;
		public String email;
		public String photourl;
		public String accessToken;
		public String refreshToken;
		public List<String> deliveredCommands;
		public List<String> pendingCommands;
		public List<String> bussyBeacons;
		public List<String> availableBeacons;

		public User ()
		{
			
		}
	}
}

