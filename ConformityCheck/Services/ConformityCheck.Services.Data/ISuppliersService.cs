namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;

    using ConformityCheck.Services.Data.Models;

    public interface ISuppliersService : IService
    {
        //void Create(string number, string name);

        //void AddArticle(int supplierId, int articleId);

        //void DeleteArticle(int supplierId, int articleId);

        //IEnumerable<ArticleExportDTO> ListArticles(int articleId);

        //IEnumerable<ConformityImportDTO> ListConformities(int articleId);

        //void UpdateSupplierInformation(int supplierId);

        //void DeleteSupplier(int supplierId);

        //void AddConformity(int supplierId); //shte slaga na vsichki negovi articuli, tova conformity!!!! Otdelno shte go zakacha za supplier-a!
        ////trqbwa da pravq proverka dali pri kachvaneto na article, veche ne e potvyrdeno towa conformity i ako dostavchika go e potvyrdil,
        ////da go zakacham i pri kachvane na article syshto!!!!

        //IEnumerable<SupplierExportDTO> SearchSupplier(int supplierId);

        //IEnumerable<SupplierExportDTO> SearchByArticle(string articleNumber);

        //IEnumerable<SupplierExportDTO> SearchByConformity(string conformityType);

    }
}
