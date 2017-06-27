using System;
using System.Text;
using System.Security.Cryptography;
using PS_project.Models;

namespace PS_project.Utils
{
    public class Hashing
    {
        public static string GetSalt()
        {
            int minSaltSize = 4;
            int maxSaltSize = 8;

            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            byte[] saltBytes = new byte[saltSize];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            rng.GetNonZeroBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public static string GetHashed(string text, string salt)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(text+salt);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool ComparePasswords(string password, UserModel user)
        {
            string hash_pass = GetHashed(password, user.salt);
            string hash = user.hashedpassword.Replace(" ", "");
            return hash_pass.Equals(hash);
        }
    }
}