using System;

using Junior.Route.Routing.Responses;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.Responses
{
	public static class ImmutableResponseTester
	{
		[TestFixture]
		public class When_creating_instance
		{
			private class Foo : ImmutableResponse
			{
				public Foo(Action<Response> configurationDelegate)
					: base(Response.OK(), configurationDelegate)
				{
				}
			}

			[Test]
			public void Must_call_configuration_delegate()
			{
				bool executed = false;

				// ReSharper disable ObjectCreationAsStatement
				new Foo(response => executed = true);
				// ReSharper restore ObjectCreationAsStatement

				Assert.That(executed, Is.True);
			}
		}
	}
}