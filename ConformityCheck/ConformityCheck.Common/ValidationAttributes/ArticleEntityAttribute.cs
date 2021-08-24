namespace ConformityCheck.Common.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Services;

    public class ArticleEntityAttribute : ValidationAttribute
    {
        public ArticleEntityAttribute(bool allowNull = false)
        {
            this.ErrorMessage = $"No such article.";
            this.AllowNull = allowNull;
        }

        public bool AllowNull { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && this.AllowNull)
            {
                return ValidationResult.Success;
            }

            if (value == null && !this.AllowNull)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));
            var articleEntity = context.ArticleEntityIdCheck(value.ToString());

            if (!articleEntity)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
