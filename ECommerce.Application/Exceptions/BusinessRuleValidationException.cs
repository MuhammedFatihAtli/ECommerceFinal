using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Exceptions
{
    public class BusinessRuleValidationException : Exception
    {
        public BusinessRuleValidationException(string message) : base(message)
        {
            
        }
        public BusinessRuleValidationException(string code, string message) : base ($"{code}: {message}")
        {
            Code = code;
        }
        public string? Code { get; set; }
    }
}
