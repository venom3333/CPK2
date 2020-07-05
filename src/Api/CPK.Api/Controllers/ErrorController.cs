using System;
using System.Collections.Generic;
using System.Linq;
using CPK.Api.Models;
using CPK.SharedConfiguration;
using FluentValidationGuard;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;

namespace CPK.Api.Controllers
{
    [ApiController]
    [Route("api/v1/Error")]
    public sealed class ErrorController : ControllerBase
    {
        public const int PROBLEM_STATUS_CODE = 422;
        private readonly IErrorConverter _converter;

        public ErrorController(IErrorConverter converter)
        {
            _converter = converter;
        }

        private ActionResult Handle()
        {
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var details = _converter.Convert(exceptionHandlerPathFeature?.Error);

            switch (SharedConfig.EnvironmentNameEnum)
            {
                case EnvironmentNameEnum.Production:
                case EnvironmentNameEnum.Staging:
                    return new ObjectResult(GenerateErrorString(details));
                
                case EnvironmentNameEnum.Development:
                default:
                    return new ObjectResult(new ErrorModel
                    {
                        TraceIdentifier = HttpContext.TraceIdentifier,
                        Path = exceptionHandlerPathFeature?.Path,
                        Errors = details
                    }){ StatusCode = PROBLEM_STATUS_CODE };
            }
        }

        private string GenerateErrorString(List<Error> details)
        {
            var result = string.Join(' ', details.Select(d => d.Message));
            return result;
        }

        [HttpGet, HttpPost, HttpDelete, HttpHead, HttpOptions, HttpPatch, HttpPut]
        [ProducesResponseType(PROBLEM_STATUS_CODE, Type = typeof(ErrorModel))]
        public ActionResult Get() => Handle();
    }
}
