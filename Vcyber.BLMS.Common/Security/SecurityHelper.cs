using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    public static class VcyberSecurityHelper
    {
        public static string ComputeHashString(string convertString)
        {
            var sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            string hashString = BitConverter.ToString(
                sha256.ComputeHash(System.Text.UTF8Encoding.Default.GetBytes(convertString)));
            hashString = hashString.Replace("-", "");
            return hashString;
        }
    }
}
