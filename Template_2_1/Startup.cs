using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template_2_1.Context.Context;
using Template_2_1.Context.IContext;
using Template_2_1.Mapper;
using Template_2_1.Services.IServices;
using Template_2_1.Services.Services;

namespace Template_2_1
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			// Context
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			// Identity
			services.AddDefaultIdentity<IdentityUser>( config => { config.SignIn.RequireConfirmedEmail = true; } )
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();
			// Identity - Scoped - For Roles - NOW You can use [Authorize(Roles = "Admin")] - attribute
			services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, UserClaimsPrincipalFactory<IdentityUser, IdentityRole>>();

			// Singleton
			services.AddSingleton(AutoMapperConfig.Initialize());
			services.AddSingleton<IContextLoggerService, ContextContextLoggerService>();

			// Scoped
			services.AddScoped<IDbInitializer, DbInitializer>();
			services.AddScoped<IEmailConfirmationCheckerService, EmailConfirmationCheckerService>();

			// Transient
			services.AddTransient<IEmailSender, EmailSender>(i =>
				new EmailSender(
					Configuration["EmailSender:Host"],
					Configuration.GetValue<int>("EmailSender:Port"),
					Configuration.GetValue<bool>("EmailSender:EnableSSL"),
					Configuration["EmailSender:UserName"],
					Configuration["EmailSender:Password"]
				)
			);

			// MVC
			services.AddMvc(options =>
			{
				options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
				options.Filters.Add(new RequireHttpsAttribute());
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Redirect to HTTPS (on server)
			services.AddHttpsRedirection(options =>
			{
				options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
				options.HttpsPort = 443;
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				
				// Strict-Transport-Security
				app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains().Preload());
				
				// X-Content-Type-Options
				app.UseXContentTypeOptions();

				// Referrer-Policy
				app.UseReferrerPolicy(opts => opts.NoReferrer());

				// X-XSS-Protection
				app.UseXXssProtection(options => options.EnabledWithBlockMode());

				// X-Frame-Options
				app.UseXfo(options => options.SameOrigin());

				// Content-Security-Policy
				app.UseCsp(opts => opts
					.BlockAllMixedContent()
					.StyleSources(s => s.Self().UnsafeInline())
					.FontSources(s => s.Self())
					.FormActions(s => s.Self())
					.FrameAncestors(s => s.Self())
					.ImageSources(s => s.Self())
					.ScriptSources(s => s.Self().UnsafeInline())
				);
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			// Default Route
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			// Database AutoInitial
			dbInitializer.Initialize();
		}
	}
}
