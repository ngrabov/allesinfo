using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(allinfo.Areas.Identity.IdentityHostingStartup))]
namespace allinfo.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}