using System.Diagnostics;
using System.Net;
using System.Web;

using Junior.Common;

namespace NathanAlden.JuniorRouting.Core.Responses
{
	[DebuggerDisplay("{_statusCode}.{_subStatusCode}")]
	public class NoContentResponse : IHttpRouteResponse
	{
		public static readonly NoContentResponse Default = new NoContentResponse(HttpStatusCode.NoContent);
		private readonly int _statusCode;
		private readonly int _subStatusCode;

		public NoContentResponse()
		{
		}

		public NoContentResponse(HttpStatusCode statusCode, int subStatusCode = 0)
		{
			_statusCode = (int)statusCode;
			_subStatusCode = subStatusCode;
		}

		public NoContentResponse(int statusCode, int subStatusCode = 0)
		{
			_statusCode = statusCode;
			_subStatusCode = subStatusCode;
		}

		public int ResponseStatusCode
		{
			get
			{
				return _statusCode;
			}
		}

		public HttpStatusCode? ParsedResponseStatusCode
		{
			get
			{
				return Enum<HttpStatusCode>.IsDefined(_statusCode) ? (HttpStatusCode?)_statusCode : null;
			}
		}

		public int ResponseSubStatusCode
		{
			get
			{
				return _subStatusCode;
			}
		}

		public virtual void WriteResponse(HttpResponseBase response)
		{
			response.ThrowIfNull("response");

			response.StatusCode = _statusCode;
			response.SubStatusCode = _subStatusCode;
		}

		public static NoContentResponse StatusCodes(HttpStatusCode statusCode, int subStatusCode = 0)
		{
			return new NoContentResponse(statusCode, subStatusCode);
		}

		public static NoContentResponse StatusCodes(int statusCode, int subStatusCode = 0)
		{
			return new NoContentResponse(statusCode, subStatusCode);
		}
	}
}