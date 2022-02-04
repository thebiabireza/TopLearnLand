using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using TopLearnLand_Core.Convertors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using TopLearnLand.Controllers;
using TopLearnLand_Core.Helpers;
using TopLearnLand_Core.Security;
using WebMarkupMin.AspNet.Common;
using WebMarkupMin.AspNetCore3;
using ICookieManager = Microsoft.AspNetCore.Authentication.Cookies.ICookieManager;

namespace TopLearnLand
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            /*var myMaxModelBindingCollectionSize = Convert.ToInt32(
                Configuration["MyMaxModelBindingCollectionSize"] ?? "100");

            services.Configure<MvcOptions>(options =>
                   options.MaxModelBindingCollectionSize = myMaxModelBindingCollectionSize);*/

            /*services.AddSession();
            services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));

            services.AddSingleton<ILinkTools, LinkTools>();
            services.AddRecaptcha(Configuration.GetSection("RecaptchaSettings"));*/

            /*services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
                *//*options.AllowSynchronousIO = true;
                options.AuthenticationDisplayName = "Auth_displayeName";
                options.AutomaticAuthentication = false;*//*
            });
            services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 52428800; });//==> other platform*/

            #region Identity System

            /*services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                //Signin
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = true;

                //TODO Search for Stores
                options.Stores.MaxLengthForKeys = Int32.MaxValue;
                options.Stores.ProtectPersonalData = false;

                //TODO Search for Tokens
                options.Tokens.AuthenticatorIssuer = "";
                options.Tokens.AuthenticatorTokenProvider = "";
                options.Tokens.ChangeEmailTokenProvider = "";
                options.Tokens.ChangePhoneNumberTokenProvider = "";
                options.Tokens.EmailConfirmationTokenProvider = "";
                options.Tokens.PasswordResetTokenProvider = "";

                //TODO Search for ClaimsIdentity
                options.ClaimsIdentity.RoleClaimType = "";
                options.ClaimsIdentity.SecurityStampClaimType = ToString();
                options.ClaimsIdentity.UserIdClaimType = "";
                options.ClaimsIdentity.UserNameClaimType = "";
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<PersianIdentityErrorDescriber>();*/

            #region Set Cookie options

            services.ConfigureApplicationCookie(options =>
            {
                #region other

                /*options.AccessDeniedPath = "/Account/AccessDenied";
                //options.Cookie.Name = "IdentityProj";
                options.LoginPath = "/Account/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;*/

                #endregion
                // Cookie settings
                options.Cookie.Domain = "";
                options.Cookie.Expiration = TimeSpan.FromDays(44);
                options.Cookie.IsEssential = true;
                options.Cookie.MaxAge = TimeSpan.Zero;
                options.Cookie.Name = "";
                options.Cookie.Path = "";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.HttpOnly = true;
                //options.CookieManager = new CookieManager();//TODO khodam bayad config konam (search google)
                options.DataProtectionProvider = new EphemeralDataProtectionProvider();//TODO Set this dataProtection Service
                //TODO Set this SessionStore Service
                /*options.SessionStore*/
                /*options.TicketDataFormat= new SecureDataFormat<AuthenticationTicket>(); => //TODO Set this TicketDataFormat Service
                options.TicketDataFormat= new TicketDataFormat();*///TODO Set this TicketDataFormat Service
                /*options.ClaimsIssuer = new List<Claim>();*///TODO Set this ClaimsIssuer Service
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.AccessDeniedPath = "/AccessDenied";
                /*--Set CookieAuthenticationEvents--*/
                //options.Events = new CookieAuthenticationEvents()
                //{
                //    OnValidatePrincipal = LastChangedValidator.ValidateAsync,

                //};
            });

            #endregion

            services.Configure<SecurityStampValidatorOptions>(option =>
            {
                // option.ValidationInterval = TimeSpan.FromSeconds(15);
            });

            #endregion

            #region Add HttpClient For Call Api Server

            /*services.AddHttpClient("Client_Name", client =>
            {
                client.BaseAddress = new Uri("api server url");
                client.MaxResponseContentBufferSize = Int64.MaxValue;
                client.Timeout = TimeSpan.FromDays(4);
                client.DefaultRequestVersion = Version.Parse("s");
            });*/

            #endregion

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

            #region HttpContextAccessor

            services.AddHttpContextAccessor();

            #endregion

            #region IoC

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IViewRenderService, RenderViewToString>();
            services.AddTransient<IImageConvertor, ImageConvertor>();

            /*services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IProtectionProvider, ProtectionProvider>();*/

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
                options.LoginPath = $"/Login";
                options.LogoutPath = $"/Logout";
                options.AccessDeniedPath = $"/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
                options.SlidingExpiration = true;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToAccessDenied = (context) =>
                    {
                        context.RedirectUri = new PathString($"/AccessDenied");
                        //return context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    },
                    OnRedirectToLogin = context =>
                    {
                        context.RedirectUri = new PathString($"/Login");
                        return Task.CompletedTask;
                    },
                };
            }).AddOAuth(CookieAuthenticationDefaults.AuthenticationScheme, configure =>
             {
                 configure.Events = new OAuthEvents()
                 {
                     OnCreatingTicket = (context) =>
                     {
                         var accessToken = context.AccessToken;//TODO my access Token
                         var jwtPayload = accessToken.Split(".")[1];//TODO Payload of JWt
                         var jwtBytes = Convert.FromBase64String(jwtPayload);
                         var jsonpayload = Encoding.UTF8.GetString(jwtBytes);
                         var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonpayload);

                         foreach (var claim in claims)
                         {
                             context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
                         }

                         return Task.CompletedTask;
                     }
                 };
                 configure.AuthorizationEndpoint = $"set or get==>https://fikaland.com/auth";
                 configure.TokenEndpoint = $"set or get==>https://fikaland.com/auth/token";
                 configure.ClaimActions.ToList();
                 configure.ClientId = "myClient-Id";
                 //configure.ClientSecret.Where(s => s.CompareTo(s)).ToList();
                 //configure.Scope.Where(s => s.Trim(new char() { s }));
                 //configure.StateDataFormat.Protect();
                 configure.UsePkce = true;
                 configure.UserInformationEndpoint = "userInfo Endpoint";
                 configure.AccessDeniedPath = $"{typeof(AccountController)}/{"AccessDenied"}";
                 /*configure.BackchannelHttpHandler = new HttpClientHandler()
                 {
                     AllowAutoRedirect = true,
                     AutomaticDecompression = ,
                     CheckCertificateRevocationList = true,
                     ClientCertificateOptions = ,
                     UseCookies = true,
                 };*/
                 configure.BackchannelTimeout = TimeSpan.FromHours(8);
                 configure.CallbackPath = "callback Path";
                 configure.ClaimsIssuer = "";
                 configure.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
             });

            #region Configure Identity Cookie

            /*services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddIdentityCookies(o =>
            {
                o.ApplicationCookie = new OptionsBuilder<CookieAuthenticationOptions>(services, "name").Configure(c =>
                {
                    c.Cookie = new CookieBuilder()
                    {
                        Domain = "domain",
                        Expiration = TimeSpan.FromDays(2),
                        HttpOnly = true,
                        IsEssential = true,
                        MaxAge = TimeSpan.FromDays(5),
                        Name = "cookie-name",
                        Path = new PathString("path"),
                        SameSite = SameSiteMode.Strict,
                        SecurePolicy = CookieSecurePolicy.Always
                    };
                    c.SlidingExpiration = true;
                    c.AccessDeniedPath = new PathString($"/account/accessDenied");
                    c.LoginPath = new PathString($"/account/login");
                    c.LogoutPath = new PathString($"/account/logout");
                    c.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                    c.ExpireTimeSpan = TimeSpan.FromDays(5);
                    c.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToAccessDenied = (context) =>
                        {
                            context.RedirectUri = new PathString($"/account/accessDenied");
                            return Task.CompletedTask;
                        },
                        OnRedirectToLogin = context =>
                        {
                            context.RedirectUri = new PathString($"/account/login");
                            return Task.CompletedTask;
                        },
                    };
                    //c.TicketDataFormat = new TicketDataFormat();
                    //c.DataProtectionProvider = new EphemeralDataProtectionProvider();
                    //c.SessionStore;
                    //c.CookieManager = new SecureCookiesProvider(new ProtectionProvider());
                });
                o.ExternalCookie.Configure(configureOptions =>
                {
                    configureOptions.Cookie = new CookieBuilder()
                    {
                        Domain = "domain",
                        Expiration = TimeSpan.FromDays(2),
                        HttpOnly = true,
                        IsEssential = true,
                        MaxAge = TimeSpan.FromDays(5),
                        Name = "cookie-name",
                        Path = new PathString("path"),
                        SameSite = SameSiteMode.Strict,
                        SecurePolicy = CookieSecurePolicy.Always
                    };
                    configureOptions.SlidingExpiration = true;
                    configureOptions.AccessDeniedPath = new PathString($"/account/accessDenied");
                    configureOptions.LoginPath = new PathString($"/account/login");
                    configureOptions.LogoutPath = new PathString($"/account/logout");
                    configureOptions.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                    configureOptions.ExpireTimeSpan = TimeSpan.FromDays(5);
                    configureOptions.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToAccessDenied = (context) =>
                        {
                            context.RedirectUri = new PathString($"/account/accessDenied");
                            return Task.CompletedTask;
                        },
                        OnRedirectToLogin = context =>
                        {
                            context.RedirectUri = new PathString($"/account/login");
                            return Task.CompletedTask;
                        },
                    };
                });
                //o.TwoFactorRememberMeCookie;
                //o.TwoFactorUserIdCookie;
            });*/

            #endregion

            #region Config Coolie Authentication (event,...)

            services.ConfigureApplicationCookie(configure =>
            {
                configure.Events = new CookieAuthenticationEvents()
                {
                    //TODO Config Events
                };
            });

            #endregion

            #region Cookie policy Options

            /*services.Configure<CookiePolicyOptions>(option =>
            {
                option.CheckConsentNeeded = context => true;
                option.ConsentCookie.SameSite = SameSiteMode.Lax;
                option.HttpOnly = HttpOnlyPolicy.Always;
                option.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                option.OnAppendCookie = context => context.Context.Response.Cookies.Append("key", "value");
                var cookieManager = new CookieManager(_protectionProvider);
                option.OnAppendCookie = context => cookieManager.Add(context.Context, "token", "value", DateTimeOffset.UtcNow, true);
                option.OnDeleteCookie = context => context.Context.Response.Cookies.Delete("key", new CookieOptions()
                {
                    Domain = "my domain==> fikarender.com",
                    Expires = DateTimeOffset.UtcNow,
                    HttpOnly = true,
                    IsEssential = true,
                    MaxAge = TimeSpan.FromDays(30),
                    Path = "cookie path",
                    SameSite = SameSiteMode.Strict,
                    Secure = true
                });
                option.Secure = CookieSecurePolicy.SameAsRequest;
            });*/

            #endregion

            #endregion

            #region Protections

            services.AddDataProtection()
                .DisableAutomaticKeyGeneration()
                .SetDefaultKeyLifetime(new TimeSpan(14, 0, 0, 0));

            #endregion

            #region Authorization

            services.AddAuthorization(options =>
            {
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder.AddRequirements(new JwtRequirement()).Build();
                //options.DefaultPolicy = defaultAuthPolicy;

                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
            });
            //services.AddScoped<IAuthorizationHandler, JwtRequirementHandler>();
            #endregion

            #region Access DirectoryBrowser

            services.AddDirectoryBrowser();

            #endregion

            //TODO Api Documentation Generator
            #region Api Documrnt Sections 

            //TODO JWT 
            #region JWT WebTokens Authentication

            #region Full Example

            /*services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                config.Schemes = new List<AuthenticationSchemeBuilder>();
                config.RequireAuthenticatedSignIn = true;
                config.SchemeMap
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
                    IssuerSigningKeyValidator = new IssuerSigningKeyValidator().Target.ToString() //TODO Research for this section
                    IssuerSigningKeyResolver = new IssuerSigningKeyResolver() //TODO Research for this section
                    IssuerValidator = new IssuerValidator() //TODO Research for this section

                    #endregion

                    //TODO Search other tokenValidationParameter Options
                };
                options.Audience = "audience name";
                options.Authority = "";
                options.BackchannelHttpHandler.Dispose();
                options.Challenge = "";
                options.SecurityTokenValidators.IsReadOnly;
                options.SecurityTokenValidators[index: 1];
                options.SaveToken = true;
                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        context.SecurityToken.SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sdad"));
                    },
                    OnAuthenticationFailed = context => { context.},
                    OnChallenge,
                    OnForbidden,
                    OnMessageReceived,
                };
                options.RefreshOnIssuerKeyNotFound = false;
                options.RequireHttpsMetadata = true;
                options.BackchannelTimeout = TimeSpan.FromDays(5);
                options.IncludeErrorDetails = false;
                //TODO Search other
            });*/

            #endregion

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                //config.Schemes = new List<AuthenticationSchemeBuilder>();
                config.RequireAuthenticatedSignIn = true;
                //config.SchemeMap.Add("ass");
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
                    ValidAudience = "valid client for connection our server{https://amir.ir}",
                    ValidIssuer = "valid other server  {https://fikarender-game.com},{https://fikarender-land.com}",

                    #endregion

                    #region setup options(Issuer & Audience)

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("custom My security Key")),
                    //IssuerSigningKeyValidator = new IssuerSigningKeyValidator().Target.ToString() //TODO Research for this section
                    //IssuerSigningKeyResolver = new IssuerSigningKeyResolver() //TODO Research for this section
                    //IssuerValidator = new IssuerValidator() //TODO Research for this section

                    #endregion

                    //TODO Search other tokenValidationParameter Options
                };
                options.Audience = "audience name";
                options.Authority = "";
                options.BackchannelHttpHandler.Dispose();
                options.Challenge = "get or set the challenge to put----- header['WWW-Authenticate']";
                options.SecurityTokenValidators.IsReadOnly.Equals(true);
                options.SecurityTokenValidators[1].MaximumTokenSizeInBytes = Int32.MaxValue;
                var securityToken = new JwtSecurityToken(JwtHeader.Base64UrlDeserialize("Sdsdsd"), JwtPayload.Deserialize("s"), rawHeader: "", rawPayload: "", rawSignature: "");
                //options.SecurityTokenValidators.Where(s => s.ValidateToken(securityToken: securityToken.ToString(),
                //    validationParameters: new TokenValidationParameters(), out securityToken));
                options.SaveToken = true;
                options.Events = new JwtBearerEvents()
                {
                    /*OnTokenValidated = context =>
                    {
                        context.SecurityToken.SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sdad"));
                    },
                    OnAuthenticationFailed = context => { },
                    OnChallenge = (context) =>
                    {
                        if (context.Request.Query.ContainsKey("access_token"))
                        {
                            context.Token = context.Request.Query["access_token"];
                            return Task.CompletedTask;
                        }
                    },
                    OnForbidden = (context) =>
                    {
                        if (context.Request.Query.ContainsKey("access_token"))
                        {
                            context.Token = context.Request.Query["access_token"];
                            return Task.CompletedTask;
                        }
                    },
                    OnMessageReceived = (context) =>
                    {
                        if (context.Request.Query.ContainsKey("access_token"))
                        {
                            context.Token = context.Request.Query["access_token"];
                            return Task.CompletedTask;
                        }
                    },*/
                };
                options.RefreshOnIssuerKeyNotFound = false;
                options.RequireHttpsMetadata = true;
                options.BackchannelTimeout = TimeSpan.FromDays(5);
                options.IncludeErrorDetails = false;
                //TODO Search other
            })
            #region Open Authentication(OAuth)

            .AddOAuth(CookieAuthenticationDefaults.AuthenticationScheme, configure =>
             {
                 configure.Events = new OAuthEvents()
                 {
                     OnCreatingTicket = (context) =>
                     {
                         var accessToken = context.AccessToken;//TODO my access Token
                         var jwtPayload = accessToken.Split(".")[1];//TODO Payload of JWt
                         var jwtBytes = Convert.FromBase64String(jwtPayload);
                         var jsonpayload = Encoding.UTF8.GetString(jwtBytes);
                         var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonpayload);

                         foreach (var claim in claims)
                         {
                             context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
                         }

                         return Task.CompletedTask;
                     }
                 };
                 configure.AuthorizationEndpoint = $"set or get==>https://fikaland.com/auth";
                 configure.TokenEndpoint = $"set or get==>https://fikaland.com/auth/token";
                 configure.ClaimActions.ToList();
                 configure.ClientId = "myClient-Id";
                 //configure.ClientSecret.Where(s => s.CompareTo(s)).ToList();
                 //configure.Scope.Where(s => s.Trim(new char() { s }));
                 //configure.StateDataFormat.Protect();
                 configure.UsePkce = true;
                 configure.UserInformationEndpoint = "userInfo Endpoint";
                 configure.AccessDeniedPath = $"{typeof(AccountController)}/{"AccessDenied"}";
                 //configure.BackchannelHttpHandler = new HttpClientHandler()
                 //{
                 //    AllowAutoRedirect = true,
                 //    AutomaticDecompression = ,
                 //    CheckCertificateRevocationList = true,
                 //    ClientCertificateOptions = ,
                 //    UseCookies = true,
                 //};
                 //configure.BackchannelTimeout = TimeSpan.FromHours(8);
                 configure.CallbackPath = "callback Path";
                 configure.ClaimsIssuer = "";
                 configure.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
             });

            #endregion

            #endregion

            //TODO Cors Api
            #region Add Cors Api

            //TODO access other application for use our webapp
            /*services.AddCors(corsOptions =>
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
            });*/

            /*services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("name" , builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();//others
                });
            });*/

            #endregion

            //TODO Swagger
            #region Add Api Document Generator(Swagger)

            /*services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("Firs Api Document", new OpenApiInfo()
                {
                    Title = "FikaRender Api Service",
                    Description = "my description",
                    Version = "1.0.0",
                    *//*Contact = ,
                    Extensions = ,
                    License = ,
                    TermsOfService =*//*
                });
                swagger.IncludeXmlComments(Path.Combine(Directory.GetCurrentDirectory(), @"TopLearnLand\TopLearnLand", "TopLearnLand.xml"));
                *//*swagger.DocumentFilterDescriptors = new List<FilterDescriptor>() { };
                swagger.OperationFilterDescriptors = new List<FilterDescriptor>();
                swagger.ParameterFilterDescriptors = new List<FilterDescriptor>();
                swagger.RequestBodyFilterDescriptors = new List<FilterDescriptor>();
                swagger.SchemaFilterDescriptors = new List<FilterDescriptor>();
                swagger.SchemaGeneratorOptions = new SchemaGeneratorOptions();
                swagger.AddSecurityDefinition("security", new OpenApiSecurityScheme());
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement());
                swagger.AddServer(new OpenApiServer());
                swagger.CustomOperationIds(s =>
                {
                    s.HttpMethod = "post";
                    *//*s.Properties;
                    s.ActionDescriptor = new ActionDescriptor();
                    s.GroupName = "";
                    s.HttpMethod = "post";
                    s.ParameterDescriptions = new List<ApiParameterDescription>();
                    s.RelativePath = "";
                    s.SupportedRequestFormats = new List<ApiRequestFormat>();
                    s.SupportedResponseTypes = new List<ApiResponseType>();*//*
                });
                swagger.CustomSchemaIds(s=>{});
                swagger.DescribeAllParametersInCamelCase();*//*
                //TODO Other Options
            });*/

            #endregion

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

            #region ResponseCache Section

            /*services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = Int64.MaxValue;
                options.SizeLimit = Int64.MinValue;
                options.UseCaseSensitivePaths = true;
            });*/

            #endregion

            #region Add MemoryCache

            /*services.AddMemoryCache(options =>
            {
                *//*options.CompactionPercentage = Double.Epsilon;
                options.ExpirationScanFrequency = TimeSpan.FromDays(1);
                options.SizeLimit = Int64.MaxValue;*/
            /*options.Clock = new SystemClock();*//*
        });

        services.AddDistributedMemoryCache(options =>
        {
            *//*options.CompactionPercentage = Double.Epsilon;
            options.ExpirationScanFrequency = TimeSpan.FromDays(1);
            options.SizeLimit = Int64.MaxValue;*/
            /*options.Clock = new SystemClock();*//*
        });*/

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

            /*app.UseFileServer(new FileServerOptions()
            {
                EnableDefaultFiles = true,
                EnableDirectoryBrowsing = true,
                FileProvider = new PhysicalFileProvider("file-provider")
                {
                    UseActivePolling = true,
                    UsePollingFileWatcher = false
                },
                RequestPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\default")
            });
            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("customIndex.html");
            app.UseDefaultFiles(options);*/

            #endregion

            #region Swagger

            /*app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/1.0.0/swagger.json", "My First Swagger");
            });*/

            #endregion

            app.UseResponseCaching();
            app.UseWebMarkupMin();

            app.UseStatusCodePages();
            var rewriteOptions = new RewriteOptions().AddRedirectToHttps();
            app.UseRewriter(rewriteOptions);
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            //app.UseMvcWithDefaultRoute();

            #region CookiePolicy Options & Useable 

            /*app.UseCookiePolicy(new CookiePolicyOptions()
            {
                CheckConsentNeeded = context => true,
                ConsentCookie = new CookieBuilder()
                {
                    Domain = "domain",
                    Expiration = TimeSpan.FromDays(4),
                    HttpOnly = true,
                    IsEssential = true,
                    MaxAge = TimeSpan.FromDays(4),
                    Name = "Cookie Name",
                    Path = "Cookie Path",
                    SameSite = SameSiteMode.Lax,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                }
            });*/

            #endregion

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
                policy.WithOrigins(new string[]{ "PolicyName" , "PolicyName2..." });//TODO more options similar top add cors services
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
