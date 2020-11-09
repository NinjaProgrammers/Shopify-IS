using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statuscode}")]
        public IActionResult HttpStatusCodeHandler(int statuscode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            ErrorViewModel viewModel = new ErrorViewModel();
            switch (statuscode)
            {
                case 404:
                    viewModel.ErrorTitle = "Oops! Page not found.";
                    viewModel.ErrorMessage = "We could not find the page you were looking for.";
                    viewModel.Path = statusCodeResult.OriginalPath;
                    viewModel.QueryString = statusCodeResult.OriginalQueryString;
                    break;
            }
            return View("NotFound",viewModel);
        }

        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ErrorViewModel viewModel = new ErrorViewModel
            {
                ErrorTitle = "Error!",
                ErrorMessage = exceptionDetails.Error.Message,
                Path = exceptionDetails.Path,
                StackTrace = exceptionDetails.Error.StackTrace
            };
            return View("Error",viewModel);
        }
    }
}
