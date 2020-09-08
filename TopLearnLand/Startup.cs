using System;
using System.Collections.Generic;
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
            });
            //services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 52428800; });//==> other platform

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
            }
            );

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


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseIIS(options => { options.MaxRequestBodySize = 1048576000; });

            app.UseRouting();
            app.UseStaticFiles();
            app.UseRouting();
            //app.UseMvcWithDefaultRoute();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapAreaControllerRoute(name: "areas",
            //        "Admin",
            //        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            //
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //
            //    endpoints.MapRazorPages();
            //});

        }

    }
}
