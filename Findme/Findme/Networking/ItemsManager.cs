using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Plugin.Geolocator;

namespace Findme
{
	public class ItemsManager: NetworkingManager
	{
		public static ItemsManager singletonInstance;
		Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;

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
			this.locator.DesiredAccuracy = 100;
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

				List<Item> itemList = JsonConvert.DeserializeObject<List<Item>>((String)findMeResponse.Result);
				if (null == itemList) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = itemList;
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
		/// Adds the item.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="item">Item.</param>
		/// <param name="imageBase64">Image base64.</param>
		public async Task<FindMeResponse> AddItem(Item item, String imageBase64)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var keyValues = new List<KeyValuePair<string, string>>();
				keyValues.Add(new KeyValuePair<string, string>("token", UserStorage.GetAccessToken()));
				keyValues.Add(new KeyValuePair<string, string>("id", item._id));
				keyValues.Add(new KeyValuePair<string, string>("name", item.name));
				keyValues.Add(new KeyValuePair<string, string>("description", item.description));
				keyValues.Add(new KeyValuePair<string, string>("categoryName", item.category));
				keyValues.Add(new KeyValuePair<string, string>("beaconId", item.beacon._id));
				if (imageBase64 != null) {
					if (imageBase64 != "") {
						keyValues.Add(new KeyValuePair<string, string>("image", imageBase64));
					}
				}

				var parameters = new FormUrlEncodedContent(keyValues);

				var result = await client.PostAsync(NetworkingUrls.ADD_ITEM_URL, parameters);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "message");

				String message = (String)findMeResponse.Result;
				if (null == message) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = message;
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
		/// Deletes the item.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="itemId">Item identifier.</param>
		/// <param name="imageBase64">Image base64.</param>
		public async Task<FindMeResponse> DeleteItem(String itemId)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var keyValues = new List<KeyValuePair<string, string>>();
				keyValues.Add(new KeyValuePair<string, string>("token", UserStorage.GetAccessToken()));
				keyValues.Add(new KeyValuePair<string, string>("id", itemId));

				var parameters = new FormUrlEncodedContent(keyValues);

				var result = await client.PostAsync(NetworkingUrls.DELETE_ITEM_URL, parameters);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "message");

				String message = (String)findMeResponse.Result;
				if (null == message) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = message;
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
		/// Edits the item.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="item">Item.</param>
		/// <param name="imageBase64">Image base64.</param>
		public async Task<FindMeResponse> EditItem(Item item, String imageBase64)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var keyValues = new List<KeyValuePair<string, string>>();
				keyValues.Add(new KeyValuePair<string, string>("token", UserStorage.GetAccessToken()));
				keyValues.Add(new KeyValuePair<string, string>("id", item._id));
				keyValues.Add(new KeyValuePair<string, string>("name", item.name));
				keyValues.Add(new KeyValuePair<string, string>("description", item.description));
				keyValues.Add(new KeyValuePair<string, string>("categoryName", item.category));
				keyValues.Add(new KeyValuePair<string, string>("beaconId", item.beacon._id));
				if (item.isLosed) {
					keyValues.Add(new KeyValuePair<string, string>("isLosed", "true"));
				}
				else {
					keyValues.Add(new KeyValuePair<string, string>("isLosed", "false"));
				}
				//detachBeacon
				if (imageBase64 != null) {
					if (imageBase64 != "") {
						keyValues.Add(new KeyValuePair<string, string>("image", imageBase64));
					}
				}

				var parameters = new FormUrlEncodedContent(keyValues);

				var result = await client.PostAsync(NetworkingUrls.EDIT_ITEM_URL, parameters);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "item");

				Item updatedItem = JsonConvert.DeserializeObject<Item>((String)findMeResponse.Result);
				if (null == updatedItem) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = updatedItem;
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
		/// Detachis the beacon from item.
		/// </summary>
		/// <returns>The beacon from item.</returns>
		/// <param name="item">Item.</param>
		public async Task<FindMeResponse> DetachiBeaconFromItem(Item item)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				var keyValues = new List<KeyValuePair<string, string>>();
				keyValues.Add(new KeyValuePair<string, string>("token", UserStorage.GetAccessToken()));
				keyValues.Add(new KeyValuePair<string, string>("id", item._id));
				keyValues.Add(new KeyValuePair<string, string>("detachBeacon", "true"));

				var parameters = new FormUrlEncodedContent(keyValues);

				var result = await client.PostAsync(NetworkingUrls.EDIT_ITEM_URL, parameters);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "item");

				Item updatedItem = JsonConvert.DeserializeObject<Item>((String)findMeResponse.Result);
				if (null == updatedItem) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = updatedItem;
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

			try {
				var result = await client.GetAsync (NetworkingUrls.GET_CATEGORIES_URL + "?token=" + UserStorage.GetAccessToken ());
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage (result, "categories");

				List<Category> categoryList = JsonConvert.DeserializeObject<List<Category>> ((String)findMeResponse.Result);
				if (null == categoryList) {
					findMeResponse.ErrorInfo = "No Object Found";
				} else {
					findMeResponse.Result = categoryList;
				}
			} catch (Exception ex) {
				ConsoleOutput.PrintLine ("Error message : " + ex.Message);
				findMeResponse.ErrorInfo = ex.Message;
			}

			return findMeResponse;
		}

		/// <summary>
		/// Gets the items for beacons.
		/// </summary>
		/// <returns>The items for beacons.</returns>
		/// <param name="beaconList">Beacon list.</param>
		public async Task<FindMeResponse> GetItemsForBeacons(List<FMBeacon> beaconList)
		{
			var findMeResponse = new FindMeResponse ();

			try
			{
				String beaconsJsonString = JsonConvert.SerializeObject(beaconList);
				var result = await client.GetAsync(NetworkingUrls.GET_ITEMS_URL + 
					"?token=" + UserStorage.GetAccessToken() + 
					"&beacons=" + beaconsJsonString);
				findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage(result, "items");

				List<Item> itemList = JsonConvert.DeserializeObject<List<Item>>((String)findMeResponse.Result);
				if (null == itemList) {
					findMeResponse.ErrorInfo = "No Object Found";
				}
				else {
					findMeResponse.Result = itemList;
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
		/// Creates the repport for item identifier.
		/// </summary>
		/// <returns>The repport for item identifier.</returns>
		/// <param name="itemId">Item identifier.</param>
		/// <param name="longitude">Longitude.</param>
		/// <param name="latitude">Latitude.</param>
		public async Task<FindMeResponse> CreateRepportForItemId(String itemId)
		{
			var findMeResponse = new FindMeResponse ();
			var position = await this.locator.GetPositionAsync (timeoutMilliseconds: 15000);

			if (position != null) {
				try {
					var keyValues = new List<KeyValuePair<string, string>> ();
					keyValues.Add (new KeyValuePair<string, string> ("token", UserStorage.GetAccessToken ()));
					keyValues.Add (new KeyValuePair<string, string> ("itemId", itemId));
					keyValues.Add (new KeyValuePair<string, string> ("longitude", position.Longitude.ToString()));
					keyValues.Add (new KeyValuePair<string, string> ("latitude", position.Latitude.ToString()));

					var parameters = new FormUrlEncodedContent (keyValues);

					var result = await client.PostAsync (NetworkingUrls.ADD_REPPORT_URL, parameters);
					var stringResult = await result.Content.ReadAsStringAsync();
					findMeResponse = await NetworkingManager.getFindMeResponseFromHttpResponseMessage (result, "message");

					String message = (String)findMeResponse.Result;
					if (null == message) {
						findMeResponse.ErrorInfo = "No Object Found";
					} else {
						findMeResponse.Result = message;
					}
				} catch (Exception ex) {
					ConsoleOutput.PrintLine ("Error message : " + ex.Message);
					findMeResponse.ErrorInfo = ex.Message;
				}
			} else {
				findMeResponse.ErrorInfo = "Can not get your location";
			}

			return findMeResponse;
		}
	}
}

