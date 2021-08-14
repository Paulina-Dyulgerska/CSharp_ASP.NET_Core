namespace ConformityCheck.Common.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    using ConformityCheck.Common;

    public class DateAttribute : ValidationAttribute
    {
        public DateAttribute(string minDate)
        {
            this.MinDate = DateTime.ParseExact(minDate, GlobalConstants.DateTimeFormat, CultureInfo.InvariantCulture);
            this.ErrorMessage = $"Date should be between {this.MinDate.Date.ToString(GlobalConstants.DateTimeFormat)} " +
                $"and {DateTime.UtcNow.Date.ToString(GlobalConstants.DateTimeFormat)}";
        }

        public DateTime MinDate { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue.ToUniversalTime() <= this.MinDate.ToUniversalTime()
                    || dateTimeValue.ToUniversalTime() > DateTime.UtcNow)
                {
                    return new ValidationResult(this.ErrorMessage);
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
