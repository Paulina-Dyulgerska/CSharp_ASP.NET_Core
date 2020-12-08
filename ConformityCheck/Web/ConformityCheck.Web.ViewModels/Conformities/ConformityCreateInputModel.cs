namespace ConformityCheck.Web.ViewModels.Conformities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ConformityCreateInputModel : ConformityBaseModel
    {
        public bool ValidForAllArticles { get; set; }

        public bool ValidForSingleArticle { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ValidForSingleArticle && this.ArticleId == null)
            {
                yield return new ValidationResult("Please choose single article or change the conformity coverage.");
            }

            if (this.ValidForAllArticles && this.ArticleId != null)
            {
                yield return new ValidationResult("Please choose single article or change the conformity coverage.");
            }

            if (this.ValidForSingleArticle && this.ValidForAllArticles)
            {
                yield return new ValidationResult("Please choose one of the options for conformity coverage.");
            }

            if (!this.ValidForSingleArticle && !this.ValidForAllArticles)
            {
                yield return new ValidationResult("Please choose one of the options for conformity coverage.");
            }
        }
    }
}
