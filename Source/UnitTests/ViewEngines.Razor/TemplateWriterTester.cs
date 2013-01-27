using Junior.Route.ViewEngines.Razor;

using NUnit.Framework;

namespace Junior.Route.UnitTests.ViewEngines.Razor
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

		[TestFixture]
		public class When_writing_to_textwriter
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
		}
	}
}