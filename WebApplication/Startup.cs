using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace WebApplication
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
			services.Configure<WebEncoderOptions>(options => { options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All); });

			services.AddControllersWithViews();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment() == true) { app.UseDeveloperExceptionPage(); }
			else
			{
				app.UseExceptionHandler("/error");
				app.UseHsts();
			}

			app.UseRouting();
			app.UseStaticFiles();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{action}",
					defaults: new { controller = "Home", action = "Index" }
				);
			});
		}
	}
}