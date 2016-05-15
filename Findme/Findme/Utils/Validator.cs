using System;
using Xamarin.Forms;

namespace Findme
{
	public class Validator
	{
		public Validator ()
		{
		}

		/// <summary>
		/// Validate an array of entries if there text is empty
		/// </summary>
		/// <returns><c>true</c>, if entires was validated, <c>false</c> otherwise.</returns>
		/// <param name="entries">Entries.</param>
		public static bool ValidateEntires(Entry[] entries) {

			foreach (Entry entry in entries) {
				String entryText = entry.Text;

				if (entryText == null) {
					return false;
				}

				entryText = entryText.Replace (" ", "");

				if (entryText == "") {
					return false;
				}
			}

			return true;
		}
	}
}

