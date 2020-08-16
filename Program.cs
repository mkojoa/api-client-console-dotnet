using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RegApiClient
{


    class Program
    {
        /**
         * Static variables
         * 
         * @private httpClient
         */
        private static HttpClient persolClientApiCall;
        private static HttpClient persolClientTokenRequest;

        public static async Task Main(string[] args)
        {
            /*
            |--------------------------------------------------------------------------
            |   Authentication & Token Process
            |--------------------------------------------------------------------------
            | 
            | Before a request can be made we need to pass an api-key and api-secret.
            | when this api-key and secret has been passed, we then make a request to 
            | the authentication server for a "secured encrypted token" once we
            | have successfully been authenticated.
            |
            */

            /**
             * The HttpClient acts as a base class for more specific HTTP clients..
             *
             * @var persolClientTokenRequest & persolClientApiCall
             */

            persolClientTokenRequest = persolClientApiCall = new HttpClient();


            /**
             * The URI that should be reachable when we need to be authenticated.
             * 
             * Set-up address = "Authentication Server" and httpSchema policy to a boolean value.
             * By default the httpSchema policy is set to false
             * 
             * @var disco
             */
            var disco = await persolClientTokenRequest.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
               // Address = "http://psl-app-vm3/RevenueIDP",  
                Address = "http://collect.localrevenue-gh.com/RevenueIDP",  

                Policy =
                {
                    RequireHttps = false
                    //RequireHttps = true
                }
            });
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }


            /**
             * We can now "request the encrypted token" from the authenticated server once the 
             * Client ID , SECRET & SCOPE are set
             * 
             * @var _tokenResponse
             */
            var _tokenResponse = await persolClientTokenRequest
                .RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                /*
                 *@var Address
                */
                Address = disco.TokenEndpoint,

                /*
                 *@var ClientId
                */
                ClientId = "emergentClient",

                /*
                 *@var ClientSecret
                */
                ClientSecret = "3585BDD3-09D7-4F1E-B581-6A0AD606D3F1",

                /*
                 *@var Scope
                */
                Scope = "revenueapi"

            });

            /**
             * Return encrypted Token if authenticated ELSE return a token error message
             * 
             * @return _tokenResponse
             */
            if (_tokenResponse.IsError)
            {
                Console.WriteLine(_tokenResponse.Error);
                return;
            }
            Console.WriteLine(_tokenResponse.Json);
            Console.WriteLine("\n\n");




            /*
            |--------------------------------------------------------------------------
            |   Make Api Calls
            |--------------------------------------------------------------------------
            |
            | Once we have the encrypted token, we can handle the incoming request [GET] 
            | and outgoing request[POST] and send the associated response back.
            |
            */

            /**
             * Grub the encrypted token if authenticated
             *
             * @var token
             */
            persolClientApiCall.SetBearerToken(_tokenResponse.AccessToken);

            /**
             *  Peform an HTTP GET request
             *  "gule" the encrypted token to the get,post request 
             *  
             *  @return data & statusCode 
             */
            var response = await persolClientApiCall
                .GetAsync("http://collect.localrevenue-gh.com/Revenue/api/v1.0/products/GetAllProduct")
                //.GetAsync("http://psl-app-vm3/Revenue/api/v1.0/products/GetAllProduct")
                .ConfigureAwait(false);

            /**
             *  Peform an HTTP GET request
             *  Else Return failed status code.
             *  
             *  @return data & statusCode
             */
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            { 
                var content = await response
                    .Content
                    .ReadAsStringAsync();

                Console.WriteLine(JArray.Parse(content));
            }


            /**
             *  Peform an HTTP POST request
             *  "gule" the encrypted token to the get,post request 
             *  
             *  @return success with statusCode
             */

            // follow the same process to make a post request
        }
    }

}
