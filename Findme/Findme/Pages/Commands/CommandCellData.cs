using System;
using System.Collections.Generic;

namespace Findme
{
	public class CommandCellData
	{
		public String Id { get; set; }
		public String PriceValue { get; set; }
		public String QuantityValue { get; set; }
		public String PriceAndQuantityValue { get; set; }
		public String DateValue { get; set; }
		public bool IsDelivered { get; set; }
		public bool NeedToShowActions { get; set; }

		public CommandCellData (Command command)
		{
			if (command._id != null) {
				this.Id = command._id;
			} else {
				this.Id = "NO_ID";
			}

			if (command.totalPrice != null) {
				this.PriceValue = command.totalPrice.ToString () + " $";
			} else {
				this.PriceValue = "No Information";
			}

			if (command.beacons != null) {
				this.QuantityValue = command.beacons.Count.ToString () + " beacon(s)";
			} else {
				this.QuantityValue = "No Information";
			}

			this.PriceAndQuantityValue = this.PriceValue + " for " + this.QuantityValue;

			this.DateValue = "Created at " + command.dateCreation.ToString ("yyyy MMMMM dd");

			if (command.status != null) {
				if (command.status == CommandsManager.DeliveredCommandsStatus) {
					this.IsDelivered = true;
				} else {
					this.IsDelivered = false;
				}
			} else {
				this.IsDelivered = false;
			}

			this.NeedToShowActions = !this.IsDelivered;
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

