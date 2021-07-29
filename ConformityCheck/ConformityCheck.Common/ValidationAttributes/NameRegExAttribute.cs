namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class NameRegExAttribute : ValidationAttribute
    {
        public NameRegExAttribute()
        {
            this.ErrorMessage = "The field Name could contain only letters.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pattern = "[a-zA-Z]";
            var match = Regex.IsMatch(value.ToString(), pattern);

            if (!match)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
