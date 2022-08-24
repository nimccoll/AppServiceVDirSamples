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
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RootB2CApp.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            Dictionary<string, string> userInformation = new Dictionary<string, string>();

            foreach (var claim in ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims)
            {
                userInformation[claim.Type] = claim.Value;
            }

            ViewBag.UserInformation = userInformation;

            return View();
        }

        public ActionResult Logout()
        {
            List<AuthenticationDescription> authenticationTypes = HttpContext.GetOwinContext().Authentication.GetAuthenticationTypes().ToList();
            string[] authenticationTypeNames = authenticationTypes.Select(a => a.AuthenticationType).ToArray();
            HttpContext.GetOwinContext().Authentication.SignOut(authenticationTypeNames);

            return View();
        }

        public ActionResult LoggedOut()
        {
            return View();
        }
    }
}
