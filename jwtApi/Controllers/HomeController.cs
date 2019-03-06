using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using jwtApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace jwtApi.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/version")]
        public ActionResult<string> About()
        {
            var version = typeof(Startup).Assembly.GetName().Version.ToString();

            return Ok(version);
        }

        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var creationDate = System.IO.File.GetCreationTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {creationDate}");
        }

        [HttpGet("/error-codes")]
        public ActionResult<IDictionary<int, string>> ErrorCodes()
        {
            var values = Enum
                .GetValues(typeof(DomainErrorCode))
                .Cast<DomainErrorCode>();

            var result = new Dictionary<int, string>();

            foreach (var v in values)
                result.Add((int)v, v.ToString());

            return result;
        }
    }
}