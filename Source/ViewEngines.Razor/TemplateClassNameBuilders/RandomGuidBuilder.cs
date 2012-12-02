using Junior.Common;

namespace Junior.Route.ViewEngines.Razor.TemplateClassNameBuilders
{
	public class RandomGuidBuilder : ITemplateClassNameBuilder
	{
		private readonly IGuidFactory _guidFactory;

		public RandomGuidBuilder(IGuidFactory guidFactory)
		{
			guidFactory.ThrowIfNull("guidFactory");

			_guidFactory = guidFactory;
		}

		public string BuildFromRandomGuid()
		{
			return "_" + _guidFactory.Random().ToString("N");
		}
	}
}