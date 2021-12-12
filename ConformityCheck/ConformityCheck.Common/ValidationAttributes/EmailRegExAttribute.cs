namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class EmailRegExAttribute : ValidationAttribute
    {
        public EmailRegExAttribute(string errorMessage = null)
        {
            this.ErrorMessage = errorMessage ?? "The field is not a valid e-mail address.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var pattern = "^(?:[a-zA-Z0-9][a-zA-Z0-9_.-]+@(?:[a-zA-Z0-9-_]{2,}[.][a-zA-Z0-9-_]{2,}))(?:.[a-zA-Z0-9-_]{2,})?$";
            var match = Regex.IsMatch(value.ToString(), pattern);

            if (!match)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
