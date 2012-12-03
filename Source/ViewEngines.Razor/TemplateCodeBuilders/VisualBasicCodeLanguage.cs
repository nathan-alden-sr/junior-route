using System.Web.Razor;
using System.Web.Razor.Generator;

namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public class VisualBasicCodeLanguage : VBRazorCodeLanguage
	{
		private readonly bool _throwExceptionOnParserError;

		public VisualBasicCodeLanguage(bool throwExceptionOnParserError = true)
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
			return new VisualBasicCodeGenerator(className, rootNamespaceName, sourceFileName, host, _throwExceptionOnParserError);
		}
	}
}