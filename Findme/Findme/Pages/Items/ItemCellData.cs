using System;
using System.Collections.Generic;

namespace Findme
{
	public class ItemCellData
	{
		public String Id { get; set; }
		public String Name { get; set; }
		public String Category { get; set; }
		public String MinorMajor { get; set; }
		public String Description { get; set; }
		public bool IsDelivered { get; set; }
		public bool NeedToShowActions { get; set; }

		public ItemCellData (Command command)
		{
			
		}

		public static List<CommandCellData> GetCommandCellList(List<Command> commandList) {

			List<CommandCellData> listToReturn = new List<CommandCellData>();

			if (commandList != null) {

				foreach (Command command in commandList) {
					CommandCellData commandCellData = new CommandCellData (command);
					listToReturn.Add (commandCellData);
				}
			}

			return listToReturn;
		}
	}
}

