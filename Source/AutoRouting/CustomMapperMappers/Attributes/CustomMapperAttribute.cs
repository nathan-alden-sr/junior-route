using System;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.CustomMapperMappers.Attributes
{
	public class CustomMapperAttribute : Attribute
	{
		private readonly ICustomMapper _mapper;

		public CustomMapperAttribute(Type customMapperType)
		{
			customMapperType.ThrowIfNull("customMapperType");

			if (customMapperType.IsNotPublic)
			{
				throw new ArgumentException("Type must be public.", "customMapperType");
			}
			if (customMapperType.IsAbstract)
			{
				throw new ArgumentException("Type cannot be abstract or static.", "customMapperType");
			}
			if (!customMapperType.ImplementsInterface<ICustomMapper>())
			{
				throw new ArgumentException(String.Format("Type must implement {0}.", customMapperType.FullName), "customMapperType");
			}

			ConstructorInfo constructorInfo = customMapperType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);

			if (constructorInfo == null)
			{
				throw new ArgumentException("Type must declare a public default constructor.", "customMapperType");
			}

			_mapper = (ICustomMapper)Activator.CreateInstance(customMapperType);
		}

		public void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			_mapper.Map(route, container);
		}
	}
}