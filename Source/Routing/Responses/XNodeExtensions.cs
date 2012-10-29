using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Junior.Common;

namespace Junior.Route.Routing.Responses
{
	public static class XNodeExtensions
	{
		public static string GetString(this XNode rss)
		{
			rss.ThrowIfNull("rss");

			var stringWriter = new StringWriter();
			var xmlTextWriter = new XmlTextWriter(stringWriter);

			rss.WriteTo(xmlTextWriter);

			return stringWriter.ToString();
		}

		public static byte[] GetBytes(this XNode rss, Encoding encoding)
		{
			rss.ThrowIfNull("rss");

			var stream = new MemoryStream();
			var writer = new XmlTextWriter(stream, encoding);

			rss.WriteTo(writer);

			return stream.ToArray();
		}
	}
}