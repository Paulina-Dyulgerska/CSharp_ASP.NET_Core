namespace ConformityCheck.Web.ViewModels.Conformities
{
    using AutoMapper;
    using ConformityCheck.Common.ValidationAttributes;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Services.Mapping;

    public class ConformityEditInputModel : ConformityBaseModel, IHaveCustomMappings
    {
        [ConformityEntityAttribute(allowNull: true)]
        public string Id { get; set; }

        public string CallerViewName { get; set; }

        public override void CreateMappings(IProfileExpression configuration)
        {
            // /wwwroot/conformityFiles/conformities/jhdsi-343g3h453-=g34g.pdf
            configuration.CreateMap<Conformity, ConformityEditInputModel>()
                .ForMember(x => x.ConformityFileUrl, opt =>
                    opt.MapFrom(x =>
                        x.RemoteFileUrl != null ?
                        x.RemoteFileUrl :
                        "/files/conformities/" + x.Id + "." + x.FileExtension));
        }
    }
}
