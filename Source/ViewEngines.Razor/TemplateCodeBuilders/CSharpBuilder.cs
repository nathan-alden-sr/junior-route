using System;
using System.Linq;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public class CSharpBuilder : TemplateCodeBuilder
	{
		public CSharpBuilder(bool throwExceptionOnParserError = true)
			: base(new CSharpCodeLanguage(throwExceptionOnParserError))
		{
		}

		protected override string MakeGlobalNamespace(string @namespace)
		{
			@namespace.ThrowIfNull("namespace");

			return "global::" + @namespace;
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

			return String.Format("{0}.{1}<{2}>", @namespace, typeName, dynamic ? "dynamic" : String.Join(", ", type.GetGenericArguments().Select(MakeTypeName)));
		}
	}
}