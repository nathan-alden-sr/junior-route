namespace Junior.Route.Routing.AntiCsrf.Validators
{
	public enum ValidationResult
	{
		ValidationDisabled,
		ValidationSkipped,
		FormFieldMissing,
		CookieMissing,
		TokensMatch,
		TokensDoNotMatch
	}
}