using System.Text;
using System.Xml;
using System.Xml.Linq;

using Junior.Route.Routing.Responses;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.Responses
{
	public static class XmlNodeExtensionsTester
	{
		[TestFixture]
		public class When_serializing_xmlnode_to_byte_array
		{
			[SetUp]
			public void SetUp()
			{
				var document = new XmlDocument();

				_node = document.AppendChild(document.CreateElement("test"));
			}

			private XmlNode _node;

			[Test]
			public void Must_generate_correct_xml_string()
			{
				Assert.That(_node.GetBytes(Encoding.ASCII), Is.EqualTo(Encoding.ASCII.GetBytes("<test />")));
			}
		}

		[TestFixture]
		public class When_serializing_xmlnode_to_string
		{
			[SetUp]
			public void SetUp()
			{
				_node = new XElement("test", "value");
			}

			private XElement _node;

			[Test]
			public void Must_generate_correct_xml_string()
			{
				Assert.That(_node.GetString(), Is.EqualTo("<test>value</test>"));
			}
		}
	}
}