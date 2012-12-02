using System;
using System.Web.Razor.Parser.SyntaxTree;

namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public class TemplateParsingException : ApplicationException
	{
		private readonly int _characterIndex;
		private readonly int _lineIndex;

		internal TemplateParsingException(RazorError error)
			: base(error.Message)
		{
			_characterIndex = error.Location.CharacterIndex;
			_lineIndex = error.Location.LineIndex;
		}

		public int CharacterIndex
		{
			get
			{
				return _characterIndex;
			}
		}

		public int LineIndex
		{
			get
			{
				return _lineIndex;
			}
		}
	}
}