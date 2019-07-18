using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DiscountStore.Server
{
    /// <summary>
    /// Wraps the execution of the server application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point of the server application.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates an instance of <see cref="IWebHostBuilder"/>.
        /// </summary>
        /// <param name="args">An array of <see cref="string"/> with options.</param>
        /// <returns>An instance of <see cref="IWebHostBuilder"/>.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:5000");
    }
}
