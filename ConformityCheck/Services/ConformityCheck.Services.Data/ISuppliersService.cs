namespace ConformityCheck.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ConformityCheck.Web.ViewModels.Suppliers;

    public interface ISuppliersService : IService<string>
    {
        Task CreateAsync(SupplierCreateInputModel input, string userId);

        Task EditAsync(SupplierEditInputModel input, string userId);

        Task<IEnumerable<T>> GetArticlesByIdAsync<T>(string id);

        Task<SupplierDetailsModel> DetailsByIdAsync(string id);

        //void AddConformity(int supplierId); //shte slaga na vsichki negovi articuli, tova conformity!!!! Otdelno shte go zakacha za supplier-a!
        ////trqbwa da pravq proverka dali pri kachvaneto na article, veche ne e potvyrdeno towa conformity i ako dostavchika go e potvyrdil,
        ////da go zakacham i pri kachvane na article syshto!!!!
    }
}
