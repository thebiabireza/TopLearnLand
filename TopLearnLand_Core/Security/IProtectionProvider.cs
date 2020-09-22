using System;
using System.Collections.Generic;
using System.Text;

namespace TopLearnLand_Core.Security
{
    public interface IProtectionProvider
    {
        string Decrypt(string inputText);
        string Encrypt(string inputText);
    }
}
