using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ThirdPartyDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //HttpClient 无需用using包起来
            //https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
            if (disco.IsError) throw new Exception(disco.Error);

            var tockenReponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api"
            });

            if(tockenReponse.IsError)
            {
                Console.WriteLine(tockenReponse.Error);
            }
            else
            {
                Console.WriteLine(tockenReponse.Json);
            }

            client.SetBearerToken(tockenReponse.AccessToken);

            var result = await client.GetAsync("https://localhost:5002/api/values");

            if(result.IsSuccessStatusCode)
            {
                Console.WriteLine(result.Content.ReadAsStringAsync().Result);
            }

            Console.ReadLine();
        }
    }
}
