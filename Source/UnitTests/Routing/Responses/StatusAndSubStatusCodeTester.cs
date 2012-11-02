using System.Net;

using Junior.Route.Routing.Responses;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Routing.Responses
{
	public static class StatusAndSubStatusCodeTester
	{
		[TestFixture]
		public class When_comparing_equal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_statusAndSubStatusCode1 = new StatusAndSubStatusCode(HttpStatusCode.Created, 1);
				_statusAndSubStatusCode2 = new StatusAndSubStatusCode(HttpStatusCode.Created, 1);
			}

			private StatusAndSubStatusCode _statusAndSubStatusCode1;
			private StatusAndSubStatusCode _statusAndSubStatusCode2;

			[Test]
			public void Must_be_equal()
			{
				Assert.That(_statusAndSubStatusCode1.Equals(_statusAndSubStatusCode2), Is.True);
			}
		}

		[TestFixture]
		public class When_comparing_inequal_instances
		{
			[SetUp]
			public void SetUp()
			{
				_statusAndSubStatusCode1 = new StatusAndSubStatusCode(HttpStatusCode.Created, 1);
				_statusAndSubStatusCode2 = new StatusAndSubStatusCode(HttpStatusCode.Created, 2);
			}

			private StatusAndSubStatusCode _statusAndSubStatusCode1;
			private StatusAndSubStatusCode _statusAndSubStatusCode2;

			[Test]
			public void Must_be_equal()
			{
				Assert.That(_statusAndSubStatusCode1.Equals(_statusAndSubStatusCode2), Is.False);
			}
		}

		[TestFixture]
		public class When_creating_instance_with_httpstatuscode
		{
			[SetUp]
			public void SetUp()
			{
				_statusAndSubStatusCode = new StatusAndSubStatusCode(HttpStatusCode.Created, 1);
			}

			private StatusAndSubStatusCode _statusAndSubStatusCode;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_statusAndSubStatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.Created));
				Assert.That(_statusAndSubStatusCode.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
				Assert.That(_statusAndSubStatusCode.SubStatusCode, Is.EqualTo(1));
			}
		}

		[TestFixture]
		public class When_creating_instance_with_integer_status_codes
		{
			[SetUp]
			public void SetUp()
			{
				_statusAndSubStatusCode = new StatusAndSubStatusCode(201, 1);
			}

			private StatusAndSubStatusCode _statusAndSubStatusCode;

			[Test]
			public void Must_set_properties()
			{
				Assert.That(_statusAndSubStatusCode.ParsedStatusCode, Is.EqualTo(HttpStatusCode.Created));
				Assert.That(_statusAndSubStatusCode.StatusCode, Is.EqualTo(201));
				Assert.That(_statusAndSubStatusCode.SubStatusCode, Is.EqualTo(1));
			}
		}
	}
}