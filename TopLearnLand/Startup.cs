using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TopLearnLand_DataLayer.Context;
using Microsoft.Extensions.Configuration;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_Core.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using TopLearnLand_Core.Convertors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNetCore3;

namespace TopLearnLand
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
                /*options.AllowSynchronousIO = true;
                options.AuthenticationDisplayName = "Auth_displayeName";
                options.AutomaticAuthentication = false;*/
            });
            services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 52428800; });//==> other platform

            #region Configure Custom Feuture

            //services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 52428800; });//==> other platform

            /*services.Configure<FormOptions>(options =>
            {
                options.KeyLengthLimit = Int32.MaxValue;
                options.BufferBody = true;
                options.BufferBodyLengthLimit = Int64.MaxValue;
                options.MemoryBufferThreshold = 2000;
                options.MultipartBodyLengthLimit = Int64.MaxValue;
                options.MultipartBoundaryLengthLimit = Int32.MaxValue;
                options.MultipartHeadersCountLimit = 100;
                options.MultipartHeadersLengthLimit = Int32.MaxValue;
                options.ValueCountLimit = Int32.MaxValue;
                options.ValueLengthLimit = Int32.MaxValue;
            });*/

            /*services.Configure<FormFile>(options =>
            {
                options.ContentType = "myContentType";
                options.Headers["key"] = "value";
                options.ContentDisposition = "";
            });*/

            #endregion

            #region AddMVC

            services.AddMvc(mvcOptions => mvcOptions.EnableEndpointRouting = false);

            #endregion

            #region DataBase Contexts

            services.AddDbContext<TopLearnLandContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TopLearnLandConnection"));
            });


            #endregion

            #region IoC

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IViewRenderService, RenderViewToString>();
            services.AddTransient<IImageConvertor, ImageConvertor>();

            #endregion

            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.AccessDeniedPath = "/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
                options.SlidingExpiration = true;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.Events = new CookieAuthenticationEvents()
                {
                    //TODO Config Events
                };
            });

            #region Config Coolie Authentication

            services.ConfigureApplicationCookie(configure =>
            {
                configure.Events = new CookieAuthenticationEvents()
                {
                    //TODO Config Events
                };
            });

            #endregion

            #endregion

            #region Authorization

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
            });

            #endregion

            #region Access DirectoryBrowser

            services.AddDirectoryBrowser();

            #endregion

            #region Add Cors Api

            /*services.AddCors(corsOptions =>
            {
                corsOptions.DefaultPolicyName = "adminOnly";
                corsOptions.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                    policy.AllowCredentials();
                    policy.Build();
                    policy.DisallowCredentials();
                    policy.SetIsOriginAllowed(s => s.Contains("myOrigins"));
                    policy.SetIsOriginAllowedToAllowWildcardSubdomains();
                    policy.SetPreflightMaxAge(TimeSpan.FromHours(2));
                    policy.WithExposedHeaders(new string[] { "ex_header1", "ex_header2", "...." });
                    policy.WithHeaders(new string[] { "header1", "header2", "...." });
                    policy.WithOrigins(new string[] { "origin1", "origin2", "...." });
                    policy.WithMethods(new string[] { "method1", "method2", "...." });
                });
                corsOptions.AddPolicy("myPolicy", corsPolicy =>
                {
                    corsPolicy.WithHeaders();//.... more
                });
                corsOptions.GetPolicy("policyName");
            });*/

            #endregion

            #region markup Web Html Compression

            services.AddWebMarkupMin(options =>
            {
                options.AllowCompressionInDevelopmentEnvironment = true;
                options.AllowMinificationInDevelopmentEnvironment = true;
                #region more Options

                /*options.IsCompressionEnabled();
                options.IsMinificationEnabled();
                options.IsAllowableResponseSize(Int64.MaxValue);
                options.IsPoweredByHttpHeadersEnabled();*/

                #endregion
            }).AddHtmlMinification().AddHttpCompression()
                .AddXhtmlMinification().AddXmlMinification();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory _loggerFactory, TopLearnLandContext _context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                #region DataBase Configures

                //_context.Database.EnsureCreatedAsync();
                /*_context.Database.AutoTransactionsEnabled = true;
                _context.Database.BeginTransaction();
                _context.Database.CanConnectAsync();
                _context.Database.CommitTransaction();
                _context.Database.CreateExecutionStrategy();
                _context.Database.CloseConnection();*/
                //more options please read after 

                #endregion
            }
            else
            {
                #region Handle ExceptionError

                /*app.UseExceptionHandler(subApp =>
                {
                    var service = subApp.ApplicationServices.GetService(typeof(IUserService));
                    subApp.Run(async context =>
                    {
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync("<p>html supported type content!!!</p>");
                        await context.Response.WriteAsync("hellooo this is supported text type content");
                    });
                });

                app.UseExceptionHandler("error handling path view or page ==> /home/error");*/

                #endregion
            }

            #region Envirements Options

            /*env.WebRootFileProvider.GetDirectoryContents("subPath");
            env.WebRootFileProvider.GetFileInfo("");
            env.WebRootFileProvider.Watch("filter");
            env.ContentRootFileProvider.GetDirectoryContents("subPath");
            env.ContentRootFileProvider.GetFileInfo("");
            env.ContentRootFileProvider.Watch("filter");
            env.WebRootPath = "/topLearn";
            env.ApplicationName = "get or set";
            env.EnvironmentName = "get or set";*/

            #endregion

            #region Create Page for handle status code

            /*app.UseStatusCodePages(subApp =>
            {
                subApp.Run(async context =>
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<p>html supported type content!!! so create page for status code handle</p>");
                });
            });*/

            #endregion

            #region Limit size content in IIS

            //app.UseIIS(options => { options.MaxRequestBodySize = 1048576000; });

            #endregion

            #region Access DirectoryBrowser

            /*app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "path_delkhah")),
                RequestPath = "myPath"
            });*/

            #endregion

            #region Create Default page html (Intro WebPage)

            /*var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("customIndex.html");
            app.UseDefaultFiles(options);*/

            #endregion

            app.UseWebMarkupMin();

            app.UseStatusCodePages();
            var rewriteOptions = new RewriteOptions().AddRedirectToHttps();
            app.UseRewriter(rewriteOptions);
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            //app.UseMvcWithDefaultRoute();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

                /*routes.MapRoute(
                    name: "insta",
                    template:"insta/{accountName}",
                    defaults: new { controllers = "insta", action = "index"}
                );*/
            });

            #region User Cors Api

            /*app.UseCors(policy =>
            {
                policy.WithOrigins();//more options similar top add cors services
            });*/

            #endregion

            #region Customize MiddleWare

            /*app.Use(async (context, next) =>
            {
                await next();
                var coursId = context.Request.Path.Value;
                if (context.User.Identity.IsAuthenticated && _userService.IsUserInCourse(context.User.Identity.Name,1))
                {
                    if (context.Response.StatusCode == 404)
                    {
                        context.Request.Path = "/home/err404";
                        await next();
                    }
                }
            });*/

            #endregion

            #region EndPoints

            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(name: "areas",
                    "Admin",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}",
                    defaults: new {controller = "insta", action = "index"});

                endpoints.MapRazorPages();
            });*/

            #region Full Endpoint Map Options

            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "area",
                    pattern: "{area:exists}/{controller=home}/{action=index}/{id?}",
                    areaName: "api",
                    defaults: new { area = "api", controller = "home", action = "index", id = 1},
                    constraints:"",
                    dataTokens: ""
                );
            });*/

            #endregion

            #endregion

        }

    }
}
