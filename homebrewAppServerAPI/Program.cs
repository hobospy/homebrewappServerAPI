using homebrewAppServerAPI.Persistence.Contexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace homebrewAppServerAPI
{
    public class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
#if USE_SQLITE
                using (var context = scope.ServiceProvider.GetService<SqliteDbContext>())
                {
                    log.Debug("Using the Sqlite database context");
                    context.Database.EnsureCreated();
                }
#else
            using (var context = scope.ServiceProvider.GetService<AppDbContext>())
            {
                log.Debug("Using the EF in memory database context");
                context.Database.EnsureCreated();
            }
#endif
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
    }
}
