using System;
using IdentityModel.Client;
using System.Threading.Tasks;
using System.Net.Http;


namespace PwdClientDemo
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
            if (disco.IsError) throw new Exception(disco.Error);    

            Console.WriteLine($"TockenEndPoint:{disco.TokenEndpoint}");
            
            //https://identitymodel.readthedocs.io/en/latest/client/token.html#requesting-a-token-using-the-password-grant-type
            var tockenReponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "pwdClient",
                ClientSecret = "secret",
                Scope = "api",

                UserName = "young",
                Password = "wenwen520"
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
                Console.WriteLine($"StatusCode: {result.StatusCode}");
                Console.WriteLine($"Content: {result.Content.ReadAsStringAsync().Result}");
            }

            Console.ReadLine();
        }
    }
}
