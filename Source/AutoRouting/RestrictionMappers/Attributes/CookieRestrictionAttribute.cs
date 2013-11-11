using System;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class CookieRestrictionAttribute : RestrictionAttribute
	{
		private readonly string _name;
		private readonly RequestValueComparer? _nameComparer;
		private readonly bool _optional;
		private readonly string _value;
		private readonly RequestValueComparer? _valueComparer;

		public CookieRestrictionAttribute(string name, string value, bool optional = false)
		{
			name.ThrowIfNull("name");
			value.ThrowIfNull("value");

			_name = name;
			_value = value;
			_optional = optional;
		}

		public CookieRestrictionAttribute(string name, RequestValueComparer nameComparer, string value, RequestValueComparer valueComparer, bool optional = false)
		{
			name.ThrowIfNull("name");
			value.ThrowIfNull("value");

			_name = name;
			_nameComparer = nameComparer;
			_value = value;
			_valueComparer = valueComparer;
			_optional = optional;
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			if (_nameComparer != null && _valueComparer != null)
			{
				route.RestrictByCookie(_name, GetComparer(_nameComparer.Value), _value, GetComparer(_valueComparer.Value), _optional);
			}
			else
			{
				route.RestrictByCookie(_name, _value, _optional);
			}
		}
	}
}