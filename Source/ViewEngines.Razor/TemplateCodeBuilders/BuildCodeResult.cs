using System.CodeDom;

using Junior.Common;

namespace Junior.Route.ViewEngines.Razor.TemplateCodeBuilders
{
	public class BuildCodeResult
	{
		private readonly CodeCompileUnit _compileUnit;
		private readonly string _typeFullName;

		public BuildCodeResult(CodeCompileUnit compileUnit, string typeFullName)
		{
			compileUnit.ThrowIfNull("compileUnit");
			typeFullName.ThrowIfNull("TypeFullName");

			_compileUnit = compileUnit;
			_typeFullName = typeFullName;
		}

		public CodeCompileUnit CompileUnit
		{
			get
			{
				return _compileUnit;
			}
		}

		public string TypeFullName
		{
			get
			{
				return _typeFullName;
			}
		}
	}
}