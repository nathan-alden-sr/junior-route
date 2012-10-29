using System;

using Junior.Common;

namespace Junior.Route.AutoRouting.NamingStrategies.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class NameAttribute : Attribute
	{
		private readonly string _name;

		public NameAttribute(string name)
		{
			name.ThrowIfNull("name");

			_name = name;
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}
	}
}