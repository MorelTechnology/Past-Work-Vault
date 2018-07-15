using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.IDP
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                //new IdentityResource("activeDirectory", "Get additional properties from Active Directory", new List<string>() {"id" })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("commutationCentralApi", "Commutation Central API")
                {
                    ApiSecrets = {new Secret("secret".Sha256()) },
                    UserClaims = new List<string> {"id" }
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                //Postman Client
                new Client
                {
                    ClientId = "postman",
                    ClientName = "Postman Test Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = {"https://www.getpostman.com/oauth2/callback" },
                    PostLogoutRedirectUris = {"https://www.getpostman.com" },
                    AllowedCorsOrigins = {"https://www.getpostman.com" },
                    EnableLocalLogin = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "commutationCentralApi"
                    },
                    ClientSecrets = new List<Secret>() {new Secret("secret".Sha256())}
                },
                // Angular Client
                new Client
                {
                    ClientId = "commutationCentralApp",
                    ClientName = "Commutation Central App",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AccessTokenType = AccessTokenType.Reference,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris = {"http://localhost:4200/auth-callback", "http://localhost:4200/silent-refresh.html" },
                    PostLogoutRedirectUris = {"http://localhost:4200/signout-callback" },
                    AllowedCorsOrigins = {"http://localhost:4200" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "commutationCentralApi"
                    }
                }
            };
        }
    }
}
