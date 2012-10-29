using System.IO;
using System.Text;
using System.Xml;

using Junior.Common;

namespace Junior.Route.Routing.Responses
{
	public static class XmlNodeExtensions
	{
		public static string GetString(this XmlNode rss)
		{
			rss.ThrowIfNull("rss");

			var stringWriter = new StringWriter();
			var xmlTextWriter = new XmlTextWriter(stringWriter);

			rss.WriteTo(xmlTextWriter);

			return stringWriter.ToString();
		}

		public static byte[] GetBytes(this XmlNode rss, Encoding encoding)
		{
			rss.ThrowIfNull("rss");

			var stream = new MemoryStream();
			var writer = new XmlTextWriter(stream, encoding);

			rss.WriteTo(writer);

			return stream.ToArray();
		}
	}
}