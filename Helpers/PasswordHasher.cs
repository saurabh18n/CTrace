using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CTrace.Helpers
{
    public static class PasswordHasher
    {
        public const int SALT_LENGTH = 128 / 8;
        public const int PASS_LENGTH = 256 / 8;
        public const int ITRATION_COUNT = 10000;
        public static string GenrateHash(string pass, out string salt)
        {
            byte[] saltAr = new byte[SALT_LENGTH];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(saltAr);
            }
            salt = Convert.ToBase64String(saltAr);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: pass,
            salt: saltAr,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: ITRATION_COUNT,
            numBytesRequested: PASS_LENGTH));
            return hashed;
        }
        public static bool VerifyHash(string pass,string hash, string salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: pass,
            salt: Convert.FromBase64String(salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: ITRATION_COUNT,
            numBytesRequested: PASS_LENGTH));
            if(hash == hashed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
