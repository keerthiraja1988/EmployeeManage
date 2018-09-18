using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;
using WebApp.Infrastructure;
using CrossCutting.Logging;
using DomainModel;
using CrossCutting.Caching;
using Autofac;
using WebApp.Controllers;
using Autofac.Extensions.DependencyInjection;
using static DependencyInjecionResolver.DependencyInjecionResolver;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            List<ApplicationConfigModel> applicationConfigs = new List<ApplicationConfigModel>();

            applicationConfigs = Configuration.GetChildren().Select(x =>  new ApplicationConfigModel{
                                                                            Key = x.Key
                                                                            ,Value = x.Value }).ToList();
            Caching.Instance.AddApplicationConfigs(applicationConfigs);
        }

        public IConfiguration Configuration { get; set; }
        public Autofac.IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Maintain property names during serialization. See:
            // https://github.com/aspnet/Announcements/issues/194
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                   
                    ;

            // Add Kendo UI services to the services container
            services.AddKendo();
            services.AddScoped<NlogTraceAttribute>();

            var sqlConnection = Configuration.GetValue<string>("ApplicationsSetting:SQLConnection");

            var builder = new Autofac.ContainerBuilder();

            builder.RegisterAssemblyModules(System.Reflection.Assembly.GetExecutingAssembly());
            builder.Populate(services);
            builder.RegisterModule(new ServiceDIContainer(sqlConnection));
            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables()
                    ;

            Configuration = builder.Build();

           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
           
            //loggerFactory.AddLog4Net(); // << Add this line
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
