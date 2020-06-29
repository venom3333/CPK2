using System.Collections.Generic;
using FluentValidationGuard;

namespace CPK.Api.Models
{
    public sealed class ErrorModel
    {
        public string TraceIdentifier { get; set; }
        public string Path { get; set; }
        public List<Error> Errors { get; set; }
    }
}
