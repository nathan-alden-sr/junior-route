using System;
using System.Reflection;

using Junior.Common;

namespace Junior.Route.AutoRouting.NamingStrategies
{
	public class ClassNamespaceAndClassNameAndMethodNameStrategy : INamingStrategy
	{
		public NamingResult Name(Type type, MethodInfo method)
		{
			type.ThrowIfNull("type");
			method.ThrowIfNull("method");

			string name = String.Format("{0}.{1}", type.FullName, method.Name);

			return NamingResult.RouteNamed(name);
		}
	}
}