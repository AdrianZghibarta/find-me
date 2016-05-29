using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Findme
{
	public class ItemsManager: NetworkingManager
	{
		public static ItemsManager singletonInstance;

		/// <summary>
		/// Gets the shared instance.
		/// </summary>
		/// <value>The shared instance.</value>
		public static ItemsManager SharedInstance
		{
			get 
			{
				if (singletonInstance == null)
				{
					singletonInstance = new ItemsManager ();
				}
				return singletonInstance;
			}
		}

		public ItemsManager ()
		{
		}

		/// <summary>
		/// Gets the items.
		/// </summary>
		/// <returns>The items.</returns>
		public async Task<FindMeResponse> GetItems()
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var result = await client.GetAsync(NetworkingUrls.GET_ITEMS_URL + "?token=" + UserStorage.GetAccessToken() + "&category=" + "");
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "items");

				List<Item> commandList = JsonConvert.DeserializeObject<List<Item>>((String)findMeResponse.Result);
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
		/// Gets the categories.
		/// </summary>
		/// <returns>The categories.</returns>
		public async Task<FindMeResponse> GetCategories()
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var result = await client.GetAsync(NetworkingUrls.GET_CATEGORIES_URL + "?token=" + UserStorage.GetAccessToken());
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "categories");

				List<Category> categoryList = JsonConvert.DeserializeObject<List<Category>>((String)findMeResponse.Result);
				if (null == categoryList) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = categoryList;
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

