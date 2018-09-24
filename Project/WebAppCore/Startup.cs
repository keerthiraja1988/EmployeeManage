using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CrossCutting.Caching;
using CrossCutting.Logging;
using DomainModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using StackExchange.Profiling.Storage;
using WebAppCore.Infrastructure;

using static DependencyInjecionResolver.DependencyInjecionResolver;

namespace WebAppCore
{


    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            List<ApplicationConfigModel> applicationConfigs = new List<ApplicationConfigModel>();

            applicationConfigs = Configuration.GetChildren().Select(x => new ApplicationConfigModel
            {
                Key = x.Key
                                                                            ,
                Value = x.Value
            }).ToList();
            Caching.Instance.AddApplicationConfigs(applicationConfigs);

        }

        public IConfiguration Configuration { get; set; }
        public Autofac.IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
           
            // Maintain property names during serialization. See:
            // https://github.com/aspnet/Announcements/issues/194
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                    ;
            ConfigureRazorAndCompression(services);

            // Add Kendo UI services to the services container
            services.AddKendo();

            services.AddScoped<NlogTraceAttribute>();

            services.AddAutoMapper();

            var sqlConnection = Configuration.GetValue<string>("DBConnection");
            
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
                   .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                   {
                       options.AccessDeniedPath = new PathString("/Security/Access");
                       options.LoginPath = new PathString("/UserAccount");
                       options.ExpireTimeSpan = TimeSpan.FromSeconds(600);
                       options.SlidingExpiration = true;
                   });

            ConfigureMiniProfier(services);

            ConfigureWebOptimer(services);

         
          


            var builder = new Autofac.ContainerBuilder();

            builder.RegisterAssemblyModules(System.Reflection.Assembly.GetExecutingAssembly());
            builder.Populate(services);
            builder.RegisterModule(new ServiceDIContainer(sqlConnection));
            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // ...existing configuration...
            app.UseMiniProfiler();

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
           // app.UseWebOptimizer();
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();


            app.UseAuthentication();

            // app.UseWebMarkupMin();
            //loggerFactory.AddLog4Net(); // << Add this line

           

            app.UseMvc(routes =>
            {
                routes.MapRoute(
             name: "area",
             template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void ConfigureRazorAndCompression(IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(o =>
            {

                // {2} is area, {1} is controller,{0} is the action    
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Views/Shared/{0}" + RazorViewEngine.ViewExtension);


                // Untested. You could remove this if you don't care about areas.
                o.AreaViewLocationFormats.Clear();
                o.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/Areas/Shared/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddResponseCompression(options =>
            {

                options.Providers.Add<Infrastructure.BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;

            }
          );

        }

        private static void ConfigureMiniProfier(IServiceCollection services)
        {
            // Note .AddMiniProfiler() returns a IMiniProfilerBuilder for easy intellisense
            services.AddMiniProfiler(options =>
            {
                // All of this is optional. You can simply call .AddMiniProfiler() for all defaults

                // (Optional) Path to use for profiler URLs, default is /mini-profiler-resources
                options.RouteBasePath = "/profiler";

                // (Optional) Control storage
                // (default is 30 minutes in MemoryCacheStorage)
                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

                // (Optional) Control which SQL formatter to use, InlineFormatter is the default
                //options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

                // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
                // (defaults to true, and connection opening/closing is tracked)
                options.TrackConnectionOpenClose = true;
            });
        }

        private static void ConfigureWebOptimer(IServiceCollection services)
        {
            services.AddWebOptimizer(pipeline =>
            {
                // Creates a CSS and a JS bundle. Globbing patterns supported.
                pipeline.AddCssBundle("/css/bundle.css", "css/site.css"
                                     , "lib/bootstrap/dist/css/bootstrap.min.css"
                                    );


                pipeline.AddJavaScriptBundle("/js/bundle.js", "js/Main.js"
                                        , "js/site.js"

                                        );


                // This will minify any JS and CSS file that isn't part of any bundle
                pipeline.MinifyCssFiles();
                pipeline.MinifyJsFiles();

            });
        }
    }
}
