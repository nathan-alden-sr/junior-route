using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http.RequestHeaders;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http.RequestHeaders
{
	public static class StringExtensionsTester
	{
		[TestFixture]
		public class When_finding_commas_outside_quotes
		{
			[Test]
			[TestCase("\"A,B,C,D\"  \"E,F\" \"\r\n,\" \",\"")]
			[TestCase("A,B,C,D\"  \"E,F\" \"\r\n,\" \",\"", 1, 3, 5, 12, 19, 23)]
			[TestCase(",,,,,", 0, 1, 2, 3, 4)]
			[TestCase(@""",,,,,""")]
			[TestCase(@""","","","","",""", 3, 7)]
			public void Must_find_correct_indices(object[] parameters)
			{
				var value = (string)parameters[0];
				IEnumerable<int> expectedIndices = parameters
					.Where((arg, i) => i > 0)
					.Cast<int>();

				Assert.That(value.FindCommasOutsideQuotes(), Is.EquivalentTo(expectedIndices));
			}
		}

		[TestFixture]
		public class When_splitting_on_commas_outside_quotes
		{
			[Test]
			[TestCase("\"A,B,C,D\"  \"E,F\" \"\r\n,\" \",\"", "\"A,B,C,D\"  \"E,F\" \"\r\n,\" \",\"")]
			[TestCase("A,B,C,D\"  \"E,F\" \"\r\n,\" \",\"", "A", "B", "C", @"D""  ""E", "F\" \"\r\n", @""" """, @"""")]
			[TestCase(",,,,,", "", "", "", "", "", "")]
			[TestCase(@""",,,,,""", @""",,,,,""")]
			[TestCase(@""","","","","",""", @""",""", @""",""", @""",""")]
			public void Must_split_into_correct_strings(object[] parameters)
			{
				var value = (string)parameters[0];
				IEnumerable<string> expectedStrings = parameters
					.Where((arg, i) => i > 0)
					.Cast<string>();

				Assert.That(value.SplitOnCommasOutsideQuotes(), Is.EquivalentTo(expectedStrings));
			}
		}
	}
}