namespace Junior.Route.AutoRouting.RestrictionMappers.Attributes
{
	public class FormContentTypeAttribute : HeaderAttribute
	{
		public FormContentTypeAttribute()
			: base("Content-Type", "^(application/x-www-form-urlencoded|multipart/form-data)(;|$)", RequestValueComparer.CaseInsensitiveRegex)
		{
		}
	}
}