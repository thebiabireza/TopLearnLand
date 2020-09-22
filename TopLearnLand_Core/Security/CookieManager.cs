using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TopLearnLand_Core.Security
{
    public class CookieManager : ICookieManager
    {
        private readonly IProtectionProvider _protectionProvider;

        public CookieManager(IProtectionProvider protectionProvider)
        {
            _protectionProvider = protectionProvider;
        }

        public void Add(HttpContext context, string token, string value, DateTimeOffset expire, bool isEssential)
        {
            value = _protectionProvider.Encrypt(value);
            context.Response.Cookies.Append(token, value, getCookieOptions(context, expire, isEssential));
        }

        public bool Contains(HttpContext context, string token)
        {
            return context.Request.Cookies.ContainsKey(token);
        }

        public string GetValue(HttpContext context, string token)
        {
            string cookieValue;
            if (!context.Request.Cookies.TryGetValue(token, out cookieValue))
            {
                return null;
            }
            return _protectionProvider.Decrypt(cookieValue);
        }

        public void Remove(HttpContext context, string token)
        {
            if (context.Request.Cookies.ContainsKey(token))
            {
                context.Response.Cookies.Delete(token);
            }
        }

        /// <summary>
        /// Expires at the end of the browser's session.
        /// </summary>
        private CookieOptions getCookieOptions(HttpContext context, DateTimeOffset expire, bool isEssential)
        {
            return new CookieOptions
            {
                IsEssential = true,
                Expires = expire,
                HttpOnly = true,
                Path = context.Request.PathBase.HasValue ? context.Request.PathBase.ToString() : "/",
                Secure = context.Request.IsHttps
            };
        }
    }
}
