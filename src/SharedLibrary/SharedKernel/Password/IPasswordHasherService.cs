using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.SharedKernel.Password
{
    public interface IPasswordHasherService
    {
        Task<string> HashPassword(string password);
        Task<string> GenerateSecureVerificationCode();
        Task<bool> VerifyPassword(string hashedPassword, string providedPassword);
        Task<(string PlainPassword, string HashedPassword)> GenerateAndHashPassword(int length = 10);

    }
}
