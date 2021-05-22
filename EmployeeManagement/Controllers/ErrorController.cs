using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var expectionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($" Path :{expectionHandlerFeature.Path} Error : {expectionHandlerFeature.Error} ");
            return View("Error");
        }

        [Route("{statusCode}")]
        public IActionResult HttpStatusCodehandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the requested page could not be found.";
                    return View("NotFound");
            }
            return View();
        }
        
    }
}
