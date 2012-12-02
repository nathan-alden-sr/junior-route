using System;

namespace Junior.Route.ViewEngines.Razor
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class DynamicModelAttribute : Attribute
	{
	}
}