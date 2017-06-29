using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Keycloak;
using KendoUIApp1;

[assembly: OwinStartup(typeof(KendoUIApp1.Startup))]

namespace KendoUIApp1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            const string persistentAuthType = "keycloak_cookies"; // Or name it whatever you want

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = persistentAuthType
            });

            // You may also use this method if you have multiple authentication methods below,
            // or if you just like it better:
            app.SetDefaultSignInAsAuthenticationType(persistentAuthType);

            app.UseKeycloakAuthentication(new KeycloakAuthenticationOptions
            {
                Realm = "DHS",

                ClientId = "DHS-Proto",
                ClientSecret = "352c280d-68cb-4f4c-9331-571407a768df",
                KeycloakUrl = "http://key.purvisms.com:8080/auth",
                SignInAsAuthenticationType = persistentAuthType // Not required with SetDefaultSignInAsAuthenticationType
            });
        }
    }
}