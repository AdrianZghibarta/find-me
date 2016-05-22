using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Findme
{
	public class CommandsManager: NetworkingManager
	{

		public static CommandsManager singletonInstance;
		public static readonly String DeliveredCommandsStatus = "delivered";
		public static readonly String PendingCommandStatus = "pending";
		public static readonly String CanceledCommandStatus = "canceled";

		/// <summary>
		/// Gets the shared instance.
		/// </summary>
		/// <value>The shared instance.</value>
		public static CommandsManager SharedInstance
		{
			get 
			{
				if (singletonInstance == null)
				{
					singletonInstance = new CommandsManager ();
				}
				return singletonInstance;
			}
		}

		public CommandsManager ()
		{
		}


		/// <summary>
		/// Gets the commands.
		/// </summary>
		/// <returns>The commands.</returns>
		/// <param name="commandType">Command type.</param>
		public async Task<FindMeResponse> GetCommands(String commandType)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var result = await client.GetAsync(NetworkingUrls.GET_COMMANDS + "?token=" + UserStorage.GetAccessToken() + "&type=" + commandType);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "commands");

				List<Command> commandList = JsonConvert.DeserializeObject<List<Command>>((String)findMeResponse.Result);
				if (null == commandList) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = commandList;
				}
			}
			catch (Exception ex)
			{
				ConsoleOutput.PrintLine ("Error message : " + ex.Message);
				findMeResponse.ErrorInfo = ex.Message;
			}

			return findMeResponse;
		}

		/// <summary>
		/// Modifies the command status.
		/// </summary>
		/// <returns>The command status.</returns>
		/// <param name="commandId">Command identifier.</param>
		/// <param name="commandStatus">Command status.</param>
		public async Task<FindMeResponse> ModifyCommandStatus(String commandId, String commandStatus)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var keyValues = new List<KeyValuePair<string, string>>();
				keyValues.Add(new KeyValuePair<string, string>("token", UserStorage.GetAccessToken()));
				keyValues.Add(new KeyValuePair<string, string>("id", commandId));
				keyValues.Add(new KeyValuePair<string, string>("type", commandStatus));

				var parameters = new FormUrlEncodedContent(keyValues);

				var result = await client.PostAsync(NetworkingUrls.EDIT_COMMAND_URL, parameters);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "command");

				Command command = JsonConvert.DeserializeObject<Command>((String)findMeResponse.Result);
				if (null == command) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = command;
				}
			}
			catch (Exception ex)
			{
				ConsoleOutput.PrintLine ("Error message : " + ex.Message);
				findMeResponse.ErrorInfo = ex.Message;
			}

			return findMeResponse;
		}

	}
}

