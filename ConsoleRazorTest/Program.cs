using System;
using System.Reflection;
using RazorViewLibrary;

namespace ConsoleRazorTest
{
	class Program
	{
		static void Main(string[] args)
		{
			//this is required because the IHostingEnvironment needs to be initialized with this assembly name 
			RazorViewRenderer.Initialize(Assembly.GetExecutingAssembly());
			
			//this will compile the test view in this project and display it
			Console.Write(RazorViewRenderer.RenderView("Views/TestView.cshtml"));

			//this will run the precompiled view in the razor library
			Console.Write(RazorViewRenderer.RenderView("Views/TestView2.cshtml"));

			Console.ReadKey();
		}
	}
}
