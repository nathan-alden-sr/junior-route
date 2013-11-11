namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	public class FormContentTypeHeaderRestrictionAttribute : HeaderRestrictionAttribute
	{
		public FormContentTypeHeaderRestrictionAttribute()
			: base("Content-Type", "^(application/x-www-form-urlencoded|multipart/form-data)(;|$)", RequestValueComparer.CaseInsensitiveRegex)
		{
		}
	}
}