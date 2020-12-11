﻿namespace ConformityCheck.Web.ViewModels.Articles
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Web.ViewModels.Conformities;
    using ConformityCheck.Web.ViewModels.ConformityTypes;
    using ConformityCheck.Web.ViewModels.Suppliers;

    public class ArticleDetailsModel :/* ArticleBaseModel, IHaveCustomMappings*/ ArticleEditModel, IHaveCustomMappings
    {
        public ArticleDetailsModel()
        {
        }
        //private IEnumerable<SupplierModel> suppliers;

        //[ArticleEntityAttribute]
        //public string Id { get; set; }

        public bool IsConfirmed => this.Suppliers.Count() > 0 && this.Suppliers.All(x => x.HasAllConformed);

        //public IEnumerable<SupplierModel> Suppliers
        //{
        //    get
        //    {
        //        return this.suppliers;
        //    }

        //    set
        //    {
        //        this.suppliers = value;

        //        foreach (var item in this.suppliers)
        //        {
        //            item.HasAllConformed = true;

        //            foreach (var conformityType in this.ConformityTypes)
        //            {
        //                var conformity = this.Conformities
        //                    .FirstOrDefault(x => x.ConformityType.Id == conformityType.Id
        //                                    && x.Supplier.Id == item.Id);
        //                if (conformity == null || !conformity.IsAccepted || !conformity.IsValid)
        //                {
        //                    item.HasAllConformed = false;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        //public IEnumerable<ConformityTypeModel> ConformityTypes { get; set; }

        //public IEnumerable<ConformityModel> Conformities { get; set; }

        //public IEnumerable<string> Products { get; set; }

        //public IEnumerable<string> Substances { get; set; }

        ////public IEnumerable<string> ArticleMissingConformityTypes { get; set; }

        ////public IEnumerable<string> ArticleConformityTypes { get; set; }

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