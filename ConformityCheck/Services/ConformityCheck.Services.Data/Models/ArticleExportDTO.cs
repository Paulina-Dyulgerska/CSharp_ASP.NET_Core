namespace ConformityCheck.Services.Data.Models
{
    public class ArticleExportDTO
    {
        public string Number { get; set; }

        public string Description { get; set; }

        public bool IsConfirmed { get; set; } //confirmed - not confirmed
    }
}
