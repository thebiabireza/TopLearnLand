using System;
using System.Collections.Generic;
using System.Text;

namespace TopLearnLand_Core.Convertors
{
     public class FixedTexts
    {
        public static string FixedEmail(string email)
        {
            return email.Trim().ToLower();
        }
    }
}
