using Blazored.LocalStorage;
using BlazorGameApp.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorGameApp.Client
{
    public class CustomeAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly IBananaService _bananaService;

        public CustomeAuthStateProvider(ILocalStorageService LocalStorage, HttpClient http, IBananaService bananaService)
        {
            _localStorage = LocalStorage;
            _http = http;
            _bananaService = bananaService;
        }
        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    if (await _localStorage.GetItemAsync<bool>("isAuthenticated")){
        //        var identity = new ClaimsIdentity(new[] {
        //            new Claim(ClaimTypes.Name,"Chinmay")
        //        }, "test authentication type");

        //        var user = new ClaimsPrincipal(identity);
        //        var state = new AuthenticationState(user);

        //        NotifyAuthenticationStateChanged(Task.FromResult(state));
        //        return state;
        //    }
        //    return new AuthenticationState(new ClaimsPrincipal());
        //}
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authToken = await _localStorage.GetItemAsStringAsync("authToken");

            var identity = new ClaimsIdentity();

            // Reset the Authorization header on the HttpClient.
            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken)) {
                try {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"",""));
                    await _bananaService.GetBananas();
                }
                catch(Exception) {
                    //if invalid token
                    await _localStorage.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
             
            }
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            // Notify that the authentication state has changed
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;

            //var state = new AuthenticationState(new ClaimsPrincipal());
            //if (await _localStorage.GetItemAsync<bool>("isAuthenticated"))
            //{
            //    var identity = new ClaimsIdentity(new[] {
            //        new Claim(ClaimTypes.Name,"Chinmay")
            //    }, "test authentication type");

            //    var user = new ClaimsPrincipal(identity);
            //    state = new AuthenticationState(user);
            //}
            //NotifyAuthenticationStateChanged(Task.FromResult(state));
            //return state;
        }

        // Helper method to parse Base64 without padding.
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        // Helper method to parse claims from a JWT token.
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

            return claims;
        }

        //public override  Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    var identity = new ClaimsIdentity(new[] {

        //        new Claim(ClaimTypes.Name,"Chinmay")
        //    }, "test authentication type");

        //    var user = new ClaimsPrincipal(identity);
        //    return Task.FromResult(new AuthenticationState(user));//returns current user info

        //    return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));//no authentication,unauthorized
        //}
    }
}
//register this class in program.cs
//CascadingAuthenticationState and AuthorizeRouteView in app.razor
