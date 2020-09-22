using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace WebApiService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region Add Cors Api

            #region JWT WebTokens Authentication

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                /*config.Schemes = new List<AuthenticationSchemeBuilder>();*/
                config.RequireAuthenticatedSignIn = true;
                /*config.SchemeMap*/
            }).AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    #region Validation options

                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateActor = false,
                    ValidateIssuerSigningKey = true,
                    ValidateTokenReplay = false,
                    ValidAudience = "valid client for connection our server",
                    ValidIssuer = "valid other server  {https://fikarender-game.com},{https://fikarender-land.com}",

                    #endregion

                    #region setup options(Issuer & Audience)

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("custom My security Key")),
                    /*IssuerSigningKeyValidator = new IssuerSigningKeyValidator().Target.ToString() //TODO Research for this section
                    IssuerSigningKeyResolver = new IssuerSigningKeyResolver() //TODO Research for this section
                    IssuerValidator = new IssuerValidator() //TODO Research for this section*/

                    #endregion

                    //TODO Search other tokenValidationParameter Options
                };
                options.Audience = "audience name";
                options.Authority = "";
                options.BackchannelHttpHandler.Dispose();
                options.Challenge = "";
                /*options.SecurityTokenValidators.IsReadOnly;
                options.SecurityTokenValidators[index: 1];*/
                options.SaveToken = true;
                /*options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        context.SecurityToken.SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sdad"));
                    },
                    OnAuthenticationFailed = context => { context.},
                    OnChallenge,
                    OnForbidden,
                    OnMessageReceived,
                };*/
                options.RefreshOnIssuerKeyNotFound = false;
                options.RequireHttpsMetadata = true;
                options.BackchannelTimeout = TimeSpan.FromDays(5);
                options.IncludeErrorDetails = false;
                //TODO Search other
            });

            #endregion

            //TODO access other application for use our webapp
            services.AddCors(corsOptions =>
            {
                corsOptions.DefaultPolicyName = "";//TODO PolicyName for use api
                corsOptions.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();//TODO ==> tu header https check mishavad
                    policy.AllowAnyMethod();//TODO ==> all method http verb 
                    policy.AllowAnyOrigin();//TODO ==> access other to our api with Authentication
                    policy.AllowCredentials();//TODO ==> gobahi ssl
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
            });

            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("name", builder =>
               {
                   builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();//others
                });
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "TestArea",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    /*defaults: new { controller = "insta", action = "index" }*/);
            });

            /*app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });*/
        }
    }
}
