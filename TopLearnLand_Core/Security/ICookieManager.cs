using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TopLearnLand_Core.Security
{
    public interface ICookieManager
    {
        void Add(HttpContext context, string token, string value, DateTimeOffset expire, bool isEssential);
        bool Contains(HttpContext context, string token);
        string GetValue(HttpContext context, string token);
        void Remove(HttpContext context, string token);
    }
}
