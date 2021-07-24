namespace ConformityCheck.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public int Articles { get; set; }

        public int Products { get; set; }

        public int Substances { get; set; }

        public int Suppliers { get; set; }

        public int Conformities { get; set; }

        public int ConformityTypes { get; set; }

        public int RegulationLists { get; set; }

        public string CurrentSearchInput { get; set; }

        // Search result entity id:
        public string Id { get; set; }
    }
}
