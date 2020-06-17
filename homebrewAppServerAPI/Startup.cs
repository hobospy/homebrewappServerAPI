using AutoMapper;
using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Helpers;
using homebrewAppServerAPI.Persistence.Contexts;
using homebrewAppServerAPI.Persistence.Repositories;
using homebrewAppServerAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace homebrewAppServerAPI
{
    public class Startup
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
#if DEBUG
                log.Debug("Entered ConfigureServices (debug version)");

                services.AddCors();
#else
                log.Debug("Entered ConfigureServices (release version)");

                string[] whitelist = { "http://localhost:3000", "http://homebrew-react-app.s3-ap-southeast-2.amazonaws.com/" };
                services.AddCors(o => o.AddPolicy("MyCorsPolicy", builder =>
                {
                    //builder.SetIsOriginAllowed((host) => whitelist.Contains(host))
                    builder.SetIsOriginAllowed(_ => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                }));
#endif

                services.AddControllersWithViews().AddNewtonsoftJson();

                services.AddControllersWithViews(options =>
                {
                    options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                });

                services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
                services.AddMvc(options => options.Filters.Add(typeof(homebrewAPIExceptionFilter)));

#if USE_SQLITE
                log.Debug("Using SQLITE data source");
                services.AddDbContext<SqliteDbContext>(options =>
                {
#if DEBUG
                    options.EnableSensitiveDataLogging();
#endif
                    options.UseSqlite("Data Source=./homebrew.db");
                });
#else
                log.Debug("Using EF in memory data source");
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("homebrewapp-api-in-memory");
                });
#endif
                log.Debug("Adding services");
                services.AddScoped<IIngredientRepository, IngredientRepository>();
                services.AddScoped<IRecipeStepRepository, RecipeStepRepository>();
                services.AddScoped<IWaterProfileRepository, WaterProfileRepository>();
                services.AddScoped<IRecipeRepository, RecipeRepository>();
                services.AddScoped<IBrewRepository, BrewRepository>();
                services.AddScoped<IIngredientService, IngredientService>();
                services.AddScoped<IRecipeStepService, RecipeStepService>();
                services.AddScoped<IWaterProfileService, WaterProfileService>();
                services.AddScoped<IRecipeService, RecipeService>();
                services.AddScoped<IBrewService, BrewService>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();

                log.Debug("Adding auto mapper");
                services.AddAutoMapper(typeof(Startup));
            }
            catch (System.Exception ex)
            {
                log.Error(ex.Message);
                if (ex.InnerException != null )
                    log.Error(ex.InnerException);
            }
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            log.Debug("Entered Configure");

            app.UseOptions();

#if DEBUG
            app.UseCors(options => options.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());
#else
            log.Debug("Using configured cors policy ('MyCorsPolicy')");
            app.UseCors("MyCorsPolicy");
#endif

            if (env.IsDevelopment())
            {
                log.Debug("Environment is development mode");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                log.Debug("Environment is not development mode");
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            log.Debug("Using MVC");
            app.UseMvc();
        }
    }
}
