namespace ConformityCheck.Web.ViewModels.Conformities
{
    public class ConformityFileExportModel
    {
        public string FilePath { get; set; }

        public string FileContentType { get; set; }

        public byte[] FileBytes { get; set; }
    }
}
