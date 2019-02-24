using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.ObjectPool;

namespace RazorViewLibrary
{

	public static class RazorViewRenderer
	{
		private static Assembly CallingAssembly;

		public static void Initialize(Assembly assembly)
		{
			CallingAssembly = assembly;
		}

		public static string RenderView(string view)
		{
			var model = new TestModel() {Name = "Foobar"};

			using (var services = InitializeServices().CreateScope())
			{
				var helper = services.ServiceProvider.GetRequiredService<RazorViewToStringRenderer>();
				return helper.RenderViewToStringAsync(view, model).Result; ;
			}
		}

		private static IServiceScopeFactory InitializeServices()
		{
			var services = new ServiceCollection();
			ConfigureDefaultServices(services);

			//add services here
			
			var serviceProvider = services.BuildServiceProvider();

			return serviceProvider.GetRequiredService<IServiceScopeFactory>();
		}

		private static void ConfigureDefaultServices(IServiceCollection services)
		{
			var fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

			//The RazorViewEngine will cause issues if this doesn't match the .deps file, so we initialize the value first
			if (CallingAssembly == null)
				CallingAssembly = Assembly.GetCallingAssembly();

			services.AddSingleton<IHostingEnvironment>(new HostingEnvironment()
			{
				ApplicationName = CallingAssembly.GetName().Name,
				ContentRootFileProvider = fileProvider,
			});
			
			services.Configure<RazorViewEngineOptions>(options =>
			{
				options.FileProviders.Add(fileProvider);
			});

			var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
			services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
			services.AddSingleton<DiagnosticSource>(diagnosticSource);
			services.AddLogging();
			services.AddMvc();
			services.AddTransient<RazorViewToStringRenderer>();
		}
	}
}
