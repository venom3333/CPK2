using CPK.Api.Models;
using FluentValidationGuard;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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
            return new ObjectResult(new ErrorModel
            {
                TraceIdentifier = HttpContext.TraceIdentifier,
                Path = exceptionHandlerPathFeature?.Path,
                Errors = details
            })
            { StatusCode = PROBLEM_STATUS_CODE };
        }

        [HttpGet, HttpPost, HttpDelete, HttpHead, HttpOptions, HttpPatch, HttpPut]
        [ProducesResponseType(PROBLEM_STATUS_CODE, Type = typeof(ErrorModel))]
        public ActionResult Get() => Handle();
    }
}
