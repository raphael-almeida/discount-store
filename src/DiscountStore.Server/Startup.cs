using System;
using System.IO;
using System.Reflection;
using Akka.Actor;
using DiscountStore.Server.Domain.Basket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace DiscountStore.Server
{
    /// <summary>
    /// Sets up the startup of the ASP.NET MVC Core application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Creates a new instance of <see cref="Startup"/>.
        /// </summary>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/>.</param>
        public Startup(IConfiguration configuration) => Configuration = configuration;

        /// <summary>
        /// Gets an instance of <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Adds services to the DI composition container.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime.
        /// </remarks>
        /// <param name="services">An instance of <see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services) => services
            .AddSingleton(_ => ActorSystem.Create("basket"))
            .AddSingleton<BasketManagerActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                var booksManagerActor = actorSystem.ActorOf(Props.Create(() => new BasketManagerActor()));
                return () => booksManagerActor;
            })
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Accounts API" });
                c.DescribeAllEnumsAsStrings();
                c.ExampleFilters();
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.XML"));
            })
            .AddSwaggerExamples()
            .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);




        /// <summary>
        /// Configures the HTTP request pipeline for the application.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime.
        /// </remarks>
        /// <param name="app">An instance of <see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">An instance of <see cref="IHostingEnvironment"/>.</param>
        /// <param name="lifetime">An instance of <see cref="IApplicationLifetime"/>.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Accounts API"));
            app.UseMvc();
            lifetime.ApplicationStarted.Register(() => app.ApplicationServices.GetService<ActorSystem>());
            lifetime.ApplicationStopping.Register(() => app.ApplicationServices.GetService<ActorSystem>().Terminate().Wait());
        }
    }
}
