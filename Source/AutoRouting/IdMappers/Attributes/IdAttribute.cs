using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.IdMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class IdAttribute : Attribute
	{
		private readonly Guid _id;

		public IdAttribute(string id)
		{
			id.ThrowIfNull("id");

			_id = Guid.Parse(id);
		}

		public Guid Id
		{
			get
			{
				return _id;
			}
		}
	}
}