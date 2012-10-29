using System;
using System.Text.RegularExpressions;

using Junior.Common;

namespace Junior.Route.Http.RequestHeaders
{
	public class HostHeader
	{
		private const string RegexPattern = "^" + CommonRegexPatterns.Hostport + "$";
		private readonly string _host;
		private readonly ushort? _port;

		private HostHeader(string host, ushort? port)
		{
			host.ThrowIfNull("host");

			_host = host;
			_port = port;
		}

		public string Host
		{
			get
			{
				return _host;
			}
		}

		public ushort? Port
		{
			get
			{
				return _port;
			}
		}

		public static HostHeader Parse(string headerValue)
		{
			if (headerValue == null)
			{
				return null;
			}

			if (!Regex.IsMatch(headerValue, RegexPattern))
			{
				return null;
			}

			string[] hostAndPortParts = headerValue.Split(':');

			return new HostHeader(hostAndPortParts[0], hostAndPortParts.Length == 2 ? UInt16.Parse(hostAndPortParts[1]) : (ushort?)null);
		}
	}
}