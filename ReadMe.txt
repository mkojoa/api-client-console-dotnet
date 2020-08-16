
## About Revenue Api Client

Revenue Api Client is a console application that allows client-to-client flow communication. 
..............................

## PROJECT DEPENDENCIES
  Target Framework  = netcoreapp2.2;
- [IdentityModel]
- [ Newtonsoft.Json]
- [Net.Http]

##	NOTE

Make sure you set the following:

1. [Authentication Address] found in GetDiscoveryDocumentAsync method.
	Eg: url

	var disco = await persolClientTokenRequest.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "url",  

                Policy =
                {
                    RequireHttps = false
                    //RequireHttps = true
                }
            });


2. [ClientID] , [ClientSecret] & [Scope] found in RequestClientCredentialsTokenAsync method in other to be authenticated.
	Eg: 
		-> ClientId = "id", 
		-> ClientSecret = "secret", 
		-> Scope = "scope-name"

		 var _tokenResponse = await persolClientTokenRequest
                .RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "id",
                ClientSecret = "secret",
                Scope = "scope-name"

            });


3. [Add Service URL] in other to make a request.
	Eg: http://psl-app-vm3/Revenue/api/v1.0/products/GetAllProduct

		var response = await persolClientApiCall
                .GetAsync("url")
                .ConfigureAwait(false);