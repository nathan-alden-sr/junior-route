using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class UpgradeHeader
	{
		private static readonly string _elementsRegexPattern = String.Format("^(?:{0})$", CommonRegexPatterns.ListOfElements(CommonRegexPatterns.Product, 1));
		private readonly string _product;
		private readonly string _productVersion;

		private UpgradeHeader(string product, string productVersion)
		{
			product.ThrowIfNull("product");

			_product = product;
			_productVersion = productVersion;
		}

		public string Product
		{
			get
			{
				return _product;
			}
		}

		public string ProductVersion
		{
			get
			{
				return _productVersion;
			}
		}

		public static IEnumerable<UpgradeHeader> ParseMany(string headerValue)
		{
			if (headerValue == null)
			{
				return Enumerable.Empty<UpgradeHeader>();
			}

			return Regex.IsMatch(headerValue, _elementsRegexPattern)
				       ? headerValue.SplitElements()
					         .Select(arg => arg.Split('/'))
					         .Select(arg => new UpgradeHeader(arg[0], arg.Length == 2 ? arg[1] : null))
				       : Enumerable.Empty<UpgradeHeader>();
		}
	}
}