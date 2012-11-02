using Junior.Route.AutoRouting.ResolvedRelativeUrlMappers;
using Junior.Route.AutoRouting.ResolvedRelativeUrlMappers.Attributes;

using NUnit.Framework;

namespace Junior.Route.UnitTests.AutoRouting.ResolvedRelativeUrlMappers
{
	public static class ResolvedRelativeUrlAttributeMapperTester
	{
		[TestFixture]
		public class When_mapping_resolved_relative_urls_from_resolvedrelativeurlattributes
		{
			[SetUp]
			public void SetUp()
			{
				_mapper = new ResolvedRelativeUrlAttributeMapper();
			}

			private ResolvedRelativeUrlAttributeMapper _mapper;

			public class Endpoint
			{
				[ResolvedRelativeUrl("relative")]
				public void Method()
				{
				}
			}

			[Test]
			public void Must_use_name_from_attribute()
			{
				ResolvedRelativeUrlResult result = _mapper.Map(typeof(Endpoint), typeof(Endpoint).GetMethod("Method"));

				Assert.That(result.ResolvedRelativeUrl, Is.EqualTo("relative"));
				Assert.That(result.ResultType, Is.EqualTo(ResolvedRelativeUrlResultType.ResolvedRelativeUrlMapped));
			}
		}
	}
}