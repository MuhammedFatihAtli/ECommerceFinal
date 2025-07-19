using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application
{
    public class AuthResult
    {
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public List<string> Errors { get; private set; } = new();

        public static AuthResult Success()
        {
            return new AuthResult { IsSuccess = true };
        }

        public static AuthResult Failure(string error)
        {
            return new AuthResult
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }

        public static AuthResult Failure(List<string> errors)
        {
            return new AuthResult
            {
                IsSuccess = false,
                Errors = errors
            };
        }
    }

}
