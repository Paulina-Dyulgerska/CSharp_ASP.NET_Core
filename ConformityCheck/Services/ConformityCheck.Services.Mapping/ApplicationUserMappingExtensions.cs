namespace ConformityCheck.Services.Mapping
{
    using System;

    using ConformityCheck.Data.Models;

    public static class ApplicationUserMappingExtensions
    {
        public static TDestination To<TDestination>(this ApplicationUser source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return AutoMapperConfig.MapperInstance.Map<TDestination>(source);
        }
    }
}
