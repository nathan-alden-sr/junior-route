using System;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core
{
	public class HttpMethodPairMapper : PairMapper<HttpMethod, string>
	{
		public static readonly HttpMethodPairMapper Instance = new HttpMethodPairMapper();

		private HttpMethodPairMapper()
			: base(
				new Tuple<HttpMethod, string>(HttpMethod.Connect, "CONNECT"),
				new Tuple<HttpMethod, string>(HttpMethod.Delete, "DELETE"),
				new Tuple<HttpMethod, string>(HttpMethod.Get, "GET"),
				new Tuple<HttpMethod, string>(HttpMethod.Head, "HEAD"),
				new Tuple<HttpMethod, string>(HttpMethod.Post, "POST"),
				new Tuple<HttpMethod, string>(HttpMethod.Put, "PUT"),
				new Tuple<HttpMethod, string>(HttpMethod.Trace, "TRACE"))
		{
		}
	}
}