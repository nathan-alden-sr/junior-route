using System.Threading.Tasks;

using Junior.Route.AutoRouting;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting
{
	public static class TaskExtensionsTester
	{
		[TestFixture]
		public class When_upcasting_generic_task
		{
			[SetUp]
			public void SetUp()
			{
				_task = Task.FromResult("test");
			}

			private Task<string> _task;

			[Test]
			public async void Must_generate_result_using_original_delegate()
			{
				Assert.That(await _task.Upcast<string, object>(), Is.EqualTo("test"));
			}

			[Test]
			public void Must_upcast_to_specified_type()
			{
				Assert.That(() => _task.Upcast<string, object>(), Throws.Nothing);
			}
		}
	}
}