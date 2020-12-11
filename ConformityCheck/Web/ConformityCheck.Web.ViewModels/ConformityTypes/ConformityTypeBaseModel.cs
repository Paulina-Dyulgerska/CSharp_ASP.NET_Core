﻿namespace ConformityCheck.Web.ViewModels.ConformityTypes
{
    using System.ComponentModel.DataAnnotations;

    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public abstract class ConformityTypeBaseModel : IMapFrom<ConformityType>
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9]+[a-zA-Z0-9 _-]*$", ErrorMessage = "The field Description could contain only letters, digits, '-', '_' or ' '.")]
        //[DescriptionRegExAttribute]
        [DescriptionDublicatedAttribute]
        public string Description { get; set; }
    }
}