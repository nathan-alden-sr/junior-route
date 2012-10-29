using System;

namespace Junior.Route.Http.RequestHeaders
{
	public class ContentLengthHeader
	{
		private readonly ulong _contentLength;

		private ContentLengthHeader(ulong contentLength)
		{
			_contentLength = contentLength;
		}

		public ulong ContentLength
		{
			get
			{
				return _contentLength;
			}
		}

		public static ContentLengthHeader Parse(string headerValue)
		{
			if (headerValue == null)
			{
				return null;
			}

			ulong contentLength;

			return UInt64.TryParse(headerValue, out contentLength) ? new ContentLengthHeader(contentLength) : null;
		}
	}
}