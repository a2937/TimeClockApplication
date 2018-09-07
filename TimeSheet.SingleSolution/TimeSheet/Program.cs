using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TimeSheet.Data;

namespace TimeSheet
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = BuildWebHost(args);



			/*using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var context = services.GetRequiredService<ApplicationDbContext>();
				context.Database.Migrate();

				// requires using Microsoft.Extensions.Configuration;
				var config = host.Services.GetRequiredService<IConfiguration>();
				// Set password with the Secret Manager tool.
				// dotnet user-secrets set SeedUserPW <pw>

				var testUserPw = config["SeedUserPW"];
				/*
				try
				{
					SeedData.Initialize(services, testUserPw).Wait();
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
					throw ex;
				}
				
			}
			*/
		
			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseKestrel()
				.UseDefaultServiceProvider(options =>
					options.ValidateScopes = false)
				.Build();
	}


	/*
public static IWebHost BuildWebHost(string[] args)
{


  return new WebHostBuilder()
	.UseKestrel()
	.UseUrls("http://*:40001/")
	.UseContentRoot(Directory.GetCurrentDirectory())
	.ConfigureAppConfiguration((hostingContext, config) =>
	{
		var env = hostingContext.HostingEnvironment;

		config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

		if (env.IsDevelopment())
		{
			var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
			if (appAssembly != null)
			{
				config.AddUserSecrets(appAssembly, optional: true);
			}
		}

		config.AddEnvironmentVariables();

		if (args != null)
		{
			config.AddCommandLine(args);
		}
	})
	 .ConfigureLogging((hostingContext, logging) =>
	 {
		 logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
		 logging.AddConsole();
		 logging.AddDebug();
	 })
		.UseIISIntegration()
		/*
		.UseDefaultServiceProvider((context, options) =>
		{
			options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
		})

	.UseStartup<Startup>()
	.UseApplicationInsights()
	.Build();
}
*/

}
