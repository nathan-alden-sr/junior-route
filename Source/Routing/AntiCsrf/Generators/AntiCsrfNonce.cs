using System;

namespace Junior.Route.Routing.AntiCsrf.Generators
{
	public class AntiCsrfNonce
	{
		private readonly Guid _nonce;
		private readonly Guid _sessionId;

		public AntiCsrfNonce(Guid sessionId, Guid nonce)
		{
			_sessionId = sessionId;
			_nonce = nonce;
		}

		public Guid SessionId
		{
			get
			{
				return _sessionId;
			}
		}

		public Guid Nonce
		{
			get
			{
				return _nonce;
			}
		}
	}
}