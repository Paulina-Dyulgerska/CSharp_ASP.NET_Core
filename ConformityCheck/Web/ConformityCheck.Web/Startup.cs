namespace ConformityCheck.Web
{
    using System;
    using System.Reflection;

    using Azure.Storage.Blobs;
    using ConformityCheck.Data;
    using ConformityCheck.Data.Common;
    using ConformityCheck.Data.Common.Repositories;
    using ConformityCheck.Data.Models;
    using ConformityCheck.Data.Repositories;
    using ConformityCheck.Data.Seeding;
    using ConformityCheck.Services;
    using ConformityCheck.Services.Data;
    using ConformityCheck.Services.Mapping;
    using ConformityCheck.Services.Messaging;
    using ConformityCheck.Web.Filters;
    using ConformityCheck.Web.Infrastructure.Settings;
    using ConformityCheck.Web.Middlewares.ExceptionHandler;
    using ConformityCheck.Web.ViewModels;
    using ConformityCheck.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    this.configuration.GetConnectionString("DefaultConnection"))); // o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddLogging(loggingBuilder =>
            {
                var loggingSection = this.configuration.GetSection("Logging");
                loggingBuilder.AddFile(loggingSection);

                // loggingBuilder.AddConsole();
            });

            services.Configure<ReCaptchaSettings>(this.configuration.GetSection(ReCaptchaSettings.ReCaptcha));

            var facebookSettingsSection = this.configuration.GetSection(FacebookLoginSettings.FacebookLogin);
            services.Configure<FacebookLoginSettings>(facebookSettingsSection);
            var facebookSettings = facebookSettingsSection.Get<FacebookLoginSettings>();

            var emailSettingsSection = this.configuration.GetSection(EmailSettings.EmailSetting);
            services.Configure<EmailSettings>(emailSettingsSection);
            var emailSettings = emailSettingsSection.Get<EmailSettings>();

            services.AddAuthentication()
                    .AddFacebook(options =>
                    {
                        // options.AppId = this.configuration["FacebookLogin:AppId"];
                        // options.AppSecret = this.configuration["FacebookLogin:AppSecret"];
                        options.AppId = facebookSettings.AppId;
                        options.AppSecret = facebookSettings.AppSecret;
                    });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // options.MinimumSameSitePolicy = SameSiteMode.None;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
                options.ConsentCookie.IsEssential = true;
                options.ConsentCookie.Expiration = TimeSpan.FromDays(30);
                options.CheckConsentNeeded = context => true;
            });

            services.AddDistributedSqlServerCache(
                        options =>
                        {
                            options.ConnectionString = this.configuration.GetConnectionString("DefaultConnection");
                            options.SchemaName = "dbo";
                            options.TableName = "DistributedCache";
                        });
            services.AddSession();

            services
                .AddControllersWithViews(options =>
                            {
                                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                                options.Filters.Add(new ExceptionFilter());
                            })
                .AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter(); // for app.UseMigrationsEndPoint()
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddSingleton(this.configuration);
            services.AddSingleton(new BlobServiceClient(this.configuration.GetConnectionString("BlobConnection")));

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IArticlesSeedService, ArticlesSeedService>();
            services.AddTransient<ISuppliersSeedService, SuppliersSeedService>();
            services.AddTransient<IConformityTypesSeedService, ConformityTypesSeedService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IArticlesService, ArticlesService>();
            services.AddTransient<IConformityTypesService, ConformityTypesService>();
            services.AddTransient<IGetCountsService, GetCountsService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISubstancesService, SubstancesService>();
            services.AddTransient<ISuppliersService, SuppliersService>();
            services.AddTransient<IContentDeliveryService, ContentDeliveryService>();
            services.AddTransient<IConformitiesService, ConformitiesService>();
            services.AddTransient<IContactFormEntriesService, ContactFormEntriesService>();
            services.AddTransient<IContentCheckService, ContentCheckService>();

            // services.AddTransient<IEmailSender, NullMessageSender>();
            // services.AddTransient<IEmailSender>(serviceProvider => new SendGridEmailSender(this.configuration["EmailSettings:ApiKey"]));
            services.AddTransient<IEmailSender>(serviceProvider =>
                        new SendGridEmailSender(emailSettings.ApiKey, serviceProvider.GetRequiredService<ILogger<SendGridEmailSender>>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // will register all classes that have IMapFrom<T>, IMapTo<T>, IHaveCustomMapping
            // must be the first thing done here, reflection will collect all models with the above
            // interfaces and will register the mappings in my AutoMapper instance:
            // AutoMapperConfig.RegisterMappings(typeof(ApplicationUser).GetTypeInfo().Assembly);
            // AutoMapperConfig.RegisterMappings(
            //    typeof(ErrorViewModel).GetTypeInfo().Assembly,
            //    typeof(ApplicationUser).GetTypeInfo().Assembly); //if I need model from this assembly
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            //// Seed data on application startup
            // using (var serviceScope = app.ApplicationServices.CreateScope())
            // {
            //     var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //     dbContext.Database.Migrate();
            //     new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            // }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // 400...

                // app.UseDatabaseErrorPage(); is obsolete
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseApiExceptionHandler(new LoggerFactory());
                app.UseExceptionHandler("/Home/Error"); // 500...

                // the default HSTS value is 30 days. If you need more, change it.
                app.UseHsts();
            }

            // app.UseStatusCodePagesWithRedirects("/Home/StatusCodeError?errorCode={0}");
            app.UseStatusCodePagesWithReExecute("/Home/StatusCodeError", "?errorCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
