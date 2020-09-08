using System;
using System.Collections.Generic;
using System.Text;

namespace TopLearnLand_Core.Generator
{
    public class NameGenerator
    {
        public static string GenericUniqCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
