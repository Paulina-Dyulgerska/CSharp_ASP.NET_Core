﻿namespace ConformityCheck.Web.ViewModels.Suppliers
{
    using AutoMapper;

    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class SupplierFullInfoModel : SupplierBaseModel/*, IHaveCustomMappings*/
    {
        public string Id { get; set; }

        public string ContactPersonName => this.ContactPersonFirstName + " " + this.ContactPersonLastName;

        // AutoMapper-a moje da vzeme ot Supplier.ArticleSuppliers.Count() i da mi go dade
        // direktno tuk, bez da pravq custom mapping!!! Ako spazwam conventiona za imenata na 
        // propertytata, toj shte se opravi s neshta kato Count, Min, Max i t.n.
        public int ArticleSuppliersCount { get; set; }
        //public int ArticlesCount { get; set; }

        //public int ArticlesConfirmed{ get; set; }

        //public int ArticlesUnconfirmed { get; set; }

        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<Supplier, SupplierFullInfoModel>()
        //        .ForMember(x => x.ArticlesCount, opt => opt.MapFrom(s => s.ArticleSuppliers.Count));
        //}
    }
}
