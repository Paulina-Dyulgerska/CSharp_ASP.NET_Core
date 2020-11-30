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

            var context = (IContentCheckService)validationContext.GetService(typeof(IContentCheckService));
            var articleEntity = context.ArticleEntityCheck(value.ToString());

            if (!articleEntity)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
