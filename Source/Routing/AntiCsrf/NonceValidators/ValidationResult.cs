namespace Junior.Route.Routing.AntiCsrf.NonceValidators
{
	public enum ValidationResult
	{
		ValidationDisabled,
		ValidationSkipped,
		FormFieldMissing,
		FormFieldInvalid,
		CookieMissing,
		CookieInvalid,
		NonceValid,
		NonceInvalid
	}
}