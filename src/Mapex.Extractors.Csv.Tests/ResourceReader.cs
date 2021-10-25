using System.IO;
using System.Reflection;
using System.Text;

namespace Mapex.Extractors.Csv.Tests
{
	public static class ResourceReader
	{
		public static byte[] ReadAsByteArray(string resource)
		{
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
			{
				if (stream == null)
					return null;

				using (var ms = new MemoryStream())
				{
					stream.CopyTo(ms);
					return ms.ToArray();
				}
			}
		}

		public static Stream ReadAsStream(string resourceName)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		}

		public static string ReadAsString(string resource)
		{
			return Encoding.UTF8.GetString(ReadAsByteArray(resource));
		}
	}
}
