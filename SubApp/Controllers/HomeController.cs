//===============================================================================
// Microsoft FastTrack for Azure
// Azure App Service Virtual Directory Samples
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SubApp.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace SubApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Dictionary<string, string> userInformation = new Dictionary<string, string>();

            foreach (var claim in ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims)
            {
                userInformation[claim.Type] = claim.Value;
            }

            ViewBag.UserInformation = userInformation;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
