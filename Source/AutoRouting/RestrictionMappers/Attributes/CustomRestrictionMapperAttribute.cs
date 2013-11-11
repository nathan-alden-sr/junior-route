using System;
using System.Reflection;

using Junior.Common;
using Junior.Route.AutoRouting.Containers;

namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	public class CustomRestrictionMapperAttribute : RestrictionAttribute
	{
		private readonly ICustomRestrictionMapper _mapper;

		public CustomRestrictionMapperAttribute(Type customRestrictionMapperType)
		{
			customRestrictionMapperType.ThrowIfNull("customRestrictionMapperType");

			if (customRestrictionMapperType.IsNotPublic)
			{
				throw new ArgumentException("Type must be public.", "customRestrictionMapperType");
			}
			if (customRestrictionMapperType.IsAbstract)
			{
				throw new ArgumentException("Type cannot be abstract or static.", "customRestrictionMapperType");
			}
			if (!customRestrictionMapperType.ImplementsInterface<ICustomRestrictionMapper>())
			{
				throw new ArgumentException(String.Format("Type must implement {0}.", customRestrictionMapperType.FullName), "customRestrictionMapperType");
			}

			ConstructorInfo constructorInfo = customRestrictionMapperType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);

			if (constructorInfo == null)
			{
				throw new ArgumentException("Type must declare a public default constructor.", "customRestrictionMapperType");
			}

			_mapper = (ICustomRestrictionMapper)Activator.CreateInstance(customRestrictionMapperType);
		}

		public override void Map(Routing.Route route, IContainer container)
		{
			route.ThrowIfNull("route");
			container.ThrowIfNull("container");

			_mapper.Map(route, container);
		}
	}
}