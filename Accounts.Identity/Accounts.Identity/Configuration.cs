using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Identity
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> //Scope - represents what client-app can use
            {
                new ApiScope("AccountsWebAPI", "Web Api")
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource> //Identity Resourse - represents ability of client-app
                                       //to look through subset of 'user statements'
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile() //such as username or birth date
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource> //Api Resource represents permission to all protected resource
                                  //which we are allowed to request access
            {
                new ApiResource("AccountsWebAPI", "Web API", new []
                    {JwtClaimTypes.Name})
                {
                    Scopes = {"AccountsWebApi"}
                }
            };
        public static IEnumerable<Client> Clients =>
            new List<Client> //who are allowed to request access from us
            {
                new Client
                {
                    ClientId = "accounts-web-api", //same id as on client-app
                    ClientName = "Accounts Web",
                    AllowedGrantTypes = GrantTypes.Code, //how client interact with token-service 
                    RequireClientSecret = false, //if it's true - SHA256 string, which
                                                 //must be same on a client
                    RequirePkce = true, //requires a key for authorization code
                    RedirectUris = //list of and uris we will redirect to
                                   //after clien-app authenfication
                    {
                        "http://.../signin-oidc"
                    },
                    AllowedCorsOrigins = //list of uris, who allowed to use identity server
                    {
                        "http://..."
                    },
                    PostLogoutRedirectUris = //list of uris we will redirect to
                                             //after client-app log out
                    {
                        "http:/.../signout-oidc"
                    },
                    AllowedScopes = //available scopes for client-app
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "AccountsWebAPI"
                    },
                    AllowAccessTokensViaBrowser = true //runs transfering token via browser
                }
            };
    }
}
