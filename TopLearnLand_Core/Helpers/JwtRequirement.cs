//using Microsoft.AspNetCore.Authorization;
//using System;
//using System.Net.Http;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using TopLearnLand_Core.Services.InterFaces;

//namespace TopLearnLand_Core.Helpers
//{
//    public class JwtRequirement : IAuthorizationRequirement
//    {
//        public string ClaimType { get; set; }
//        public string Requirement { get; set; }

//        public JwtRequirement()
//        {
            
//        }
//    }

//    public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
//    {
//        private readonly HttpClient _client;
//        private readonly HttpContext _httpContext;
//        private readonly IServiceProvider _serviceProvider;
//        public JwtRequirementHandler(
//            IHttpContextAccessor httpContextAccessor, 
//            HttpClientFactory httpClientFactory, 
//            IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//            _httpContext = httpContextAccessor.HttpContext;
//            _client = httpClientFactory.CreateClient();
//        }

//        protected override async Task HandleRequirementAsync(
//            AuthorizationHandlerContext context,
//            JwtRequirement requirement)
//        {
//            if (_httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
//            {
//                //TODO Get Service From ServiceProvider & RequestServices(GetService)
//                //var _userService = (IUserService)_serviceProvider.GetService(typeof(IUserService));
//                //var _userService = (IUserService)_httpContext.RequestServices.GetService(typeof(IUserService));

//                var accessToken = authHeader.ToString().Split(" ")[1];
//                string requestUri = $"https://api.fikaland.com/oauth/validateAccessToken?access_token={accessToken}";
//                var response = await _client.GetAsync(requestUri: requestUri,CancellationToken.None);
//                if (response.StatusCode == HttpStatusCode.OK)
//                {
//                    context.Succeed(requirement);
//                }

//            }
//        }
//    }
//}
