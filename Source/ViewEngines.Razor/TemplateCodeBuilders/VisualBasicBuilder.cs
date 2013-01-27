using System;
using System.Linq;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public class VisualBasicBuilder : TemplateCodeBuilder
	{
		public VisualBasicBuilder(bool throwExceptionOnParserError = true)
			: base(new VisualBasicCodeLanguage(throwExceptionOnParserError))
		{
		}

		protected override string MakeGlobalNamespace(string @namespace)
		{
			@namespace.ThrowIfNull("namespace");

			return @namespace.StartsWith("Global.") ? @namespace : "Global." + @namespace;
		}

		protected override string MakeTypeName(Type type)
		{
			type.ThrowIfNull("type");

			if (type.IsGenericType)
			{
				return type.FullName;
			}

			bool dynamic = type.IsDynamicType() || type.IsAnonymousType();
			string @namespace = type.Namespace;
			string typeName = type.Name.Substring(0, type.Name.IndexOf('`'));

			return String.Format("{0}.{1}(Of {2})", @namespace, typeName, dynamic ? "Object" : String.Join(", ", type.GetGenericArguments().Select(MakeTypeName)));
		}
	}
}