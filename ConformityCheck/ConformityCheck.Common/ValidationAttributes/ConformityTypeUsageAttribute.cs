namespace ConformityCheck.Common.ValidationAttributes
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class ConformityTypeUsageAttribute : ValidationAttribute
    {
        public ConformityTypeUsageAttribute()
        {
            this.ErrorMessage = $"Cannot delete conformity type with articles, products or conformities assigned to it.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));

            var hasArticles = context.ConformityTypeArticlesCheck(int.Parse(value.ToString()));
            var hasProducts = context.ConformityTypeProductsCheck(int.Parse(value.ToString()));
            var hasConformities = context.ConformityTypeConformitiesCheck(int.Parse(value.ToString()));

            if (hasArticles || hasProducts || hasConformities)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
