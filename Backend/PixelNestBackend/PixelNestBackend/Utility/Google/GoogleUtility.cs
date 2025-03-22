using PixelNestBackend.Dto.Google;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace PixelNestBackend.Utility.Google
{
    public class GoogleUtility
    {
        private readonly IConfiguration _configuration;

        public GoogleUtility(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public async Task<GoogleTokenResponse> GetGoogleToken(string code)
        {
            using var client = new HttpClient();
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
            {

                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "code", code },
                    {"client_id", _configuration["Google:ClientId"] },
                    {"client_secret", _configuration["Google:ClientSecret"]},
                    {"redirect_uri", "http://localhost:7157/api/Authentication/SigninGoogle"},
                    {"grant_type", "authorization_code" }


                })

            };
            var response = await client.SendAsync(tokenRequest);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var googleTokenResponse = JsonSerializer.Deserialize<GoogleTokenResponse>(content);

            return googleTokenResponse;
        }
        public GoogleAccountDto GenerateGoogleAccountDto(string googleToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(googleToken) as JwtSecurityToken;

            var name = jsonToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var givenName = jsonToken.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
            var familyName = jsonToken.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;
            var picture = jsonToken.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
            var userId = jsonToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var email = jsonToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            GoogleAccountDto googleAccountDto = new GoogleAccountDto
            {
                Email = email,
                Firstname = givenName,
                Lastname = familyName,
                Picture = picture,
            };

            return googleAccountDto;
        }
    }
}
