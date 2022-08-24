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
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(RootApp.Startup))]

namespace RootApp
{
    public class Startup
    {
        private readonly string _clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private readonly string _authority = string.Format("{0}/{1}/", ConfigurationManager.AppSettings["ida:Authority"], ConfigurationManager.AppSettings["ida:Tenant"]);

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions() { CookieName = "RootApp" });

            OpenIdConnectAuthenticationNotifications notifications = new OpenIdConnectAuthenticationNotifications()
            {
                AuthenticationFailed = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect("~/Error.aspx?message=" + context.Exception.Message);
                    return Task.FromResult(0);
                }
            };

            // If a domain has been configured, pass a domain hint to the identity provider to bypass home realm discovery
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ida:Domain"]))
            {
                notifications.RedirectToIdentityProvider = (context) =>
                {
                    context.ProtocolMessage.DomainHint = ConfigurationManager.AppSettings["ida:Domain"];
                    return Task.FromResult(0);
                };
            }

            string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
            
            app.UseOpenIdConnectAuthentication(
                 new OpenIdConnectAuthenticationOptions
                 {
                     ClientId = _clientId,
                     Authority = _authority,
                     Notifications = notifications,
                     RedirectUri = redirectUri
                 });
        }
    }
}
