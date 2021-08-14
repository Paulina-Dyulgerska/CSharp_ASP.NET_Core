using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ConformityCheck.Web.Areas.Identity.IdentityHostingStartup))]

namespace ConformityCheck.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
