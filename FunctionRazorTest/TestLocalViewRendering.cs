using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RazorViewLibrary;

namespace FunctionRazorTest
{
    public static class TestLocalViewRendering
    {
        [FunctionName("TestLocalViewRendering")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Rendering local view from project folder.");

	        //this is required because the IHostingEnvironment needs to be initialized with this assembly name 
	        RazorViewRenderer.Initialize(Assembly.GetExecutingAssembly());

			//This will fail, the view will not compile correctly on functions
			var view = RazorViewRenderer.RenderView("Views/TestView.cshtml");

			return new OkObjectResult(view);
        }
    }
}
