using System;
using System.Collections.Generic;
using System.Linq;

using Junior.Route.Http;

using NUnit.Framework;

namespace Junior.Route.UnitTests.Http
{
	public static class CommonRegexPatternsTester
	{
		[TestFixture]
		public class When_testing_invalid_alpha
		{
			[Test]
			[TestCase("\r\n\t ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<0123456789")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching("[" + CommonRegexPatterns.Alpha + "]"));
			}
		}

		[TestFixture]
		public class When_testing_invalid_cr
		{
			[Test]
			[TestCase("\n\t ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.CR));
			}
		}

		[TestFixture]
		public class When_testing_invalid_cr_lf
		{
			[Test]
			[TestCase("\r")]
			[TestCase("\n")]
			[TestCase("\r \n")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.CrLf));
			}
		}

		[TestFixture]
		public class When_testing_invalid_ctl
		{
			[Test]
			[TestCase(" ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\x7e")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching("[" + CommonRegexPatterns.Ctl + "]"));
			}
		}

		[TestFixture]
		public class When_testing_invalid_digit
		{
			[Test]
			[TestCase("\x00\r\n\t ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz\x7f")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching("[" + CommonRegexPatterns.Digit + "]"));
			}
		}

		[TestFixture]
		public class When_testing_invalid_hex
		{
			[Test]
			[TestCase("\x00\r\n ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<GHIJKLMNOPQRSTUVWXYZghijklmnopqrstuvwxyz\x7f")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching("[" + CommonRegexPatterns.Hex + "]"));
			}
		}

		[TestFixture]
		public class When_testing_invalid_ht
		{
			[Test]
			[TestCase("\x00\r\n ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\x7f")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.HT));
			}
		}

		[TestFixture]
		public class When_testing_invalid_lf
		{
			[Test]
			[TestCase("\r\t ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.LF));
			}
		}

		[TestFixture]
		public class When_testing_invalid_lo_alpha
		{
			[Test]
			[TestCase("\r\n\t ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching("[" + CommonRegexPatterns.LoAlpha + "]"));
			}
		}

		[TestFixture]
		public class When_testing_invalid_lws
		{
			[Test]
			[TestCase("\r")]
			[TestCase("\n")]
			[TestCase(@"~!@#$%^&*()-_=+~\|]}[{;:'""/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.Lws));
			}
		}

		[TestFixture]
		public class When_testing_invalid_parameter
		{
			[Test]
			[TestCase("=1")]
			[TestCase("a")]
			[TestCase("param =value")]
			[TestCase("param value")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.Parameter));
			}
		}

		[TestFixture]
		public class When_testing_invalid_qd_text
		{
			[Test]
			[TestCase(@"""")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.QdText));
			}
		}

		[TestFixture]
		public class When_testing_invalid_quote
		{
			[Test]
			[TestCase(@"~!@#$%^&*()-_=+~\|]}[{;:'/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.Quote));
			}
		}

		[TestFixture]
		public class When_testing_invalid_quoted_pair
		{
			[Test]
			[TestCase(@"\")]
			[TestCase("\x00")]
			[TestCase(@"a")]
			[TestCase("\x7f")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.QuotedPair));
			}
		}

		[TestFixture]
		public class When_testing_invalid_quoted_string
		{
			[Test]
			[TestCase(@"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghi\jklmnopqrstuvwxyz0123456789\")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching("^" + CommonRegexPatterns.QuotedString + "$"));
			}
		}

		[TestFixture]
		public class When_testing_invalid_sp
		{
			[Test]
			[TestCase("\r\n\t~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.SP));
			}
		}

		[TestFixture]
		public class When_testing_invalid_text
		{
			[Test]
			[TestCase(" ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\x00")]
			[TestCase(" ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\r\n")]
			[TestCase(" ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\r\na")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching("^" + CommonRegexPatterns.Text + "+$"));
			}
		}

		[TestFixture]
		public class When_testing_invalid_token
		{
			[Test]
			[TestCase("()<>@,;:\\\"/[]?={} \t\r\n\x00")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching(CommonRegexPatterns.Token));
			}
		}

		[TestFixture]
		public class When_testing_invalid_up_alpha
		{
			[Test]
			[TestCase("\r\n\t ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<abcdefghijklmnopqrstuvwxyz0123456789")]
			public void Regular_expression_must_not_match(string input)
			{
				Assert.That(input, Is.Not.StringMatching("[" + CommonRegexPatterns.UpAlpha + "]"));
			}
		}

		[TestFixture]
		public class When_testing_valid_alpha
		{
			[Test]
			[TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^[" + CommonRegexPatterns.Alpha + "]+$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_char
		{
			[Test]
			[TestCase("\x00\r\n\t ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching(CommonRegexPatterns.CR));
			}
		}

		[TestFixture]
		public class When_testing_valid_cr
		{
			[Test]
			[TestCase("\r")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching(CommonRegexPatterns.CR));
			}
		}

		[TestFixture]
		public class When_testing_valid_cr_lf
		{
			[Test]
			[TestCase("\r\n")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching(CommonRegexPatterns.CrLf));
			}
		}

		[TestFixture]
		public class When_testing_valid_ctl
		{
			[Test]
			[TestCase("\x00\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0a\x0b\x0c\x0d\x0e\x0f\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1a\x1b\x1c\x1d\x1e\x1f\x7f")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^[" + CommonRegexPatterns.Ctl + "]+$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_digit
		{
			[Test]
			[TestCase("0123456789")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^[" + CommonRegexPatterns.Digit + "]+$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_hex
		{
			[Test]
			[TestCase("0123456789ABCDEFabcdef")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^[" + CommonRegexPatterns.Hex + "]+$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_ht
		{
			[Test]
			[TestCase("\t")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching(CommonRegexPatterns.HT));
			}
		}

		[TestFixture]
		public class When_testing_valid_lf
		{
			[Test]
			[TestCase("\n")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching(CommonRegexPatterns.LF));
			}
		}

		[TestFixture]
		public class When_testing_valid_lo_alpha
		{
			[Test]
			[TestCase("abcdefghijklmnopqrstuvwxyz")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^[" + CommonRegexPatterns.LoAlpha + "]+$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_lws
		{
			[Test]
			[TestCase("\r\n   ")]
			[TestCase("\r\n\t\t\t")]
			[TestCase("\r\n\t \t")]
			[TestCase("\r\n ")]
			[TestCase("\r\n\t")]
			[TestCase(" ")]
			[TestCase("\t")]
			[TestCase("   ")]
			[TestCase("\t\t\t")]
			[TestCase("\t \t")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching(CommonRegexPatterns.Lws));
			}
		}

		[TestFixture]
		public class When_testing_valid_octet
		{
			[Test]
			public void Regular_expression_must_match()
			{
				Assert.That(String.Join("", Enumerable.Range(0, 255).Select(arg => (char)arg)), Is.StringMatching("^[" + CommonRegexPatterns.Octet + "]+$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_parameter
		{
			[Test]
			[TestCase("a=1")]
			[TestCase("param=value")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^" + CommonRegexPatterns.Parameter + "$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_qd_text
		{
			[Test]
			[TestCase(@"\r\n\t ~!@#$%^&*()-_=+~\|]}[{;:'/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching(CommonRegexPatterns.QdText));
			}
		}

		[TestFixture]
		public class When_testing_valid_quote
		{
			[Test]
			[TestCase(@"""")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching(CommonRegexPatterns.Quote));
			}
		}

		[TestFixture]
		public class When_testing_valid_quoted_pair
		{
			[Test]
			[TestCase(@"\\x00")]
			[TestCase(@"\\xa")]
			[TestCase(@"\\x ")]
			[TestCase(@"\\x7f")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching(CommonRegexPatterns.QuotedPair));
			}
		}

		[TestFixture]
		public class When_testing_valid_quoted_string
		{
			[Test]
			[TestCase("\"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmn\\opqrstuvwxyz0123456789\"")]
			[TestCase("\"ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n abcdefghi\\jklmnopqrstuvwxyz0123456789\"")]
			[TestCase("\"ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklm\\nopqrstuvwxyz0123456789\"")]
			[TestCase("\"ABCDEFGHIJKLMNOPQRSTUVWXYZ\tabcdefghijkl\\mnopqrstuvwxyz0123456789\"")]
			[TestCase("\"ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n\tabcdefgh\\ijklmnopqrstuvwxyz0123456789\"")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^" + CommonRegexPatterns.QuotedString + "$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_separator
		{
			[Test]
			[TestCase("()<>@,;:\\\"/[]?={} \t")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^[" + CommonRegexPatterns.Separators + "]+$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_sp
		{
			[Test]
			[TestCase(" ")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching(CommonRegexPatterns.SP));
			}
		}

		[TestFixture]
		public class When_testing_valid_text
		{
			[Test]
			[TestCase(" ~!@#$%^&*()-_=+~\\|]}[{;:'\"/?.>,<ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\r\n \r\n\t")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^" + CommonRegexPatterns.Text + "+$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_token
		{
			[Test]
			[TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-%$^&*_+'.")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^" + CommonRegexPatterns.Token + "$"));
			}
		}

		[TestFixture]
		public class When_testing_valid_up_alpha
		{
			[Test]
			[TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
			public void Regular_expression_must_match(string input)
			{
				Assert.That(input, Is.StringMatching("^[" + CommonRegexPatterns.UpAlpha + "]+$"));
			}
		}

		[TestFixture]
		public class When_using_regex_pattern_for_list_of_elements
		{
			[Test]
			[TestCase("A", 0, 1, "", "A")]
			[TestCase("A", 1, 1, "      A")]
			[TestCase("A", 0, null, "", "A", "A,A", "A ,\r\n A,A,A", "A,A,A,A,A,A,A,A,A,A,A,A,A,A,A,A,A,A")]
			[TestCase("A", 0, 5, "", "A", "A,A", "A,A,A", "A,A,A,A", "A,A,A,A,A")]
			[TestCase("A", 3, 5, "A,A,A", "A,A,A,A", "A,A,A,A,A")]
			[TestCase("A", 5, 5, "A,A,A,A,A")]
			public void Regular_expression_must_match(object[] arguments)
			{
				var elementRegexPattern = (string)arguments[0];
				var minimum = (int)arguments[1];
				var maximum = (int?)arguments[2];
				string regexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(elementRegexPattern, minimum, maximum));
				IEnumerable<string> inputs = arguments.Where((arg, i) => i >= 3).Select(arg => (string)arg);

				foreach (string input in inputs)
				{
					Assert.That(input, Is.StringMatching(regexPattern));
				}
			}

			[Test]
			[TestCase("A", 0, 1, "A,A", "A,A,A")]
			[TestCase("A", 1, 1, "", "A,A", "A,A,A")]
			[TestCase("A", 0, 5, "A,A,A,A,A,A", "A,A,A,A,A,A,A,A")]
			[TestCase("A", 3, 5, "", "A", "A,A", "A,A,A,A,A,A")]
			[TestCase("A", 5, 5, "", "A", "A,A", "A,A,A", "A,A,A,A", "A,A,A,A,A,A")]
			public void Regular_expression_must_not_match(object[] arguments)
			{
				var elementRegexPattern = (string)arguments[0];
				var minimum = (int)arguments[1];
				var maximum = (int?)arguments[2];
				string regexPattern = String.Format("^{0}$", CommonRegexPatterns.ListOfElements(elementRegexPattern, minimum, maximum));
				IEnumerable<string> inputs = arguments.Where((arg, i) => i >= 3).Select(arg => (string)arg);

				foreach (string input in inputs)
				{
					Assert.That(input, Is.Not.StringMatching(regexPattern));
				}
			}
		}
	}
}