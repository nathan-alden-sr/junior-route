using System;

using Junior.Route.Common;

namespace Junior.Route.AutoRouting.SchemeMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class SchemeAttribute : Attribute
	{
		private readonly Scheme _scheme;

		public SchemeAttribute(Scheme scheme)
		{
			_scheme = scheme;
		}

		public Scheme Scheme
		{
			get
			{
				return _scheme;
			}
		}
	}
}