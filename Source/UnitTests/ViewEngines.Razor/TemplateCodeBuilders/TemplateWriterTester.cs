using Junior.Route.ViewEngines.Razor.TemplateCodeBuilders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.Razor.TemplateCodeBuilders
{
	public static class TemplateWriterTester
	{
		[TestFixture]
		public class When_converting_instance_to_string
		{
			[SetUp]
			public void SetUp()
			{
				_writer = new TemplateWriter(writer =>
					{
						_delegateInvoked = true;
						writer.Write("Content");
					});
			}

			private TemplateWriter _writer;
			private bool _delegateInvoked;

			[Test]
			public void Must_call_delegate()
			{
				_writer.ToString();

				Assert.That(_delegateInvoked, Is.True);
			}

			[Test]
			public void Must_return_written_content()
			{
				Assert.That(_writer.ToString(), Is.EqualTo("Content"));
			}
		}
	}
}