using System;

namespace Findme
{
	public class ConsoleOutput
	{
		public ConsoleOutput ()
		{
		}

		/// <summary>
		/// Print a line in the console using System.Diagnostics.Debug.WriteLine("...");
		/// </summary>
		/// <param name="line">The string to be printed to the console optput</param>
		static public void PrintLine(String line) {
			System.Diagnostics.Debug.WriteLine(line);
		}
	}
}

