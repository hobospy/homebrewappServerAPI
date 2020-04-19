using AutoMapper;
using homebrewAppServerAPI.Domain.ExceptionHandling;
using homebrewAppServerAPI.Domain.Repositories;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Persistence.Contexts;
using homebrewAppServerAPI.Persistence.Repositories;
using homebrewAppServerAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace homebrewAppServerAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder
            //            .SetIsOriginAllowedToAllowWildcardSubdomains()
            //            .WithOrigins("http://192.168.1.*:3000")
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //        );
            //});

            //services.AddControllers(setupAction =>
            //setupAction.ReturnHttpNotAcceptable = true).AddXmlDataContractSerializerFormatters().AddNewtonsoftJson(setupAction =>
            //setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
            services.AddControllersWithViews().AddNewtonsoftJson();

            services.AddControllersWithViews(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            });

            //services.AddControllers().AddNewtonsoftJson();


            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMvc(options => options.Filters.Add(typeof(homebrewAPIExceptionFilter)));

#if USE_SQLITE
            services.AddDbContext<SqliteDbContext>(options =>
            {
                options.UseSqlite("Data Source=./homebrew.db");
            });
#else
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("homebrewapp-api-in-memory");
            });
#endif

            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IBrewRepository, BrewRepository>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IBrewService, BrewService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(Startup));
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(options => options.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader());
            //app.UseCors("AllowCors");
            //app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod());
            app.UseMvc();
        }
    }
}
