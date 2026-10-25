using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionDigest.Application
{
    public class OperationResult<T>
    {
        public T? Result { get; set; }
        public bool Success { get; set; } = true;
        public string? ErrorMessage { get; set; }
    }

    public class OperationResult
    {
        public bool Success { get; set; } = true;
        public string? ErrorMessage { get; set; }
    }
}
