using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Validation
{
    public class ValidationAccountResult
    {
        public bool IsSuccess { get; init; }
        public string ValidationError { get; init; }
    }
}
