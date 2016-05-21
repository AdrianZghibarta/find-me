using System;
using System.Threading.Tasks;
using System.IO;

namespace Findme
{
	public class StreamHelper
	{
		public StreamHelper ()
		{
		}

		/// <summary>
		/// Gets the bytes array from stream.
		/// </summary>
		/// <returns>The bytes array from stream.</returns>
		/// <param name="stream">Stream.</param>
		public static async Task<byte[]> GetBytesArrayFromStream(Stream stream) {

			using (MemoryStream ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}

