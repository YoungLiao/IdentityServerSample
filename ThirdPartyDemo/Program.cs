using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ThirdPartyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var diso = DiscoveryClient.GetAsync("https://localhost:5000").Result;
            if(diso.IsError)
            {
                Console.WriteLine(diso.Error);
            }

            var tockenClient = new TokenClient(diso.TokenEndpoint, "client", "secret");

            var tockenReponse = tockenClient.RequestClientCredentialsAsync("api").Result;

            if(tockenReponse.IsError)
            {
                Console.WriteLine(tockenReponse.Error);
            }
            else
            {
                Console.WriteLine(tockenReponse.Json);
            }

            using (HttpClient client = new HttpClient()) 
            {
                client.SetBearerToken(tockenReponse.AccessToken);

                var response = client.GetAsync("https://localhost:5002/api/values").Result;

                if(response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
            }

            // using (HttpClient client = new HttpClient()) 
            // {
            //     var disc = client.GetAsync("https://localhost:5000").Result;
            //     if(!disc.IsSuccessStatusCode)
            //     {
            //         Console.WriteLine(disc.StatusCode);
            //     }

            //     Console.WriteLine($"{disc.IsSuccessStatusCode},{disc.StatusCode}");
            // }
            
            

            Console.ReadLine();
        }
    }
}
