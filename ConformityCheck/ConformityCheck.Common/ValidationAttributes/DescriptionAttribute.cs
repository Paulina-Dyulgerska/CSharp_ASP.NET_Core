namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class DescriptionAttribute : ValidationAttribute
    {
        public DescriptionAttribute()
        {
            this.ErrorMessage = "The field Description could contain only letters, digits, '-', '_' or ' '.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pattern = "^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$";
            var match = Regex.IsMatch(value.ToString(), pattern);

            if (!match)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
