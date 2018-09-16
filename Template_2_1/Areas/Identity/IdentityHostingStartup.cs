using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Template_2_1.Areas.Identity.IdentityHostingStartup))]
namespace Template_2_1.Areas.Identity
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