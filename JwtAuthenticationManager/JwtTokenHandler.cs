using Clients;
using IdentityModel;
using IdentityModel.Client;
using JwtAuthenticationManager.Helper;
using JwtAuthenticationManager.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

using System.Text.Json;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        private const int JWT_TOKEN_VALIDITY_MINS = 20;

        public JwtTokenHandler()
        {

        }
        public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest authenticationRequest)
        {
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            return new AuthenticationResponse
            {
                IdentityId = authenticationRequest.IdentityId,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                Token = GetAccessToken(authenticationRequest.clientId,authenticationRequest.clientSecret).Result.AccessToken.ToString()
            };
        }
        private static async Task<TokenResponse> GetAccessToken(string clientId,string clientSecret)
        {
            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(Constants.Authority);
                if (disco.IsError) throw new Exception(disco.Error);

                var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,

                    ClientId = clientId,
                    ClientSecret = clientSecret,
                });

                if (response.IsError) throw new Exception(response.Error);
                return response;
            }
        }
    }
}
