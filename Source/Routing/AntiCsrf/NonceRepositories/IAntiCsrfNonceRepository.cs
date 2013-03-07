using System;
using System.Threading.Tasks;

namespace Junior.Route.Routing.AntiCsrf.NonceRepositories
{
	public interface IAntiCsrfNonceRepository
	{
		Task Add(Guid sessionId, Guid nonce, DateTime createdUtcTimestamp, DateTime expiresUtcTimestamp);
		Task<bool> Exists(Guid sessionId, Guid nonce, DateTime currentUtcTimestamp);
	}
}