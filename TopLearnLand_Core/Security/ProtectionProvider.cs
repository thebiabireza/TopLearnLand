﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.DataProtection;

namespace TopLearnLand_Core.Security
{
    public class ProtectionProvider : IProtectionProvider
    {
        private readonly IDataProtector _dataProtector;

        public ProtectionProvider(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector(typeof(ProtectionProvider).FullName);
        }

        /// <summary>
        /// رمزگشایی یک رشته
        /// </summary>
        /// <param name="inputText">رشته ورودی برای رمزگشایی</param>
        /// <returns>یک رشته رمزگشایی شده برمیگرداند</returns>
        public string Decrypt(string inputText)
        {
            var inputBytes = Convert.FromBase64String(inputText);
            var bytes = _dataProtector.Unprotect(inputBytes);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// رمزگذاری یک رشته 
        /// </summary>
        /// <param name="inputText">رشته ورودی برای رمزگذاری</param>
        /// <returns>یک رشته رمزگذاری شده برمیگرداند</returns>
        public string Encrypt(string inputText)
        {
            var inputBytes = Encoding.UTF8.GetBytes(inputText);
            var bytes = _dataProtector.Protect(inputBytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
