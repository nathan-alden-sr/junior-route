using System.Web.Razor;
using System.Web.Razor.Generator;

namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public class CSharpCodeLanguage : CSharpRazorCodeLanguage
	{
		private readonly bool _throwExceptionOnParserError;

		public CSharpCodeLanguage(bool throwExceptionOnParserError = true)
		{
			_throwExceptionOnParserError = throwExceptionOnParserError;
		}

		public bool ThrowExceptionOnParserError
		{
			get
			{
				return _throwExceptionOnParserError;
			}
		}

		public override RazorCodeGenerator CreateCodeGenerator(string className, string rootNamespaceName, string sourceFileName, RazorEngineHost host)
		{
			return new CSharpCodeGenerator(className, rootNamespaceName, sourceFileName, host, _throwExceptionOnParserError);
		}
	}
}