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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            //        );
            //});
            //services.AddControllers();

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            //app.UseCors(options => options.WithOrigins("http://localhost:3000").AllowAnyMethod());
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod());
            //app.UseCors(options => options.WithOrigins("http://192.168.1.*:3000").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyMethod());
            //app.UseCors(builder =>
            //            builder.SetIsOriginAllowedToAllowWildcardSubdomains()
            app.UseMvc();
        }
    }
}
