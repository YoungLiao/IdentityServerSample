﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ThirdPartyDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
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

            var result = client.GetAsync("https://localhost:5002/api/values/5").Result;

            if(result.IsSuccessStatusCode)
            {
                Console.WriteLine(result.Content.ReadAsStringAsync().Result);
            }

            Console.ReadLine();
        }
    }
}
