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
    public static class TestPrecompiledViewRendering
    {
        [FunctionName("TestPrecompiledViewRendering")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
			log.LogInformation("Rendering precompiled view from class library.");

	        //this is required because the IHostingEnvironment needs to be initialized with this assembly name 
	        RazorViewRenderer.Initialize(Assembly.GetExecutingAssembly());

			//This will fail if you haven't views dll manually into the bin folder
			var view = RazorViewRenderer.RenderView("Views/TestView2.cshtml");


	        return new OkObjectResult(view);
		}
    }
}
