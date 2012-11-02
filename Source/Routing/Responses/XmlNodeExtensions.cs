using System.IO;
using System.Text;
using System.Xml;

using Junior.Common;

namespace Junior.Route.Routing.Responses
{
	public static class XmlNodeExtensions
	{
		public static string GetString(this XmlNode node)
		{
			node.ThrowIfNull("node");

			var stringWriter = new StringWriter();
			var xmlTextWriter = new XmlTextWriter(stringWriter);

			node.WriteTo(xmlTextWriter);
			xmlTextWriter.Flush();

			return stringWriter.ToString();
		}

		public static byte[] GetBytes(this XmlNode node, Encoding encoding)
		{
			node.ThrowIfNull("node");

			var stream = new MemoryStream();
			var writer = new XmlTextWriter(stream, encoding);

			node.WriteTo(writer);
			writer.Flush();

			return stream.ToArray();
		}
	}
}