using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
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
