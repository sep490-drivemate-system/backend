using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Password
{
    public class PasswordHasherService
    {
        private readonly PasswordHasher<object> _passwordHasher = new();

        public Task<string> HashPassword(string password) => Task.FromResult(_passwordHasher.HashPassword(null!, password));


        public Task<bool> VerifyPassword(string hashedPassword, string providedPassword) =>
             Task.FromResult(_passwordHasher.VerifyHashedPassword(null!, hashedPassword, providedPassword) == PasswordVerificationResult.Success);
        public Task<(string PlainPassword, string HashedPassword)> GenerateAndHashPassword(int length = 10)
        {
            var plainPassword = GenerateRandomPassword(length);
            var hashed = _passwordHasher.HashPassword(null!, plainPassword);

            return Task.FromResult((plainPassword, hashed));
        }
        public string GenerateSecureVerificationCode()
        {
            byte[] bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);
            int value = BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF;
            int code = value % 1000000;
            return code.ToString("D6");
        }
        private string GenerateRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            var sb = new StringBuilder();
            var rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                sb.Append(valid[rnd.Next(valid.Length)]);
            }

            return sb.ToString();
        }
    }
}
