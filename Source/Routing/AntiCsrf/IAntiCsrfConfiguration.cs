using System;

namespace Junior.Route.Routing.AntiCsrf
{
	public interface IAntiCsrfConfiguration
	{
		bool Enabled
		{
			get;
		}

		bool ValidateHttpPost
		{
			get;
		}

		bool ValidateHttpPut
		{
			get;
		}

		bool ValidateHttpDelete
		{
			get;
		}

		string CookieName
		{
			get;
		}

		string FormFieldName
		{
			get;
		}

		TimeSpan NonceDuration
		{
			get;
		}

		string MemoryCacheName
		{
			get;
		}
	}
}