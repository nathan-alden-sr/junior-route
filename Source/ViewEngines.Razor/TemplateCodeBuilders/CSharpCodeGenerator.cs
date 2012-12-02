using System.Web.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser.SyntaxTree;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public class CSharpCodeGenerator : CSharpRazorCodeGenerator
	{
		private readonly bool _throwExceptionOnParserError;

		public CSharpCodeGenerator(string className, string rootNamespaceName, string sourceFileName, RazorEngineHost host, bool throwExceptionOnParserError = true)
			: base(className, rootNamespaceName, sourceFileName, host)
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

		public override void VisitError(RazorError err)
		{
			err.ThrowIfNull("err");

			if (_throwExceptionOnParserError)
			{
				throw new TemplateParsingException(err);
			}
		}
	}
}