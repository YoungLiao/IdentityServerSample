using System.Collections;
using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServerCenter
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "My Api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes = {"api"}
                },

                new Client()
                {
                    ClientId = "pwdClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"api"},
                    //也可以指定不要Secret
                    // RequireClientSecret = false
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "young",
                    Password = "wenwen520"
                }
            };
        }
    }
}