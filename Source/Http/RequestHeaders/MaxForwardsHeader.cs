using System;
using System.Text.RegularExpressions;

namespace Junior.Route.Http.RequestHeaders
{
	public class MaxForwardsHeader
	{
		private const string RegexPattern = "^[" + CommonRegexPatterns.Digit + "]+$";
		private readonly int _maxForwards;

		private MaxForwardsHeader(int maxForwards)
		{
			_maxForwards = maxForwards;
		}

		public int MaxForwards
		{
			get
			{
				return _maxForwards;
			}
		}

		public static MaxForwardsHeader Parse(string headerValue)
		{
			if (headerValue == null)
			{
				return null;
			}

			return Regex.IsMatch(headerValue, RegexPattern) ? new MaxForwardsHeader(Int32.Parse(headerValue)) : null;
		}
	}
}