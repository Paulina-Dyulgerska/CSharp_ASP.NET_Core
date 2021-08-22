namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class NameRegExAttribute : ValidationAttribute
    {
        public NameRegExAttribute()
        {
            this.ErrorMessage = "The field Name could contain only letters. Number of letter should be at least 3.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var pattern = "^[a-zA-Z]{3,}$";
            var match = Regex.IsMatch(value.ToString(), pattern);

            if (!match)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
