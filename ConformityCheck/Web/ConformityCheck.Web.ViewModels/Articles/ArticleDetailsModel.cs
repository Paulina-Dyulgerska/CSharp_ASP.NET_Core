namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ArticleDetailsModel : ArticleEditInputModel, IHaveCustomMappings
    {
        public bool IsConfirmed => this.Suppliers.Count() > 0 && this.Suppliers.All(x => x.HasAllConformed);

        //gyrmi mi instanciqta za nullna Suppliers, ako ne kaja izrishno na AutoMapper-a kak da mapva ot Article kym tozi class!!!!
        //ne moga da ostavq samo ArticleEditModel da iznese mappvaneto, a trqbwa i tuk da go opisha, inache 
        //mi hvyrlq null za value na suppliers!
        public override void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Article, ArticleDetailsModel>()
                .ForMember(
                x => x.Suppliers,
                opt => opt.MapFrom(a => a.ArticleSuppliers
                .OrderByDescending(x => x.IsMainSupplier)
                .ThenBy(x => x.Supplier.Name)))
                .ForMember(
                x => x.ConformityTypes,
                opt => opt.MapFrom(a => a.ArticleConformityTypes
                .OrderBy(x => x.Id)))
                .ForMember(
                x => x.Conformities,
                opt => opt.MapFrom(a => a.Conformities
                .OrderBy(x => x.Id)))
                //.ForMember(
                //x => x.ArticleConformityTypes,
                //opt => opt.MapFrom(a => a.ArticleConformityTypes.Select(x =>
                //$"{x.ConformityType.Description} => {x.Conformity.IsAccepted}").ToList()))
                //.ForMember(
                //x => x.ArticleMissingConformityTypes,
                //opt => opt.MapFrom(a => a.ArticleConformityTypes.Select(x =>
                //$"{x.ConformityType.Description} => {x.Conformity != null}").ToList()))
                ;
        }
    }
}
