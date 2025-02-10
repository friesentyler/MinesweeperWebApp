using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;


namespace MinesweeperWebApp.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public string Groups { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string State { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }    

        public void SetPassword(string password)
        {
            Salt = MakeSalt();
            PasswordHash = Hash(password, Salt);
        }

        public bool VerifyPassword(string password)
        {
            string hashedInput = Hash(password, Salt);
            return hashedInput == PasswordHash;
        }

        // Ref: https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes?view=net-9.0&utm_source=chatgpt.com

        private byte[] MakeSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);
                return salt;
            }
        }

        private string Hash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
