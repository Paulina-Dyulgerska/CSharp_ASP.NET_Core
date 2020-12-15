namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class ArticleNumberDublicatedAttribute : ValidationAttribute
    {
        public ArticleNumberDublicatedAttribute()
        {
            this.ErrorMessage = $"There is already an article with this number.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));

            var hasNumber = context
                .ArticleEntityNumberCheck(value.ToString());

            if (hasNumber)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
