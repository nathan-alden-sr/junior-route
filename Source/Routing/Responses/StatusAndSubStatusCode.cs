using System;
using System.Diagnostics;
using System.Net;
using System.Web;

using Junior.Common;

namespace Junior.Route.Routing.Responses
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class StatusAndSubStatusCode : IEquatable<StatusAndSubStatusCode>
	{
		private readonly int _statusCode;
		private readonly int _subStatusCode;

		public StatusAndSubStatusCode(int statusCode, int subStatusCode = 0)
		{
			_statusCode = statusCode;
			_subStatusCode = subStatusCode;
		}

		public StatusAndSubStatusCode(HttpStatusCode statusCode, int subStatusCode = 0)
			: this((int)statusCode, subStatusCode)
		{
		}

		public int StatusCode
		{
			get
			{
				return _statusCode;
			}
		}

		public HttpStatusCode? ParsedStatusCode
		{
			get
			{
				return Enum<HttpStatusCode>.IsDefined(_statusCode) ? (HttpStatusCode?)_statusCode : null;
			}
		}

		public int SubStatusCode
		{
			get
			{
				return _subStatusCode;
			}
		}

		public string StatusDescription
		{
			get
			{
				return HttpWorkerRequest.GetStatusDescription(_statusCode) ?? "";
			}
		}

		// ReSharper disable UnusedMember.Local
		private string DebuggerDisplay
			// ReSharper restore UnusedMember.Local
		{
			get
			{
				return String.Format("{0}.{1} ({2})", _statusCode, _subStatusCode, StatusDescription);
			}
		}

		public bool Equals(StatusAndSubStatusCode other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return _statusCode == other._statusCode && _subStatusCode == other._subStatusCode;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((StatusAndSubStatusCode)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (_statusCode * 397) ^ _subStatusCode;
			}
		}
	}
}