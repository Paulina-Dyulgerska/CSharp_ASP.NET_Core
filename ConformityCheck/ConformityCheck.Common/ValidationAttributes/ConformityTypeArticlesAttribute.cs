namespace ConformityCheck.Common.ValidationAttributes
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class ConformityTypeArticlesAttribute : ValidationAttribute
    {
        public ConformityTypeArticlesAttribute()
        {
            this.ErrorMessage = $"Cannot delete conformity type with articles assigned to it.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));

            var hasArticles = context.ConformityTypeArticlesCheck(int.Parse(value.ToString()));

            if (hasArticles)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
